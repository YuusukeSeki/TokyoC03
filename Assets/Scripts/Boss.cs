using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {

    [SerializeField] float _laserStartDelay;        // 初弾発射までの時間
    [SerializeField] float _laserInterval;          // 次弾までの発射間隔
    [SerializeField] bool _isRandomShotPosition;    // 発射口をランダム化する
    [SerializeField] int _numShot_ChangePosition;   // 回数で発射口を切り替える
    [SerializeField] int _numShot_ChangeRest;       // 回数で小休止に入る
    [SerializeField] float _restTime;               // 小休止の時間
    float _cntLaserTime;                            // レーザー発射用の時間計測バッファ
    int _cntShot;                                   // 小休止から復帰してから撃った回数
    int _currentShotPosition;                       // 現在の発射口

    enum State
    {// 状態
        BATTLE, // 戦闘
        REST,   // 小休止
    };

    State _state;   // 現在の状態

    
    [SerializeField] List<GameObject> _part;
    Vector3[] _basePosition = null;

    // １パーツの座標と回転率
    struct MotionKey
    {
        public Vector3 position;
        public Quaternion quaternion;
    };

    // １モーションの１キーのデータ
    struct MotionKey_Frame
    {
        public float time;
        public MotionKey[] key;
    };

    // 1モーションデータ
    struct MotionData
    {
        public MotionKey_Frame[] data; // モーションデータ
        public int keyMax;             // キー数
    };

    // モーションの名前、番号
    public enum MotionName
    {
        NUTORAL,
        MAX,
    }

    MotionData[] _motionList;   // モーションデータ
    MotionName _currentMotion;  // 現在のモーションの名前、番号
    int _key;                   // 現在のキー
    float _cntTime;             // 線形補間のための時間計測用バッファ

    bool _isBlend;              // ブレンド中かどうか
    float _blendTime = 0.5f;    // ブレンド時間

    [SerializeField] List<GameObject> _shotPosition;    // 発射点
    [SerializeField] GameObject _laser;



    // Use this for initialization
    void Start () {
        // ベース座標取得
        _basePosition = new Vector3[_part.Count];
        for (int i = 0; i < _part.Count; ++i)
        {
            _basePosition[i] = _part[i].transform.localPosition;
        }

        // モーションデータ生成
        _motionList = new MotionData[(int)MotionName.MAX];

        // 各モーションのキー数設定
        _motionList[(int)MotionName.NUTORAL].keyMax = 2;

        // メモリ確保
        for (int i = 0; i < (int)MotionName.MAX; ++i)
        {
            _motionList[i].data = new MotionKey_Frame[_motionList[i].keyMax];

            for (int j = 0; j < _motionList[i].keyMax; ++j)
            {
                _motionList[i].data[j].key = new MotionKey[_part.Count];
            }
        }

        // 全キーの座標、回転率の初期化
        for (int i = 0; i < (int)MotionName.MAX; ++i)
        {
            for (int j = 0; j < _motionList[i].keyMax; ++j)
            {
                for (int k = 0; k < _part.Count; ++k)
                {
                    _motionList[i].data[j].key[k].position = Vector3.zero;
                    _motionList[i].data[j].key[k].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
                }
            }
        }

        // モーションデータの作成
        {
            // NUTORAL
            {
                // Key = 0
                {
                    // 時間
                    _motionList[(int)MotionName.NUTORAL].data[0].time = 1;

                    // 座標
                    _motionList[(int)MotionName.NUTORAL].data[1].key[0].position = new Vector3(0, +0.0f, 0);    // ボール
                    _motionList[(int)MotionName.NUTORAL].data[1].key[1].position = new Vector3(0, +0.0f, 0);    // 下半身
                    _motionList[(int)MotionName.NUTORAL].data[1].key[2].position = new Vector3(0, +0.0f, 0);    // 上半身
                    _motionList[(int)MotionName.NUTORAL].data[1].key[3].position = new Vector3(0, +0.0f, 0);    // 頭
                    _motionList[(int)MotionName.NUTORAL].data[1].key[4].position = new Vector3(0, +0.0f, 0);    // 左肩
                    _motionList[(int)MotionName.NUTORAL].data[1].key[5].position = new Vector3(0, +0.0f, 0);    // 左腕
                    _motionList[(int)MotionName.NUTORAL].data[1].key[6].position = new Vector3(0, +0.0f, 0);    // 右肩
                    _motionList[(int)MotionName.NUTORAL].data[1].key[7].position = new Vector3(0, +0.0f, 0);    // 右腕
                    _motionList[(int)MotionName.NUTORAL].data[1].key[8].position = new Vector3(0, +0.0f, 0);    // ケーブル

                    // 回転率
                    _motionList[(int)MotionName.NUTORAL].data[1].key[0].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1)); // ボール
                    _motionList[(int)MotionName.NUTORAL].data[1].key[1].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1)); // 下半身
                    _motionList[(int)MotionName.NUTORAL].data[1].key[2].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1)); // 上半身
                    _motionList[(int)MotionName.NUTORAL].data[1].key[3].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1)); // 頭
                    _motionList[(int)MotionName.NUTORAL].data[1].key[4].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1)); // 左肩
                    _motionList[(int)MotionName.NUTORAL].data[1].key[5].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1)); // 左腕
                    _motionList[(int)MotionName.NUTORAL].data[1].key[6].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1)); // 右肩
                    _motionList[(int)MotionName.NUTORAL].data[1].key[7].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1)); // 右腕
                    _motionList[(int)MotionName.NUTORAL].data[1].key[8].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1)); // ケーブル

                }


                // Key = 1
                {
                    // 時間
                    _motionList[(int)MotionName.NUTORAL].data[1].time = 1;

                    // 座標
                    _motionList[(int)MotionName.NUTORAL].data[1].key[0].position = new Vector3(0, +1.0f, 0);    // ボール
                    _motionList[(int)MotionName.NUTORAL].data[1].key[1].position = new Vector3(0, -0.3f, 0);    // 下半身
                    _motionList[(int)MotionName.NUTORAL].data[1].key[2].position = new Vector3(0, +0.3f, 0);    // 上半身
                    _motionList[(int)MotionName.NUTORAL].data[1].key[3].position = new Vector3(0, +0.0f, 0);    // 頭
                    _motionList[(int)MotionName.NUTORAL].data[1].key[4].position = new Vector3(0, +0.2f, 0);    // 左肩
                    _motionList[(int)MotionName.NUTORAL].data[1].key[5].position = new Vector3(0, +0.0f, 0);    // 左腕
                    _motionList[(int)MotionName.NUTORAL].data[1].key[6].position = new Vector3(0, +0.2f, 0);    // 右肩
                    _motionList[(int)MotionName.NUTORAL].data[1].key[7].position = new Vector3(0, +0.0f, 0);    // 右腕
                    _motionList[(int)MotionName.NUTORAL].data[1].key[8].position = new Vector3(0, +0.7f, 0);    // ケーブル

                    // 回転率
                    _motionList[(int)MotionName.NUTORAL].data[1].key[0].quaternion = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1)); // ボール
                    _motionList[(int)MotionName.NUTORAL].data[1].key[1].quaternion = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1)); // 下半身
                    _motionList[(int)MotionName.NUTORAL].data[1].key[2].quaternion = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1)); // 上半身
                    _motionList[(int)MotionName.NUTORAL].data[1].key[3].quaternion = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1)); // 頭
                    _motionList[(int)MotionName.NUTORAL].data[1].key[4].quaternion = Quaternion.AngleAxis( 15, new Vector3(0, 0, 1)); // 左肩
                    _motionList[(int)MotionName.NUTORAL].data[1].key[5].quaternion = Quaternion.AngleAxis(-30, new Vector3(0, 0, 1)); // 左腕
                    _motionList[(int)MotionName.NUTORAL].data[1].key[6].quaternion = Quaternion.AngleAxis(-15, new Vector3(0, 0, 1)); // 右肩
                    _motionList[(int)MotionName.NUTORAL].data[1].key[7].quaternion = Quaternion.AngleAxis(+30, new Vector3(0, 0, 1)); // 右腕
                    _motionList[(int)MotionName.NUTORAL].data[1].key[8].quaternion = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1)); // ケーブル

                }
            }
        }


        // 初期状態の設定
        Init();

    }

    // Update is called once per frame
    void Update () {
        // 状態の変更
        ChangeState();

        // 時間経過処理
        ElapsedTime();

        // 状態に応じて、行動を変える
        switch (_state)
        {
            case State.BATTLE:
                // 攻撃処理
                Attack();
                break;

            case State.REST:
                // モーション再生
                PlayMotion();
                break;
        }

    }

    // 初期化
    public override void Init()
    {
        base.Init();

        _currentMotion = MotionName.NUTORAL;
        _key = 0;
        _cntTime = 0;

        _cntLaserTime = _laserStartDelay;
        _cntShot = 0;
        _currentShotPosition = 0;
        _state = State.REST;
    }

    // モーション再生
    void PlayMotion()
    {
        // 座標と回転率を線形補間
        for (int i = 0; i < _part.Count; ++i)
        {
            Vector3 startPos = _basePosition[i] + _motionList[(int)_currentMotion].data[_key].key[i].position;
            Vector3 endPos   = _basePosition[i] + _motionList[(int)_currentMotion].data[_key + 1 >= _motionList[(int)_currentMotion].keyMax ? 0 : _key + 1].key[i].position;

            _part[i].transform.localPosition = Vector3.Lerp(startPos, endPos, _cntTime);
            _part[i].transform.localRotation = Quaternion.Lerp( _motionList[(int)_currentMotion].data[_key].key[i].quaternion,
                                                                _motionList[(int)_currentMotion].data[_key + 1 >= _motionList[(int)_currentMotion].keyMax ? 0 : _key + 1].key[i].quaternion, _cntTime);
        }

        // 時間経過
        _cntTime += Time.deltaTime;

        // 規定時間経過で次のキーへ遷移
        if (_cntTime >= _motionList[(int)_currentMotion].data[_key].time)
        {
            _key = _key + 1 >= _motionList[(int)_currentMotion].keyMax ? 0 : _key + 1;
            _cntTime = 0;
        }

    }

    // 状態変更処理
    void ChangeState()
    {
        switch (_state)
        {
            case State.BATTLE:
                if (_cntShot >= _numShot_ChangeRest)
                {
                    _state = State.REST;
                    _cntShot = 0; // コメントを外すと組み合わせによっては、１つの発射口からしかレーザーが出なくなる。コメントアウトでオーバーフローの危険性有り。
                }
                break;

            case State.REST:
                if (_cntLaserTime <= 0)
                {
                    _state = State.BATTLE;
                    _cntLaserTime = _laserStartDelay;
                }
                break;

        }

    }

    // 時間経過処理
    void ElapsedTime()
    {
        if(_cntLaserTime <= 0)
        {
            switch (_state)
            {
                case State.BATTLE:
                    _cntLaserTime = _laserInterval;
                    break;

                case State.REST:
                    _cntLaserTime = _restTime;
                    break;

            }

        }

        _cntLaserTime -= Time.deltaTime;

    }

    // 攻撃処理
    void Attack()
    {
        if(_cntLaserTime <= 0)
        {
            CreateLazer();
            _cntShot++;

            if (_isRandomShotPosition || _cntShot % _numShot_ChangePosition == 0)
                ChangeShotPosition();
        }
    }

    // レーザー生成
    void CreateLazer()
    {
        GameObject obj = Instantiate(_laser, _shotPosition[_currentShotPosition].transform.position, new Quaternion());
        obj.transform.GetChild(0).GetComponent<Laser>().SetCreatorObject(_shotPosition[_currentShotPosition]);
    }

    // 発射口切り替え処理
    void ChangeShotPosition()
    {
        // ランダム時の処理
        if(_isRandomShotPosition)
        {
            _currentShotPosition = (int)(Random.value * 10) % 2;
            return;
        }

        // 回数で変更時の処理
        _currentShotPosition = _currentShotPosition + 1 >= 2 ? 0 : 1;

    }

}
