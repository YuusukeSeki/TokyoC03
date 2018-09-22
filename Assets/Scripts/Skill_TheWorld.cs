using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TheWorld : Skill
{
    public static float _magMoveSpeed;      // 移動倍率
    public static float _magBulletSpeed;    // 弾速倍率
    public static float _magNextBullet;     // 射撃間隔倍率

    // Use this for initialization
    void Start () {
        _type = TYPE.THE_WORLD;
        _magMoveSpeed   = 0.5f;
        _magBulletSpeed = 0.5f;
        _magNextBullet  = 2.0f;
    }

    // Update is called once per frame
    void Update () {
		
	}

    // スキルを使う
    public override void UseSkill()
    {
        base.UseSkill();

        // GameObject型の配列cubesに、"box"タグのついたオブジェクトをすべて格納
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

        // GameObject型の変数cubeに、cubesの中身を順番に取り出す。
        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<Enemy>().ReceiveSkill(_type);

        }
    }

}
