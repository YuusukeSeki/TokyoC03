using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float _attackPower;
    protected SpriteRenderer _sr;

    public AudioManager _audioManager;

    [SerializeField] protected PlayerManager _playerManager;

    //public enum Debuf
    //{
    //    NONE,
    //    SPEED_UP,
    //    NG_LETTERBULLET,
    //    NG_JUMP,
    //    DAMAGE_HP,
    //};

    //[SerializeField] protected Debuf _debuf;
    [SerializeField] protected float _debufTime;
    //[SerializeField] protected float _debufRate;
    protected float _cntDebufTime;


    // Use this for initialization
    void Start () {
        // 初期化処理
        Init();

    }

    // Update is called once per frame
    void Update () {

    }

    // 初期化処理
    public virtual void Init()
    {
        _sr = GetComponent<SpriteRenderer>();
        enabled = false;

    }

    // プレイヤー、手紙との接触処理
    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(_attackPower);
        }
        else if(col.gameObject.tag == "LetterBullet")
        {
            ReceiveLettterBullet();
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
        else if (col.gameObject.tag == "LetterBullet")
        {
            ReceiveLettterBullet();
        }

    }

    protected virtual void OnTriggerStay2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Player>().ReceiveDamage(_attackPower);
        }

    }

    // 画面外処理
    public virtual void OnBecameInvisible()
    {
        //Destroy(gameObject);
        enabled = false;

    }

    // 画面内処理
    public virtual void OnBecameVisible()
    {
        enabled = true;

    }

    // プレイヤーのスキル効果を受ける
    // type : スキルの種類
    public virtual void ReceiveSkill(Skill.TYPE type)
    {

    }

    // 手紙に当たった時の効果
    public virtual void ReceiveLettterBullet()
    {
        _cntDebufTime = _debufTime;

        //Debug.Log("ReceiveBullet");

        //switch (_debuf)
        //{
        //    case Debuf.NG_LETTERBULLET:
                _playerManager.SetDebuf(_debufTime);
        //        break;

        //    case Debuf.NG_JUMP:
        //        _playerManager.SetDebuf(_debuf, _debufTime);
        //        break;

        //    case Debuf.DAMAGE_HP:
        //        _playerManager._charaLists[_playerManager._nowChara].GetComponent<Player>().ReceiveDamage(1);
        //        break;

        //}

    }

    void DebufReset()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1);


    }

    protected void DebufUpdate()
    {
        if(_cntDebufTime <= 0)
            return;

        _cntDebufTime -= Time.deltaTime;

        if (_cntDebufTime <= 0)
            DebufReset();

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

