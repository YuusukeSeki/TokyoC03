using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

[SerializeField] Text[] texts 		= null;
[SerializeField] Image[] Icons 	    = null;
[SerializeField] Image[] Bars 	    = null;
[SerializeField] Sprite[] sprites 	= null;
//Image[] _spriteRenderer    = null;

void Start(){
    for(int i = 0; i<Icons.Length;i++){
        //_spriteRenderer[i] = Icons[i].gameObject.GetComponent<SpriteRenderer>();
    }
}

public void EditHpGauge(float maxHp, float nowHp){
    float hp = nowHp/maxHp;
    Bars[0].fillAmount = hp;
}

public void EditMpGauge(){

}

public void EditItemNumber(){

}

public void EditScore(){
	
}

public void EditSprite(int num){
    //Sprite touchSprite = _spriteRenderer[num].sprite;
    //Sprite toSprite = _spriteRenderer[0].sprite;
    Sprite touchSprite = Icons[num].sprite;
    Sprite toSprite = Icons[0].sprite;

    Icons[0].sprite = touchSprite;
    Icons[num].sprite = toSprite;

    Debug.Log("ok");
}

public void OnIconClick(int num){
    //Debug.Log("icon"+num);
    if(num == 0){
        Debug.Log("skil");
    }
    else if(num == 3){
        Debug.Log("friend");
    }
    else{
        EditSprite(num);
    }
    
}

}
