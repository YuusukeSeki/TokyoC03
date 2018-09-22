using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet02 : Bullet {

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {

        Vector2 speed = _rb.velocity;
        speed *= 0.5f;

	}

    public void SetBullet(Vector2 speed, float attackPower)
    {
        _attackPower = attackPower;
        _rb.velocity = speed;

    }




}
