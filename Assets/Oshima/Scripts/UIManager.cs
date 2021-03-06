﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

[SerializeField] Text[] texts 		          = null; //0:score 1:result
[SerializeField] Image[] Icons 	              = null;
[SerializeField] Image[] Frames               = null; //0:friend 1:sub 2:main 3:mainfriend
[SerializeField] Image[] HpBars 	          = null; //0:mainHP 1:HPbar1 2:HPbar2 3:HPbar2 
[SerializeField] Image[] MpBars 	          = null; //0:mainMPbar 1:MPbar1 2:MPbar2 3:MPbar3
[SerializeField] Image[] KiraKira             = null; //0:sub1 1:sub2 2:sub3
//[SerializeField] Sprite[] sprites 	          = null;
List<GameObject[]> pointsArray                = null; //pointsArray[i][j] i:HPbarの位置 j:pointの位置
[SerializeField] PlayerManager _playerManager = null;
[SerializeField] AudioManager _audioManager   = null;

public int[] playerPos                        = new int[4]; //playerのposition
public int friend                             = 0; //0:使用可能 1:使用中
public bool friendSkil                        = true; //スキルが使えるかどうか

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
    Frames[2].gameObject.SetActive(true);
    Frames[3].gameObject.SetActive(false);
   
}

//HPBarの操作
public void EditHpGauge(float nowHp, int gaugeNum){
    for(int i = 4; i>nowHp && i > 0; i--){
        pointsArray[gaugeNum][i-1].SetActive(false);
    }
    for(int j = 0; j<nowHp; j++){
        pointsArray[gaugeNum][j].SetActive(true);
    }
}

public void EditMpGauge(float nowMp, int playerPos){
    MpBars[playerPos].fillAmount = nowMp;
    RectTransform _recttransform = MpBars[playerPos].gameObject.GetComponent <RectTransform>();
    if(nowMp == 1){
        if(_recttransform.localScale.x < 1.2f){
            _recttransform.localScale += new Vector3 (0.03f, 0.03f, 0.03f);
            if(playerPos != 0){
                RectTransform e_recttransform = KiraKira[playerPos-1].gameObject.GetComponent <RectTransform>();
                KiraKira[playerPos-1].color = new Color(1f,1f,1f,1f);
                KiraKira[playerPos-1].gameObject.SetActive(true);
                e_recttransform.localScale += new Vector3(0.03f, 0.03f, 0.03f);
                e_recttransform.Rotate (0, 0, 2f);
            }
        }else{
            if(playerPos != 0){
                RectTransform e_recttransform = KiraKira[playerPos-1].gameObject.GetComponent <RectTransform>();
                if(KiraKira[playerPos-1].color.a > 0){
                    e_recttransform.localScale += new Vector3(0.03f, 0.03f, 0.03f);
                    e_recttransform.Rotate (0, 0, 2f);
                    KiraKira[playerPos-1].color -= new Color(0,0,0,0.03f);
                }else{
                    e_recttransform.localScale = new Vector3(1f,1f,1f);
                    KiraKira[playerPos-1].gameObject.SetActive(false);
                }
            }
        }
    }else{
        _recttransform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
    }
    if(playerPos == 0 && nowMp == 1){
        _recttransform.localScale = new Vector3 (1.2f, 1.2f, 1.2f);
    }else if(playerPos == 0 && nowMp < 1){
        _recttransform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
    }
}

public void EditResultScore(int score){
    texts[1].text = "Score:"+score.ToString();
}

public void EditScore(int score){
	texts[0].text = score.ToString();
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
    RectTransform e_recttransform = KiraKira[num-1].gameObject.GetComponent <RectTransform>();
    e_recttransform.localScale = new Vector3(1f,1f,1f);
    KiraKira[num-1].gameObject.SetActive(false);
    if (_playerManager.isDead(playerPos[num]))  // ※※死んだキャラクターとは交代しない処理
        return;                                 // ※※
    _audioManager.OnCharaSwitchPlay();
    if(num == 1 || num == 2){
        _playerManager.ChangeCharacter(playerPos[num]);
        if(friend == 1){
            KiraKira[2].gameObject.SetActive(false);
            EditSprite(num,0);
            EditSprite(3,num);
            ExChangePos(num,0);
            ExChangePos(3,num);
            friend = 0;
            Frames[0].gameObject.SetActive(true);
            Frames[1].gameObject.SetActive(false);
            Frames[2].gameObject.SetActive(true);
            Frames[3].gameObject.SetActive(false);
        }else{
            EditSprite(num,0);
            ExChangePos(num,0);
        }
    }else{
        _playerManager.ChangeCharacter(playerPos[num]);
        if(friend == 0){
            friend = 1;
            Frames[0].gameObject.SetActive(false);
            Frames[1].gameObject.SetActive(true);
            Frames[2].gameObject.SetActive(false);
            Frames[3].gameObject.SetActive(true);
        }else{
            friend = 0;
            Frames[0].gameObject.SetActive(true);
            Frames[1].gameObject.SetActive(false);
            Frames[2].gameObject.SetActive(true);
            Frames[3].gameObject.SetActive(false);
        }
        EditSprite(num,0);
        ExChangePos(num,0);
    }
        //Debug.Log(playerPos[0]+" "+playerPos[1]+" "+playerPos[2]+" "+playerPos[3]);
    }

}
