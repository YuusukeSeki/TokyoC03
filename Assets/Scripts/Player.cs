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

    public enum State
    {
        NONE,
        ENTRY,      // 交代（登場）
        EXIT,       // 交代（退場）
        CLEAR,
        DEAD,
        FALL_DEAD,  // 穴に落ちて死んだ
    };

    // 接地フラグ
    bool _isGround;

    // 横移動
    [SerializeField] float _runForce;    // 加速度
    float _runSpeed;                     // 現在の速度
    public float _runMaxSpeed; // 最大速度

    // Component
    Rigidbody2D _rb;
    BoxCollider2D _col;
    ChangeSprite _cs;
    FlashScript _ff;
    FadeScript _fade;

    // 上移動
    [SerializeField] float _jumpPower;  // ジャンプ力
    float _jumpThreshold;               // 空中判定の閾値

    // 体力
    public float hp, maxHp;

    // 無敵処理関係
    bool _invincible;
    public float _invincibleTime;  // 時間
    float _cntInvincibleTime;    // 計測カウンター

    // 状態
    public State _state;

    // 初期座標（ゴール時、死亡時にこの地点に戻す）
    Vector3 respownPosition;

    // MPが０からMAXになるまでに掛かる秒数
    public float healMP_PerSeconds;

    // 交代
    float _cntTime;
    float _changeTime;
    Vector3 _startPos;
    Vector3 _endPos;


    // Use this for initialization
    void Awake()
    {
        // コンポーネント取得
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<BoxCollider2D>();
        _cs = GetComponent<ChangeSprite>();
        _ff = GetComponent<FlashScript>();

        // ジャンプ判定の閾値設定
        _jumpThreshold = 0.00001f;

        // 点滅時間の設定
        _ff.SetFrashTime(_invincibleTime);

        // フェード取得
        _fade = GameObject.Find("FadePanel").GetComponent<FadeScript>();

        // 初期座標取得
        respownPosition = transform.position;

        // 初期化処理
        Init();

    }

    // Update is called once per frame
    void Update()
    {
        // 状態の変更
        ChangeState();

        // ゴールしてたら動かない
        if (_state == State.CLEAR)
            return;

        // 死んでたら動かない
        if (_state == State.DEAD)
            return;

        // 画面遷移中は動かさない
        if (_fade.GetFadeState() != FadeScript.FadeState.NONE)
            return;

        // 移動処理
        Move();

    }

    // 初期化処理
    public void Init()
    {
        // 体力
        hp = maxHp;

        // 無敵
        _invincible = false;
        _cntInvincibleTime = 0;

        // 状態
        _state = State.NONE;

        // 座標
        transform.position = respownPosition;

        // 速度
        _rb.velocity = new Vector2(0, 0);

        _cntTime = 0;

    }

    // 接地判定
    void OnCollisionEnter2D(Collision2D col)
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

    // 被弾判定,ゴール判定,画面外判定
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet(Enemy)")
        {// 被弾判定
            if (_state != State.NONE)
                return;

            // 通常時以外は攻撃を受けない
            if (_state != State.NONE)
                ReceiveDamage(col.GetComponent<EnemyBullet>()._attackPower);

            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "GoalFlag")
        {// ゴール判定
            _state = State.CLEAR;
        }
        else if (col.gameObject.tag == "DeadLine")
        {// 画面外判定（底）
            if (_state == State.NONE || _state == State.ENTRY)
                return;

            _state = State.FALL_DEAD;
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

        // 無敵状態時の処理
        UpdateInvincible();

    }

    // 移動
    void Move()
    {
        if (_state == State.ENTRY)
        {// 登場中
            _cntTime += Time.deltaTime;

            Vector3 pos = transform.position;

            pos.x = Mathf.MoveTowards(pos.x, _endPos.x, (_endPos.x - _startPos.x) * (Time.deltaTime / _changeTime));
            transform.position = pos;

            if (_cntTime >= _changeTime)
            {
                _state = State.NONE;
                _cntTime = 0;
            }

        }
        else if (_state == State.EXIT)
        {// 退場中
            transform.position += new Vector3(-_runMaxSpeed * 0.5f * Time.deltaTime, 0, 0);
        }
        else
        {// 通常時
            // 左右の移動。一定の速度に達するまでは加速度を足していき、最大速度以降は最大速度に固定する。
            _runSpeed += _runForce * Time.deltaTime;
            if (_runSpeed > _runMaxSpeed)
            {
                _runSpeed = _runMaxSpeed;
            }

            transform.position += new Vector3(_runSpeed * Time.deltaTime, 0, 0);
        }

    }

    // ジャンプ
    public void Jump()
    {
        if (_isGround)
        {
            _rb.AddForce(Vector2.up * _jumpPower * 10);
        }

    }

    // スライディング
    public void Sliding()
    {
        if (_isGround)
        {
            _cs.Change(true);
        }

    }

    // ダメージを受ける
    // damage : 受けるダメージ量
    public void ReceiveDamage(float damage)
    {
        if (_invincible)
            return;

        hp -= damage;

        // 死亡時状態切り替え。キャラ交代はChangeCharaに任せる
        if (hp <= 0)
        {
            _state = State.DEAD;

        }
        else
        {
            SetInvincible();
            _ff.StartFlash();
        }

    }

    // コリジョンの修正
    public void ResizeCollider2D(bool sliding, Vector2 objectSize)
    {
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

    // 無敵状態にする
    void SetInvincible()
    {
        _invincible = true;
        _cntInvincibleTime = 0;

    }

    // 無敵状態時のみ行われる処理
    void UpdateInvincible()
    {
        if (!_invincible)
            return;

        _cntInvincibleTime += Time.deltaTime;

        if(_cntInvincibleTime > _invincibleTime)
        {
            _invincible = false;
            _cntInvincibleTime = 0;
            _ff.EndFlash();
        }

    }

    // 登場時の処理
    public void SetEntry(Vector3 startPos, Vector3 endPos, float changeTime)
    {
        // 状態の変更
        _state = State.ENTRY;

        // 初期座標を設定
        transform.position = startPos;

        // 始点、終点、交代にかかる時間を取得
        _startPos = startPos;
        _endPos = endPos;
        _changeTime = changeTime;

    }

    // 退場時の処理
    public void SetExit()
    {
        _state = State.EXIT;

    }

    // 画面外判定
    void OnBecameInvisible()
    {
        if (_state == State.EXIT)
        {
            _state = State.NONE;
            gameObject.SetActive(false);

        }
    }

    public void Skill()
    {
        Debug.Log("Skill()が呼ばれました");
    }

}
