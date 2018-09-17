using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TheWorld : Skill
{

	// Use this for initialization
	void Start () {
		
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
        // foreachは配列の要素の数だけループします。
        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<Enemy>()._moveSpeed *= 0.5f;
            enemy.GetComponent<Enemy>()._speed_Bullet *= 0.5f;

        }
    }

}
