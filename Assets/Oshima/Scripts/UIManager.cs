using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {

[SerializeField] Text[] texts 		= null; //0:score
[SerializeField] Image[] Icons 	    = null;
[SerializeField] Image[] Frames     = null; //0:friend 1:sub
[SerializeField] Image[] HpBars 	= null; //0:mainHP 1:HPbar1 2:HPbar2 3:HPbar2 
[SerializeField] Image[] MpBars 	= null; //0:mainMPbar 1:MPbar1 2:MPbar2 3:MPbar3
[SerializeField] Sprite[] sprites 	= null;
List<GameObject[]> pointsArray      = null; //pointsArray[i][j] i:HPbarの位置 j:pointの位置

public int[] playerPos              = new int[4]; //playerのposition
public int friend                   = 0; //0:使用可能 1:使用中
public bool friendSkil              = true; //スキルが使えるかどうか

void Start(){
    pointsArray = new List<GameObject[]>(); 
    for(int i = 0; i<4; i++){
        playerPos[i] = i;
        GameObject[] points = new GameObject[HpBars[i].gameObject.transform.childCount];
        for(int j = 0; HpBars[i].gameObject.transform.childCount > j; j++)
        {
            points[j] = HpBars[i].gameObject.transform.GetChild(j).gameObject;
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

public void EditMpGauge(float nowMp, int playerPos){
    MpBars[playerPos].fillAmount = nowMp;
}

public void EditItemNumber(){

}

public void EditScore(int score){
	texts[0].text = "× " + score.ToString();
}

//Spriteの交換
private void EditSprite(int fromNum, int toNum){
    Sprite touchSprite = Icons[fromNum].sprite;
    Sprite toSprite = Icons[toNum].sprite;

    Icons[toNum].sprite = touchSprite;
    Icons[fromNum].sprite = toSprite;
}

private void ExChangePos(int fromNum, int toNum){
    int fromPos = playerPos[fromNum];
    int toPos = playerPos[toNum];
    playerPos[fromNum] = toPos;
    playerPos[toNum] = fromPos;
}

public int SearchPlayerPos(int playerNum){
    int pos = 0;
    for(int i = 0; i<4; i++){
        if(playerPos[i] == playerNum){
            pos = i;
        }
    }
    return pos;
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
            ExChangePos(num,0);
            ExChangePos(3,num);
            friend = 0;
            Frames[0].gameObject.SetActive(true);
            Frames[1].gameObject.SetActive(false);
        }else{
            EditSprite(num,0);
            ExChangePos(num,0);
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
        ExChangePos(num,0);
    }
        //Debug.Log(playerPos[0]+" "+playerPos[1]+" "+playerPos[2]+" "+playerPos[3]);
    }

}
