using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] protected Rigidbody2D _rb;

    public float _attackPower;  // 攻撃力
    protected float _speed;     // 移動速度
    protected Vector3 _vector;  // 進行方向

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // 移動処理
    protected virtual void Move()
    {

    }

    // 地面との接触処理
    protected void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }

    }
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            Destroy(gameObject);
        }

    }

    // 画面外処理
    protected virtual void OnBecameInvisible()
    {
        // オブジェクトの破棄
        Destroy(gameObject);

    }

}

