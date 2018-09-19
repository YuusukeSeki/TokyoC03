using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Jump : Enemy {

    Rigidbody2D _rb;
    //[SerializeField] float _jumpPower;  // ジャンプ力

    // Use this for initialization
    void Start () {
        _rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () {

    }

    // 接地時にジャンプ
    protected override void OnCollisionEnter2D(Collision2D col)
    {
        base.OnCollisionEnter2D(col);

        if (col.gameObject.tag == "Ground")
        {
            if(col.transform.position.y <= transform.position.y)
                _rb.AddForce(Vector2.up * _moveSpeed.y);

        }

    }
    protected override void OnCollisionStay2D(Collision2D col)
    {
        base.OnCollisionStay2D(col);

        if (col.gameObject.tag == "Ground")
        {
            if (col.transform.position.y <= transform.position.y)
                _rb.AddForce(Vector2.up * _moveSpeed.y);

        }

    }

}
