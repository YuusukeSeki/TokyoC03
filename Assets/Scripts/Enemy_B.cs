using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_B : Enemy {

    // 移動：〇、弾：×

    // クリボー的な
    // 横移動のみ

    [SerializeField] float _moveSpeed;

	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        DebufUpdate();

        Move();

	}

    // 初期化処理
    public override void Init()
    {
        base.Init();
    }

    // 移動処理
    void Move()
    {
        Vector3 pos = transform.position;

        // デバフ状態によっては速度を上げる
        if (_debuf == Debuf.SPEED_UP && _cntDebufTime > 0)
        {
            pos.x += _moveSpeed * _debufRate * -1 * Time.deltaTime;
        }
        else
        {
            pos.x += _moveSpeed * -1 * Time.deltaTime;

        }
        transform.position = pos;

    }

    // プレイヤーのスキル効果を受ける
    // type : スキルの種類
    public override void ReceiveSkill(Skill.TYPE type)
    {
        base.ReceiveSkill(type);

        switch (type)
        {
            case Skill.TYPE.THE_WORLD:
                // 移動速度変更
                _moveSpeed *= Skill_TheWorld._magMoveSpeed;

                break;

        }
    }

    // 手紙に当たった時の効果
    public override void ReceiveLettterBullet()
    {
        base.ReceiveLettterBullet();

        switch (_debuf)
        {
            case Debuf.SPEED_UP:
                GetComponent<SpriteRenderer>().color = new Color(1, 0.5f, 0.5f);
                break;
        }
    }


}
