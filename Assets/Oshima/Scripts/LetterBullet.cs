using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBullet : ObjectHitCheck {



	// Use this for initialization
	void Start () {
		Destroy(this.gameObject,2f);
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.position += new Vector3(0.5f,0,0);
	}

	public override void DoSomeEvent(int patern){
        if(patern == 2){
            Debug.Log("target");
			Destroy(this.gameObject);
        }
		
	}
}
