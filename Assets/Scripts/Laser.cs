using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    [SerializeField] int _attackPower;              // 攻撃力
    float _scaleX_Max;                              // 伸ばした時の、拡縮率Xの最大値
    [SerializeField] float _growTime;               // 伸び終わるまでの時間
    [SerializeField] float _stayTime;               // 伸び終わってから回るまでの時間
    Quaternion[] _rotateRoot;                       // 回転経路
    [SerializeField] float _rotateTime;             // 回転が終わるまでの時間
    [SerializeField] float _eraseTime;              // 回転が終わってから消えるまでの時間
    int _currentRoot;                               // 現在の経路
    float _cntTime;                                 // 時間計測バッファ

    Quaternion _startQuaternion, _endQuaternion;    // 開始時の回転率、終了時の回転率

    [SerializeField] float _rotateAxisZ;            // 回転軸Z値


    enum State
    {// 状態
        START,  // 撃ち始め
        STAY,   // 待機
        ROTATE, // 回転中
        END,    // 回転終了
    };

    State _state;   // 現在の状態

    const int ROOT_MAX = 1;

    [SerializeField] GameObject _parent;    // 親オブジェクトである基点
    GameObject _creator;                    // 自分を生成したオブジェクト



	// Use this for initialization
	void Start () {
        // 回転経路の生成
        _rotateRoot = new Quaternion[ROOT_MAX];
        _rotateRoot[0] = Quaternion.AngleAxis(-180 , new Vector3(0, 1, _rotateAxisZ));
        //_rotateRoot[1] = Quaternion.AngleAxis(-180, new Vector3(0, 1, 0.3f));

        // どこまで伸びればいいのかを求める
        Vector3 rightBottom = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        Vector3 leftTop = GameObject.Find("Main Camera").GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero);
        _scaleX_Max = (rightBottom.x - leftTop.x) / GetComponent<SpriteRenderer>().bounds.size.x;

        // 現在のルート
        _currentRoot = 0;

        // 現在の状態の設定
        _state = State.START;

        // 始点と終点の設定
        _startQuaternion = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
        _endQuaternion = _rotateRoot[_currentRoot];

        // 時間計測バッファのクリア
        _cntTime = 0;

        // 基点のスケール値Xをクリア
        _parent.transform.localScale = new Vector3(0, 1, 1);

	}
	
	// Update is called once per frame
	void Update () {

        // 状態の変更
        ChangeState();

        // 時間経過処理
        ElapsedTime();

        // 現在の状態を見て、処理を変える
        switch(_state)
        {
            case State.START:
                Grow();         // 伸ばす
                break;

            case State.STAY:
                // 無処理
                break;

            case State.ROTATE:
                Rotate();       // 回転させる
                break;

            case State.END:
                SelfDelete();   // 消え待ち
                break;

        }

        // 生成者との座標を同期する
        Follow();

    }

    // 時間経過処理
    void ElapsedTime()
    {
        float mag = 0;

        if (_cntTime >= 1)
            _cntTime = 0;

        switch (_state)
        {
            case State.START:
                mag = _growTime;
                break;

            case State.STAY:
                mag = _stayTime;
                break;

            case State.ROTATE:
                mag = _rotateTime / ROOT_MAX;
                break;

            case State.END:
                mag = _eraseTime;
                break;

        }

        _cntTime += Time.deltaTime / mag;

    }

    // 伸ばす
    void Grow()
    {
        Vector3 scale = new Vector3(1, 1, 1);
        scale.x = Mathf.Lerp(0, _scaleX_Max, _cntTime * _cntTime);
        _parent.transform.localScale = scale;

    }

    // 回転
    void Rotate()
    {
        _parent.transform.rotation = Quaternion.Lerp(_startQuaternion, _endQuaternion, _cntTime);

        if (_cntTime >= 1)
            ChangeRoot();

    }

    // 消え待ち
    void SelfDelete()
    {
        if (_cntTime >= 1)
            Destroy(_parent.gameObject);
    }

    // 状態の変更
    void ChangeState()
    {
        if(_cntTime >= 1)
        {
            _state = (int)_state + (State)1;
        }

    }

    // 回転経路の変更
    void ChangeRoot()
    {
        _currentRoot++;

        if(_currentRoot < ROOT_MAX)
        {
            _startQuaternion = _parent.transform.rotation;
            _endQuaternion = _rotateRoot[_currentRoot];
        }

    }

    // プレイヤーへのダメージ
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if(col.GetComponent<Player>()._state == Player.State.NONE)
                col.GetComponent<Player>().ReceiveDamage(_attackPower);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.GetComponent<Player>()._state == Player.State.NONE)
                col.GetComponent<Player>().ReceiveDamage(_attackPower);
        }
    }

    // 生成者のゲームオブジェクト情報の設定
    public void SetCreatorObject(GameObject obj)
    {
        _creator = obj;
    }

    // 生成者と座標を同期する
    void Follow()
    {
        if (_creator == null)
            return;

        Vector3 pos = _parent.transform.position;
        pos.x = _creator.transform.position.x;
        _parent.transform.position = pos;
    }

}
