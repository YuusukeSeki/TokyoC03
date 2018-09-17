using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : ObjectHitCheck {

[SerializeField] GameManager _gameManager = null;

    public override void DoSomeEvent(int patern){
        if(patern == 0){
            Debug.Log("playeraaa");
            _gameManager.hitTarget = true;

        }
        else if(patern == 1){
            Debug.Log("letter");
            _gameManager.ScoreCal();
		    //Destroy(this.gameObject);
        }
       
	}

}
