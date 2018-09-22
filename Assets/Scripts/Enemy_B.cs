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
        pos.x += _moveSpeed * -1 * Time.deltaTime;
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

}
