﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet {

	// Use this for initialization
	void Start () {


    }

    // Update is called once per frame
    void Update () {

    }

    // バレットの設定処理
    // aimingPlayer : true でプレイヤーの方向に弾を撃つ
    public void SetBullet(bool aimPlayer, float attackPower, float speed)
    {
        _attackPower = attackPower;
        _speed = speed;

        if (aimPlayer)
        {// プレイヤーの方向に撃つ
            _rb.velocity = _vector * speed;

        }
        else
        {// 左方向に撃つ
            _rb.velocity = transform.right.normalized * -speed;

        }


    }

    public void SetBullet(Vector3 vector, float speed, float attackPower)
    {
        _vector = vector;
        _speed = speed;
        _attackPower = attackPower;

        _rb.velocity = _vector * _speed;

    }

}
