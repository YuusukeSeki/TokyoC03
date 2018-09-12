using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

[SerializeField] Text[] texts 		= null;
[SerializeField] Image[] images 	= null;//0:HPbar
[SerializeField] Sprite[] sprites 	= null;

public void EditHpGauge(float maxHp, float nowHp){
    float hp = nowHp/maxHp;
    images[0].fillAmount = hp;
}

public void EditMpGauge(){

}

public void EditItemNumber(){

}

public void EditScore(){
	
}

}
