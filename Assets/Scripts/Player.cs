using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //public int sceneId;    // ランニング、シューティングの切り替えスイッチ
    //public bool isArrive;  // true でクリア

    //public float level;             // レベル
    //public float experience;        // 今までの取得経験値
    //public float getExperience;     // 今ステージでの取得経験値
    //public float attackPower;       // 攻撃力
    //public float attackCost;        // 攻撃時に消費するMP

    [SerializeField] float maxHP;             // 最大HP
    [SerializeField] float maxMP;             // 最大MP
    [SerializeField] float hp;                // 現在のHP
    [SerializeField] float mp;                // 現在のMP
    [SerializeField] float runSpeed;          // 移動速度
    [SerializeField] float jumpPower;         // ジャンプ力
    [SerializeField] float healMp_PerSeconds; // フレーム毎のMP自動回復量
    [SerializeField] float invincibleSeconds; // 攻撃を受けた際の無敵時間（フレーム単位）

    float downSpeed;                            // 落下速度
    [SerializeField] float downSpeed_PerFrame;  // フレーム毎の落下加速度
    bool isGround;
    bool isSliding;
    float cntSliding;

    Rigidbody2D rb;             // 物理演算コンポーネント
    Animator animCtrl;          // アニメーションコントロール

    //[SerializeField] GameObject bullet;             // PlayerBullet　プレハブ
    //[SerializeField] AudioManager _audioManager;  // オーディオデバイス


    // Use this for initialization
    void Start()
    {
        // プレイヤーパラメータの初期化
        hp = maxHP;
        mp = maxMP;

        // 物理演算コンポーネントの取得
        rb = GetComponent<Rigidbody2D>();

        // アニメーションの取得
        animCtrl = GetComponent<Animator>();

        // その他変数初期化
        downSpeed = 0;  // 落下速度
        isGround = false;
        isSliding = false;
        cntSliding = 0;

    }

    // Update is called once per frame
    void Update () {
        // 接地判定処理
        if (CheckIsGround())
        {
            if (Input.GetButtonDown("Jump"))
            {
                // ジャンプ処理
                Jump();
            }
            if (Input.GetButtonDown("Fire2"))
            {
                // スライディング処理
                Sliding();
            }

        }

        if (isSliding)
        {
            cntSliding += Time.deltaTime;
            if(cntSliding >= 1)
            {
                isSliding = false;
                cntSliding = 0;
                animCtrl.SetBool("IsSliding", false);

            }

        }

        // 前方向の障害物判定処理
        //CheckForword();

        // 移動処理
        Move();

        // アニメーション変更処理
        // downSpeed < 0 : 下降アニメーション
        // downSpeed > 0 : 上昇アニメーション
        animCtrl.SetFloat("DownSpeed", downSpeed);

	}

    // 初期化処理
    void Init()
    {
    }

    // 移動処理
    void Move()
    {
        Vector2 nowPos = rb.position;
        nowPos += new Vector2(runSpeed, downSpeed) * Time.deltaTime;
        transform.position = new Vector3(nowPos.x, nowPos.y, 0);


    }

    // 弾発射処理
    //void ShotBullet()
    //{
    //    if (Input.GetButtonDown("Fire2"))
    //    {
    //        // 弾をプレイヤーと同じ位置/角度で生成
    //        GameObject initBullet = Instantiate(bullet, transform.position + new Vector3(0.3f, 0.0f), transform.rotation);
    //        initBullet.GetComponent<PlayerBullet>().SetBullet(attackPower, 8);

    //    }
    //}

    //  接地判定処理
    bool CheckIsGround()
    {
        RaycastHit2D hit;

        // 下方向チェック
        if (!isSliding)
            hit = Physics2D.Raycast(transform.position + new Vector3(-0.32f, -0.96f), Vector2.right, 0.64f);
        else
            hit = Physics2D.Raycast(transform.position + new Vector3(-0.97f, -0.32f), Vector2.right, 0.64f);

        if (hit.transform != null)
        {// レイヤー名を取得
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);

            if (layerName == "Ground")
            {// 接地中の処理
                downSpeed = 0;                      // 重力の初期化
                animCtrl.SetBool("IsGround", true); // アニメーションフラグの切り替え

                isGround = true;

                return true;
            }
        }

        // 重力処理（等加速度運動）
        downSpeed -= downSpeed_PerFrame;        // 落下速度をどんどん早くする
        animCtrl.SetBool("IsGround", false);    // アニメーションフラグの切り替え

        isGround = false;

        return false;

    }

    // 前方向の障害物判定処理
    //bool CheckForword()
    //{
    //    RaycastHit2D hit;

    //    // 前方向チェック
    //    hit = Physics2D.Raycast(transform.position + new Vector3(0.34f, 0.26f), Vector2.down, 0.52f);
    //    if (hit.transform != null)
    //    {
    //        // 障害物に当たった
    //        //animCtrl.SetBool("IsIdle", true);

    //        return true;
    //    }
    //    else
    //    {
    //        // 障害物に当たっていない
    //        //animCtrl.SetBool("IsIdle", false);

    //        return false;
    //    }
    //}

    // ジャンプ&スライディング処理
    //void JumpAndSlidingProcessing()
    //{

    //    if (Input.GetButtonUp("Jump") && isGround)
    //    {
    //        Jump();
    //    }
    //    if (Input.GetButtonUp("Fire2") && isGround)
    //    {
    //        Sliding();
    //    }

    //}

    // 当たり判定
    public void OnTriggerEnter2D(Collider2D collision)
    {
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);

        Debug.Log("Trigger");
        Debug.Log(layerName);

        // Bullet(Enemy)との被弾処理
        if (layerName == "Bullet(Enemy)")
        {
            // ダメージ処理
            ReceiveDamage(collision.GetComponent<EnemyBullet>()._attackPower);

            // Enemyの破棄
            Destroy(collision.gameObject);

        }

    }

    // ダメージを受ける
    // damage : 受けるダメージ量
    public void ReceiveDamage(float damage)
    {
        // HPを減らす
        hp -= damage;
        
        // HPが0以下でGameOver
        if(hp <= 0)
        {
            // GameOver処理

        }

    }

    // ジャンプ処理
    public bool Jump()
    {
        if (isGround)
        {
            downSpeed = jumpPower;
            transform.Translate(Vector3.up * 0.1f);

            Debug.Log("Jump");

            return true;
        }
        else
            return false;
    }

    // スライディング処理
    public bool Sliding()
    {
        if (isGround)
        {
            isSliding = true;
            Vector2 pos = rb.position;
            pos += new Vector2(0, -0.32f);
            transform.position = new Vector3(pos.x, pos.y, 0);

            animCtrl.SetBool("IsSliding", true);

            Debug.Log("Sliding");

            return true;
        }
        else
            return false;
    }

    // HP取得
    public float GetHp()
    {
        return hp;
    }


}
