using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : ObjectHitCheck {

[SerializeField] GameManager _gameManager = null;

    public override void DoSomeEvent(){
        _gameManager.ScoreCal();
		//Debug.Log("target");
        //Destroy(this.gameObject);
	}

    public void SendSelfPos(){
        Debug.Log(this.gameObject.transform.position);
    }

}
