using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_B : Enemy {

	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {

        Move();

	}

    public override void Init()
    {
        base.Init();
    }

    void Move()
    {
        Vector3 pos = transform.position;
        pos.x += _moveSpeed.x * Time.deltaTime * -1;
        transform.position = pos;

    }
    

}
