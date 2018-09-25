using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBullet : ObjectHitCheck {


public Transform target 	= null;
public int sceneState = 0;



	// Use this for initialization
	void Start () {
		Destroy(this.gameObject,2f);
	}
	
	// Update is called once per frame
	void Update () {
		if(sceneState == 0){
			this.gameObject.transform.position += new Vector3(0.5f,0,0);
		}else if(sceneState == 1){
			Vector3 diff = (target.position - this.transform.position);
        	diff.Normalize();
			this.transform.position += diff * 10f * Time.deltaTime;
		}
	}

	public override void DoSomeEvent(int patern){
        if(patern == 2){
            Debug.Log("target");
			Destroy(this.gameObject);
        }else if(patern == 3){
			Debug.Log("ground");
			Destroy(this.gameObject);
		}else if(patern == 4){
			Debug.Log("enemy");
			Destroy(this.gameObject);
		}
	}

}
