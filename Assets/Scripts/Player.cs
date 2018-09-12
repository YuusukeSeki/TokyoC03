using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* memo
     * ジャンプ時に、意図しないジャンプ力のバラつきがある。
     *   Jump()
     * 　rb.AddForce(Vector2.up * _jumpPower * 10);
     * 　
    */

    public bool _isOutputDebugLog;

    bool _isGround;

    [SerializeField] float _runForce;    // 加速度
    float _runSpeed;                     // 現在の速度
    [SerializeField] float _runMaxSpeed; // 速度切り替え判定のための閾値

    Rigidbody2D _rb;
    BoxCollider2D _col;
    ChangeSprite _cs;

    [SerializeField] float _jumpPower;  // ジャンプ力
    float _jumpThreshold;               // 空中判定の閾値

    public float hp, maxHp;


    // Use this for initialization
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        _cs = GetComponent<ChangeSprite>();

        _runSpeed      = 0;
        _jumpThreshold = 0.00001f;
        hp             = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        // 状態の変更
        ChangeState();

        // 移動処理
        Move();
    }

    // 接地判定
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if(col.gameObject.transform.position.y < transform.position.y)
            {
                if (!_isGround)
                    _isGround = true;
            }
        }

    }
    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if (col.gameObject.transform.position.y < transform.position.y)
            {
                if (!_isGround)
                    _isGround = true;
            }
        }

    }

    // 被弾判定
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet(Enemy)")
        {
            ReceiveDamage(col.GetComponent<EnemyBullet>()._attackPower);

            Destroy(col.gameObject);
        }

    }

    // 状態の変更
    void ChangeState()
    {
        // 空中にいるかどうかの判定。上下の速度(rigidbody.velocity.y)が一定の値を超えている場合、空中とみなす
        if (Mathf.Abs(_rb.velocity.y) > _jumpThreshold)
        {
            _isGround = false;
        }

    }

    // 移動
    void Move()
    {
        //// デバッグ用ジャンプコード
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    Jump();
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    Sliding();
        //}
        //else
        //{
        //    SetStandSprite();
        //}

        // 左右の移動。一定の速度に達するまでは加速度を足していき、最大速度以降は最大速度に固定する。
        _runSpeed += _runForce * Time.deltaTime;
        if (_runSpeed > _runMaxSpeed)
        {
            _runSpeed = _runMaxSpeed;
        }

        transform.position += new Vector3(_runSpeed * Time.deltaTime, 0, 0);

    }

    // ジャンプ
    public void Jump()
    {
        if(_isOutputDebugLog)
            Debug.Log("Player::Jump() が呼ばれました");

        if (_isGround)
        {
            _rb.AddForce(Vector2.up * _jumpPower * 10);
        }

    }

    // スライディング
    public void Sliding()
    {
        if (_isOutputDebugLog)
            Debug.Log("Player::Sliding() が呼ばれました");

        if (_isGround)
        {
            _cs.Change(true);
        }

    }

    // ダメージを受ける
    // damage : 受けるダメージ量
    public void ReceiveDamage(float damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            // キャラクタ交代処理？

        }

    }

    // コリジョンの修正
    public void ResizeCollider2D(bool sliding, Vector2 objectSize)
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        _col.size = objectSize;

        float moveY = objectSize.y * 0.5f;

        if (sliding)
        {
            transform.position += new Vector3(0, -moveY, 0);
        }
        else
        {
            transform.position += new Vector3(0, +moveY, 0);
        }

    }

    // spriteを立ち絵に変更する
    public void SetStandSprite()
    {
        _cs.Change(false);
    }

}

