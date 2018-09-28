using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_C03 : Enemy
{

    [SerializeField] Vector2 _amplitude;    // 振幅
    Vector3 _basePosition;                  // 基点
    Vector2 _angle;
    [SerializeField] Vector2 _moveSpeed;  // 移動速度
    float _scaleX;

    [SerializeField] List<GameObject> _obj;

    struct MotionKey
    {
        public Vector3 position;
        public Quaternion quaternion;
    };

    struct MotionKey_Frame
    {
        public float time;
        public MotionKey[] key;
    };

    MotionKey_Frame[] _motion;
    float _cntTime = 0;
    int _key = 0;

    int KEY_MAX = 2;



    // Use this for initialization
    void Start()
    {
        // 基点の設定
        _basePosition = transform.position;

        _scaleX = transform.lossyScale.x;

        // モーションデータ読み込み
        _motion = new MotionKey_Frame[KEY_MAX];
        _motion[0].key = new MotionKey[_obj.Count];
        _motion[1].key = new MotionKey[_obj.Count];

        _motion[0].time = 1;
        _motion[1].time = 1;

        for (int i = 0; i < KEY_MAX; i++)
        {
            for (int j = 0; j < _obj.Count; ++j)
            {
                _motion[i].key[j].position = _obj[j].transform.position;
                _motion[i].key[j].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
            }
        }

        _motion[1].key[0].position = new Vector3(0, 1, 0);
        _motion[1].key[1].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[2].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[3].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[4].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[5].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[6].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[7].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[8].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[9].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[10].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[11].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[12].position = new Vector3(0, 0.8f, 0);

        _motion[1].key[0].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        _motion[1].key[1].quaternion = Quaternion.AngleAxis(-20, new Vector3(0, 0, 1));
        _motion[1].key[2].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        _motion[1].key[3].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        _motion[1].key[4].quaternion = Quaternion.AngleAxis(-30, new Vector3(0, 0, 1));
        _motion[1].key[5].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        _motion[1].key[6].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        _motion[1].key[7].quaternion = Quaternion.AngleAxis(+20, new Vector3(0, 0, 1));
        _motion[1].key[8].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        _motion[1].key[9].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        _motion[1].key[10].quaternion = Quaternion.AngleAxis(+30, new Vector3(0, 0, 1));
        _motion[1].key[11].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
        _motion[1].key[12].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));


        // 初期化処理
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        DebufUpdate();

        // 両軸にサインカーブをかける
        _angle += _moveSpeed * Time.deltaTime;
        Vector2 posSin;
        posSin.x = Mathf.Sin(_angle.x) * _amplitude.x;
        posSin.y = Mathf.Sin(_angle.y) * _amplitude.y;

        // 基点に振動を足した座標を求める
        Vector3 pos = _basePosition;
        pos.x += posSin.x;
        pos.y += posSin.y;

        // 進行方向に応じて向きを変える
        Vector3 scale = transform.localScale;
        if (pos.x < transform.position.x)
        {
            scale.x = _scaleX;
        }
        else
        {
            scale.x = -_scaleX;
        }

        // Objectに適用
        transform.position = pos;
        transform.localScale = scale;


        // モーション再生
        for (int i = 0; i < _obj.Count; ++i)
        {
            _obj[i].transform.localRotation = Quaternion.Lerp(_motion[_key].key[i].quaternion, _motion[_key + 1 >= KEY_MAX ? 0 : _key + 1].key[i].quaternion, _cntTime);
        }

        _cntTime += Time.deltaTime;

        if (_cntTime >= _motion[_key].time)
        {
            _key = _key + 1 >= KEY_MAX ? 0 : _key + 1;
            _cntTime = 0;
        }


    }

    // 初期化処理
    public override void Init()
    {
        base.Init();

        _angle = new Vector2(0, 0);
    }

    // プレイヤーのスキル効果を受ける
    // type : スキルの種類
    public override void ReceiveSkill(Skill.TYPE type)
    {
        base.ReceiveSkill(type);

        switch (type)
        {
            case Skill.TYPE.THE_WORLD:
                // 移動速度の変更
                _moveSpeed *= Skill_TheWorld._magMoveSpeed;
                break;
        }
    }

    // 手紙に当たった時の効果
    public override void ReceiveLettterBullet()
    {
        base.ReceiveLettterBullet();

        //switch (_debuf)
        //{
        //    case Debuf.SPEED_UP:
        //        GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f);
        //        break;
        //}
    }


    public override void OnBecameInvisible()
    {
        enabled = true;
    }


}
