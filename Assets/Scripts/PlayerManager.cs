using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public enum State
    {
        NONE,
        CLEAR,
        GAMEOVER,
    };

    public bool isUseDebugButton;

    public int _nowChara;                           //　現在操作中のキャラ
    public List<GameObject> _charaLists = null;     //　操作可能なゲームキャラクター
    public State _state;                            // 残りキャラクターの有無
    FadeScript _fs;
    CameraFixing _cf;

    float _changeTime = 0.3f;   // 交代にかかる時間

    float _entryCharaSpeed;                     // 交代後のキャラクタのスピード
    float _exitCharaSpeed;                      // 交代前のキャラクタのスピード
    Vector3 _targetPos = new Vector3(0, 0, 0);  // 交代が完了する目標座標

    Vector3 _respownPosition;   // 復帰時の座標


    // Use this for initialization
    void Start()
    {
        _fs = GameObject.Find("FadePanel").GetComponent<FadeScript>();
        _cf = GameObject.Find("Main Camera").GetComponent<CameraFixing>();
        ChangeCharacter(_charaLists.Count);

        Init();

    }

    // Update is called once per frame
    void Update()
    {
        // クリアかゲームオーバーで初期化処理
        if(_state == State.CLEAR || _state == State.GAMEOVER)
        {
            if (_fs.GetFadeState() == FadeScript.FadeState.FADE_OUT_COMPRETED)
            {
                Init();
            }

            return;
        }

        // キャラクタの状態をチェックする
        CheckCharacterState();

        if (!isUseDebugButton)
            return;

        //　キーが押されたら操作キャラクターを変更する
        if(_state != State.CLEAR && _state != State.GAMEOVER)
        {
            if (Input.GetKeyDown("1"))
            {
                ChangeCharacter(0);
            }
            else if (Input.GetKeyDown("2"))
            {
                ChangeCharacter(1);
            }
            else if (Input.GetKeyDown("3"))
            {
                ChangeCharacter(2);
            }
            else if (Input.GetKeyDown("4"))
            {
                ChangeCharacter(3);
            }
            else if (Input.GetKeyDown("s"))
            {
                Skill();
            }
        }
    }

    // 初期化処理
    public void Init()
    {
        _state = State.NONE;

        ChangeCharacter(0);

        for (int i = 0; i < _charaLists.Count; i++)
        {
            _charaLists[i].GetComponent<Player>().Init();

            if (_nowChara == i)
                _charaLists[i].SetActive(true);
            else
                _charaLists[i].SetActive(false);
        }

        _cf.SetNowCharacterPosition();

    }

    //　操作キャラクター変更メソッド
    public void ChangeCharacter(int nextChara)
    {
        // キャラクタに変更がなければ処理を行わない
        if (nextChara == _nowChara)
            return;

        bool flag;  //　オン・オフのフラグ
        
        //　指定されたキャラクターが範囲外なら最初のキャラを操作キャラにする
        if (nextChara >= _charaLists.Count || nextChara < 0)
        {
            nextChara = 0;
        }

        // 指定されたキャラクターが死んでいたらキャラリストで一番若い番号の生きているものをメインキャラに設定する
        if(_charaLists[nextChara].GetComponent<Player>()._state == Player.State.DEAD)
        {
            for (int i = 0; i < _charaLists.Count; i++)
            {
                // 生きている人を発見したら、次のキャラクターに設定してループを抜ける
                if (_charaLists[i].GetComponent<Player>()._state != Player.State.DEAD)
                {
                    nextChara = i;
                    Debug.Log("ChangeChara : Dead & Next ");
                    break;
                }

                // 全員死んでいたらGameOver
                if(i == _charaLists.Count - 1)
                {
                    _state = State.GAMEOVER;
                    Debug.Log("ChangeChara : GameOver ");
                    _fs.SetColor(0, 0, 0);
                    _fs.StartFadeOut();
                    return;
                }
            }
        }

        //　次か今の操作キャラクターだったらアクティブをONにし、それ以外だったらOFFにする
        for (int i = 0; i < _charaLists.Count; i++)
        {
            if (i == nextChara || i == _nowChara)
            {
                flag = true;

            }
            else
            {
                flag = false;
            }

            //　操作するキャラクターと操作しないキャラクターで機能のオン・オフをする
            _charaLists[i].SetActive(flag);

        }

        // 交代中のキャラクタとカメラの動きを設定
        SetMoveRoot(nextChara);

        //　次の操作キャラクターを現在操作しているキャラクターに設定して終了
        _nowChara = nextChara;

    }

    // キャラクタ状態監視メソッド
    void CheckCharacterState()
    {
        // クリアしていたら、フェードアウト終了時にプレイヤーを初期化する
        if (_charaLists[_nowChara].GetComponent<Player>()._state == Player.State.CLEAR)
        {
            _state = State.CLEAR;

            _fs.SetColor((192f / 255f), (192f / 255f), (192f / 255f));
            _fs.StartFadeOut();

        }

        // 死亡していたらキャラクタを次のキャラクタに変える。いなければGameOver
        if (_charaLists[_nowChara].GetComponent<Player>()._state == Player.State.DEAD)
        {
            ChangeCharacter(_nowChara + 1);

        }

        // 穴に落ちて死んだら、残りのキャラクタの生死を問わずGameOver
        if(_charaLists[_nowChara].GetComponent<Player>()._state == Player.State.FALL_DEAD)
        {
            _fs.SetColor(0, 0, 0);
            _fs.StartFadeOut();

            _state = State.GAMEOVER;
        }

    }

    // 交代中のキャラクターやカメラの動き方を設定
    void SetMoveRoot(int nextChara)
    {
        // 座標を前のキャラに合わせる
        // ①目標座標を設定する
        // 　１）目標座標 = 今のキャラクターの座標 + 次のキャラクターのスピード * 交代にかかる時間
        // ②次のキャラクターに登場するように通知する。その際、初期座標と目標座標も合わせて通知する
        // ③今のキャラクターに退場するように通知する
        // ④カメラに交代中であることを通知し、目標座標に向けて緩やかに動かさせる
        //　 １）交代中であることと目標座標を通知
        //   ２）カメラのスピードを合わせる（カメラ側の処理）
        // ⑤交代が終わったらカメラに交代が終了したことを通知する
        //   １）カメラの追従座標を次（今）のキャラクターにする

        // ① 目標地点の算出
        _targetPos = _charaLists[_nowChara].transform.position;
        _targetPos.x += _charaLists[nextChara].GetComponent<Player>()._paramater._runMaxSpeed * _changeTime;

        // ② 初期座標の算出し、次のキャラクターに設定
        Vector3 initPos = _cf.transform.position;
        initPos.x -= _cf._screenSize.x * 0.5f + _charaLists[nextChara].GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;    // x座標は左端の画面外
        initPos.y = _charaLists[_nowChara].transform.position.y;                                                               // y座標は交代前のキャラに合わせる
        initPos.z = 0;                                                                                                         // z座標は0
        _charaLists[nextChara].GetComponent<Player>().SetEntry(initPos, _targetPos, _changeTime);

        // ③ 今のキャラクターを退場させる
        if(nextChara != _nowChara)
            _charaLists[_nowChara].GetComponent<Player>().SetExit();

        // ④ カメラの動きの算出し、設定
        Vector2 movePos = new Vector2(0, 0);
        movePos.x = _targetPos.x - _charaLists[_nowChara].transform.position.x;
        _cf.CalledChangeCharacter(movePos, _changeTime);

        // ⑤カメラ側でやります


        // 以下旧式（その場で一瞬で切り替わる）
        // _charaLists[nextChara].transform.position = _charaLists[_nowChara].transform.position;

        //// 状態を変更する
        //_charaLists[nextChara].GetComponent<Player>()._state = Player.State.ENTRY;

        //if (_charaLists[_nowChara].GetComponent<Player>()._state != Player.State.DEAD)
        //    _charaLists[_nowChara].GetComponent<Player>()._state = Player.State.EXIT;

    }

    // 操作中のキャラクタをジャンプさせる
    public void Jump()
    {
        _charaLists[_nowChara].GetComponent<Player>().Jump();
    }

    // 操作中のキャラクタのスキル使用
    public void Skill()
    {
        _charaLists[_nowChara].GetComponent<Player>().Skill();
    }

    // 操作中のキャラクタ座標を取得
    public Vector3 GetMainCharacterPosition()
    {
        return _charaLists[_nowChara].transform.position;
    }

    // キャラクタパラメータ取得
    public Player.Paramater GetCharacterParamater(int numCharacter)
    {
        if (numCharacter < 0 || numCharacter >= _charaLists.Count)
        {
            Debug.Log("GetCharacterParamater(int numCharacter)\n" + numCharacter + ": は範囲外の番号です。現在のキャラクタのパラメータを返します。");
            return _charaLists[_nowChara].GetComponent<Player>()._paramater; ;
        }

        return _charaLists[numCharacter].GetComponent<Player>()._paramater;
    }

}
