using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBullet : ObjectHitCheck {


public Transform target 	= null;
Rigidbody2D rd				= null;



	// Use this for initialization
	void Start () {
		rd = GetComponent<Rigidbody2D>();
		Destroy(this.gameObject,2f);
	}
	
	// Update is called once per frame
	void Update () {
		//this.gameObject.transform.position += new Vector3(0.5f,0,0);
		Vector3 diff = (target.position - this.transform.position); //プレイヤーと対照との差分を取得
        diff.Normalize();
		this.transform.position += diff * 10f * Time.deltaTime;
		
	}

	public override void DoSomeEvent(int patern){
        if(patern == 2){
            Debug.Log("target");
			Destroy(this.gameObject);
        }	
	}

}
