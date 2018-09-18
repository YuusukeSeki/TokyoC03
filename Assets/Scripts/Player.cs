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

    public struct Paramater
    {
        public float _hp, _maxHp;           // 体力、最大体力
        public float _healMP_PerSeconds;    // 秒間MP回復量
        public float _jumpPower;            // ジャンプ力
        public float _runForce;             // 加速度
        public float _runMaxSpeed;          // 最大速度

    };
    
    public Paramater _paramater;

    [SerializeField] float _maxHp, _healMP_PerSeconds, _jumpPower, _runForce, _runMaxSpeed;

    // 接地フラグ
    bool _isGround;

    // 横移動
    float _runSpeed;                     // 現在の速度

    // Component
    Rigidbody2D _rb;
    FlashScript _ff;
    FadeScript _fade;

    // 上移動
    float _jumpThreshold;               // 空中判定の閾値

    // 無敵処理関係
    bool _invincible;
    public float _invincibleTime;  // 時間
    float _cntInvincibleTime;    // 計測カウンター

    // 状態
    public State _state;

    // 初期座標（ゴール時、全滅時にこの地点に戻す）
    Vector3 respownPosition;

    // 交代
    float _cntTime;
    float _changeTime;
    Vector3 _startPos;
    Vector3 _endPos;

    // スキル
    Skill _skill;
    

    // Use this for initialization
    void Awake()
    {
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
        // コンポーネント取得
        _rb = GetComponent<Rigidbody2D>();
        _ff = GetComponent<FlashScript>();
        _skill = GetComponent<Skill>();

        // ジャンプ判定の閾値設定
        _jumpThreshold = 0.00001f;

        // 点滅時間の設定
        _ff.SetFrashTime(_invincibleTime);

        // フェード取得
        _fade = GameObject.Find("FadePanel").GetComponent<FadeScript>();

        // 初期座標取得
        respownPosition = transform.position;

        // Unity 上の数字と同期
        _paramater._maxHp = _maxHp;
        _paramater._jumpPower = _jumpPower;
        _paramater._runForce = _runForce;
        _paramater._runMaxSpeed = _runMaxSpeed;

        // 体力
        _paramater._hp = _paramater._maxHp;

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
            transform.position += new Vector3(-_paramater._runMaxSpeed * 0.5f * Time.deltaTime, 0, 0);
        }
        else
        {// 通常時
            // 左右の移動。一定の速度に達するまでは加速度を足していき、最大速度以降は最大速度に固定する。
            _runSpeed += _paramater._runForce * Time.deltaTime;
            if (_runSpeed > _paramater._runMaxSpeed)
            {
                _runSpeed = _paramater._runMaxSpeed;
            }

            transform.position += new Vector3(_runSpeed * Time.deltaTime, 0, 0);
        }

    }

    // ジャンプ
    public void Jump()
    {
        if (_isGround)
        {
            _rb.AddForce(Vector2.up * _paramater._jumpPower * 10);
        }

    }

    // ダメージを受ける
    // damage : 受けるダメージ量
    public void ReceiveDamage(float damage)
    {
        // ダメージを受けない条件
        if (_invincible)
            return;
        if (_state != State.NONE && _state != State.ENTRY)
            return;

        // HP減少
        _paramater._hp -= damage;

        // 死亡時状態切り替え。キャラ交代はChangeCharaに任せる
        if (_paramater._hp <= 0)
        {
            _state = State.DEAD;

        }
        else
        {
            SetInvincible();

            gameObject.layer = LayerMask.NameToLayer("PlayerDamage"); ;
        }

    }

    // 無敵状態にする
    public void SetInvincible()
    {
        _invincible = true;
        _cntInvincibleTime = 0;
        _ff.StartFlash();

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

            gameObject.layer = LayerMask.NameToLayer("Player"); ;
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

    // スキル使用
    public void Skill()
    {
        _skill.UseSkill();

    }

}
