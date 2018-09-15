using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

[SerializeField] Text[] texts 		= null;
[SerializeField] Image[] Icons 	    = null;
[SerializeField] Image[] Frames     = null; //0:friend 1:sub
[SerializeField] Image[] Bars 	    = null; //0:mainHP 1:HPbar1 2:HPbar2 3:HPbar2 4:MPbar
[SerializeField] Sprite[] sprites 	= null;
List<GameObject[]> pointsArray      = null; //pointsArray[i][j] i:HPbarの位置 j:pointの位置

public int friend                   = 0; //0:使用可能 1:使用中
public bool friendSkil              = true; //スキルが使えるかどうか

void Start(){
    pointsArray = new List<GameObject[]>(); 
    for(int i = 0; i<4; i++){
        GameObject[] points = new GameObject[Bars[i].gameObject.transform.childCount];
        for(int j = 0; Bars[i].gameObject.transform.childCount > j; j++)
        {
            points[j] = Bars[i].gameObject.transform.GetChild(j).gameObject;
        }
        pointsArray.Add(points);
    }
    Frames[0].gameObject.SetActive(true);
    Frames[1].gameObject.SetActive(false);
   
}

//HPBarの操作
public void EditHpGauge(float nowHp, int playerNum){
    for(int i = 4; i>nowHp; i--){
        pointsArray[playerNum][i-1].SetActive(false);
    }
}

public void EditMpGauge(float nowMp){
    Bars[4].fillAmount = nowMp;
}

public void EditItemNumber(){

}

public void EditScore(){
	
}

//Spriteの交換
private void EditSprite(int fromNum, int toNum){
    Sprite touchSprite = Icons[fromNum].sprite;
    Sprite toSprite = Icons[toNum].sprite;

    Icons[toNum].sprite = touchSprite;
    Icons[fromNum].sprite = toSprite;
}

//Iconをクリックした時
public void OnIconClick(int num){
    if(num == 0){
        if(friend == 1){
            if(friendSkil == true){
                Debug.Log("friend skil");
            }
        }else{
            Debug.Log("skil");
        }
        
    }
    else if(num == 1 || num == 2){
        if(friend == 1){
            EditSprite(num,0);
            EditSprite(3,num);
            friend = 0;
            Frames[0].gameObject.SetActive(true);
            Frames[1].gameObject.SetActive(false);
        }else{
            EditSprite(num,0);
        }
    }else{
        if(friend == 0){
            friend = 1;
            Frames[0].gameObject.SetActive(false);
            Frames[1].gameObject.SetActive(true);
        }else{
            friend = 0;
            Frames[0].gameObject.SetActive(true);
            Frames[1].gameObject.SetActive(false);
        }
        EditSprite(num,0);
    }
    
}

}
