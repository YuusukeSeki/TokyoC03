using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : ObjectHitCheck {

[SerializeField] GameManager _gameManager = null;
[SerializeField] Sprite afterImg          = null;
bool receiveLetter                        = false;
SpriteRenderer _spriteRenderer         = null;


    void Start(){
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public override void DoSomeEvent(int patern){
        
        if(receiveLetter == false){
            if(patern == 0){
            //Debug.Log("playeraaa");
            _gameManager.hitTarget = true;

            }
            else if(patern == 1){
                //Debug.Log("letter");
                _gameManager.ScoreCal();
                _spriteRenderer.sprite = afterImg;
		        //Destroy(this.gameObject);
            }
        }
        receiveLetter = true;
       
	}

}
