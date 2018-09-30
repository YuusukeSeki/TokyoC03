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

    public enum DebufState
    {
        NONE,
        NG_JUMP,
        NG_LETTERBULLET,
    };

    public DebufState _debufState;
    float _cntDebufTime;

    public int _nowChara;                           //　現在操作中のキャラ
    public List<GameObject> _charaLists = null;     //　操作可能なゲームキャラクター
    public State _state;                            // 残りキャラクターの有無
    //FadeScript _fs;
    CameraFixing _cf;

    float _changeTime = 0.3f;   // 交代にかかる時間

    float _entryCharaSpeed;                     // 交代後のキャラクタのスピード
    float _exitCharaSpeed;                      // 交代前のキャラクタのスピード
    Vector3 _targetPos = new Vector3(0, 0, 0);  // 交代が完了する目標座標

    Vector3 _respownPosition;   // 復帰時の座標

    [SerializeField] GameManager _gameManager;
    [SerializeField] UIManager _uiManager;


    // Use this for initialization
    void Start()
    {
        //_fs = GameObject.Find("FadePanel").GetComponent<FadeScript>();
        _cf = GameObject.Find("Main Camera").GetComponent<CameraFixing>();
        _respownPosition = transform.position;

        Init();

    }

    // Update is called once per frame
    void Update()
    {
        // 初期化判定
        if(_state == State.CLEAR || _state == State.GAMEOVER)
        {
            //if (_fs.GetFadeState() == FadeScript.FadeState.FADE_OUT_COMPRETED)
            //{
            //    Init();
            //}

            //Init();

            return;
        }

        // デバフ状態の変化
        CheckDebuf();

        // キャラクタ状態の変化
        CheckCharacterState();

    }

    // 初期化処理
    public void Init()
    {
        _state = State.NONE;

        ChangeCharacter(_nowChara);

        for (int i = 0; i < _charaLists.Count; i++)
        {
            _charaLists[i].GetComponent<Player>().Init();
            _charaLists[i].transform.position = _respownPosition;

            if (_nowChara == i)
                _charaLists[i].SetActive(true);
            else
                _charaLists[i].SetActive(false);
        }

        _cf.FocusGameObject(_charaLists[_nowChara]);

        _cntDebufTime = 0;
    }

    //　操作キャラ変更
    public void ChangeCharacter(int nextChara)
    {
        if (nextChara == _nowChara)
            return;

        //　範囲外時は0番
        if (nextChara >= _charaLists.Count || nextChara < 0)
            nextChara = 0;

        // 指定キャラ死亡時、生きているキャラを探す
        if(GetCharacterParamater(nextChara)._hp <= 0)
        {
            for (int i = 0; i < _charaLists.Count; i++)
            {
                // 生きているキャラ発見
                if (GetCharacterParamater(i)._hp > 0)
                {
                    nextChara = i;

                    if (nextChara == _nowChara)
                        return;
                    else
                        break;
                }
                // 全員死亡
                else if(i == _charaLists.Count - 1)
                {
                    _state = State.GAMEOVER;
                    //_fs.SetColor(0, 0, 0);
                    //_fs.StartFadeOut();
                    return;
                }
            }
        }

        // キャラクタのアクティブ変更
        for (int i = 0; i < _charaLists.Count; i++)
        {
            bool flag;

            if (i == nextChara || i == _nowChara)
                flag = true;
            else
                flag = false;

            _charaLists[i].SetActive(flag);

        }

        // バリア追従者変更
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        foreach (GameObject barrier in barriers)
        {
            barrier.GetComponent<Barrier>().SetBarrier(_charaLists[nextChara]);

        }


        // 交代キャラクタとカメラの動きを設定
        SetMoveRoot(nextChara);

        _nowChara = nextChara;

    }

    // キャラ状態監視
    void CheckCharacterState()
    {
        // クリア時
        if (Player._isClear)
        {
            _state = State.CLEAR;

            //_fs.SetColor((192f / 255f), (192f / 255f), (192f / 255f));
            //_fs.StartFadeOut();

        }

        // 死亡時
        if (GetCharacterParamater(_nowChara)._hp <= 0)
        {
            ChangeCharacter(_nowChara + 1);

            if(_state != State.GAMEOVER)
            {
                for (int i = 0; i < _charaLists.Count; ++i)
                {
                    if (_uiManager.playerPos[i] == _nowChara)
                    {
                        _uiManager.OnIconClick(i);
                        break;
                    }
                }
            }
        }

        // 穴落ちた時
        if (Player._isAllDead)
        {
            _state = State.GAMEOVER;

            //_fs.SetColor(0, 0, 0);
            //_fs.StartFadeOut();
        }

        // ＭＰ満タン時
        for (int i = 0; i < _charaLists.Count; ++i)
        {
            if (i == _nowChara)
            {
                if(_gameManager.playerMPs[i] >= 1)
                {
                    _charaLists[_nowChara].GetComponent<Player>().SetEffect(true);
                }
                else
                {
                    _charaLists[_nowChara].GetComponent<Player>().SetEffect(false);
                }

                break;
            }
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
        //initPos.y = _charaLists[_nowChara].transform.position.y;                                                               // y座標は交代前のキャラに合わせる
        initPos.y = _cf.transform.position.y;                                                               // y座標は交代前のキャラに合わせる
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


    }

    // 操作中のキャラクタをジャンプさせる
    public void Jump()
    {
        if (_debufState == DebufState.NG_JUMP)
        {
            Debug.Log("NG_JUMP");
            return;
        }

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

    // 指定キャラが死んでるかどうか
    // numCharacter : 指定キャラクタ番号(0~3)
    // 返り値：true  - 死んでる
    //         false - 死んでない
    public bool isDead(int numCharacter)
    {
        if(numCharacter < 0 || numCharacter >= _charaLists.Count)
        {
            Debug.Log("ERROR!! : Alive(int numCharacter)");
            return false;
        }

        if (GetCharacterParamater(numCharacter)._hp <= 0)
            return true;
        else
            return false;
    }

    //public void SetDebuf(Enemy.Debuf debuf, float time)
    public void SetDebuf(float time)
    {
        //switch (debuf)
        //{
        //    case Enemy.Debuf.NG_LETTERBULLET:
        _debufState = DebufState.NG_LETTERBULLET;
        for (int i = 0; i < _charaLists.Count; i++)
        {
            _charaLists[i].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1);
        }
        //        break;

        //    case Enemy.Debuf.NG_JUMP:
        //        _debufState = DebufState.NG_JUMP;
        //        for (int i = 0; i < _charaLists.Count; i++)
        //        {
        //            _charaLists[i].GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1);
        //        }
        //        break;

        //}

        _cntDebufTime = time;

    }

    void CheckDebuf()
    {
        if (_cntDebufTime <= 0)
            return;

        _cntDebufTime -= Time.deltaTime;

        if (_cntDebufTime <= 0)
        {
            _debufState = DebufState.NONE;
            for (int i = 0; i < _charaLists.Count; i++)
            {
                _charaLists[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            }
        }

    }


}
