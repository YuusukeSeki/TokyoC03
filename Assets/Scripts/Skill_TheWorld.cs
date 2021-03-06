﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_TheWorld : Skill
{
    public static float _magMoveSpeed;      // 移動倍率
    public static float _magBulletSpeed;    // 弾速倍率
    public static float _magNextBullet;     // 射撃間隔倍率

    [SerializeField] List<GameObject> _effect;

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

        _effect[0].GetComponent<Skef_TheWorld>().StartEffect();

        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            enemy.GetComponent<Enemy>().ReceiveSkill(_type);
            enemy.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 1.0f);

        }

    }

}
