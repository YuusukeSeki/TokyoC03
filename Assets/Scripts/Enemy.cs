using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float _attackPower;           // 攻撃力

    public Vector3 _moveSpeed;             // 移動速度
    public float _bulletSpeed;          // 弾の移動速度


    //public PlayerManager _pm;
    //public bool _shotBullet;             // true で射撃行動を取る
    //public bool _aimingPlayer;           // true で弾をプレイヤー方向に撃つ
    //public float _attackPower_Bullet;    // 弾による攻撃力

    //public float _numShot_Seconds;       // 秒間発射数
    //float _cntTime;                      // 秒数記憶バッファ

    ////GameObject player;
    //[SerializeField] GameObject bullet;

    // Use this for initialization
    void Start () {
        // 初期化処理
        Init();

    }

    // Update is called once per frame
    void Update () {
        //// 移動処理
        //Move();

        //// 弾を撃つ
        //ShotBullet();

    }

    // 初期化処理
    public virtual void Init()
    {
        enabled = false;

        //// フレームカウンター初期化
        //_cntTime = 0;

        //_pm =  GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        ////player = _pm._charaLists[_pm._nowChara];

        //enabled = false;
    }

    // プレイヤーとの接触処理
    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(_attackPower);
        }

    }
    protected virtual void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(_attackPower);
        }

    }
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(_attackPower);
        }

    }

    protected virtual void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(_attackPower);
        }

    }

    // 画面外判定
    public virtual void OnBecameInvisible()
    {
        enabled = false;

    }

    // 画面内判定
    public virtual void OnBecameVisible()
    {
        enabled = true;

    }



    //// 移動処理
    //void Move()
    //{
    //    //if (_aimingPlayer)
    //    //{// 追尾
    //    //    Vector3 vec = (_pm._charaLists[_pm._nowChara].GetComponent<Player>().transform.position - transform.position) * _moveSpeed;
    //    //    vec.Normalize();

    //    //    transform.position += vec * _moveSpeed * Time.deltaTime;
    //    //}
    //    //else
    //    //{// まっすぐ左に進行
    //    //    transform.position += transform.right * -_moveSpeed * Time.deltaTime;
    //    //}

    //}

    //// 弾を撃つ処理
    //public void ShotBullet()
    //{
    //    // 射撃フラグがfalseで無処理
    //    if (!_shotBullet)
    //        return;

    //    // 発射間隔毎に弾を撃つ
    //    _cntTime += Time.deltaTime * _numShot_Seconds;

    //    if(_cntTime >= 1)
    //    {
    //        GameObject initBullet = Instantiate(bullet, transform.position + new Vector3(-0.6f, 0.0f), transform.rotation);

    //        if (_aimingPlayer)
    //        {// プレイヤーを狙う場合
    //            // バレット生成位置からプレイヤーへの単位ベクトルを算出
    //            Vector3 aimVector = _pm._charaLists[_pm._nowChara].GetComponent<Player>().transform.position - initBullet.transform.position;
    //            aimVector.Normalize();

    //            // 進行方向をバレットに設定する
    //            initBullet.GetComponent<EnemyBullet>()._vector = aimVector;
    //        }

    //        initBullet.GetComponent<EnemyBullet>().SetBullet(_aimingPlayer, _attackPower_Bullet, _speed_Bullet);

    //        _cntTime = 0;
    //    }

    //}

    //// ダメージを受ける処理
    //public void ReceiveDamage(float damage)
    //{
    //    // HPを減らす
    //    _hp -= damage;

    //    // HPが0以下で破棄
    //    if (_hp <= 0)
    //    {
    //        Destroy(gameObject);

    //    }

    //}

}

