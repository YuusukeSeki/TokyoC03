using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jump : Enemy {

    // 移動：〇、弾：×

    // 接地していたらジャンプ

    Rigidbody2D _rb;
    [SerializeField] float _jumpPower;  // ジャンプ力

    // Use this for initialization
    void Start () {
        Init();

    }

    // Update is called once per frame
    void Update () {

    }

    // 初期化処理
    public override void Init()
    {
        base.Init();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpPower * 10);

    }

    // 接地時にジャンプ
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);

        if (col.gameObject.tag == "Ground")
        {
            if(col.transform.position.y < transform.position.y)
            {
                Jump();

            }
        }
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
                _jumpPower *= Skill_TheWorld._magMoveSpeed + Skill_TheWorld._magMoveSpeed * 0.5f;
                _rb.gravityScale *= Skill_TheWorld._magMoveSpeed;

                break;

        }
    }

}
