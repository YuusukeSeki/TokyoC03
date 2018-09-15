using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

[SerializeField] Player _player 			= null;
//[SerializeField] Boss _boss					= null;
[SerializeField] UIManager _uiManager 		= null;
[SerializeField] AudioManager _audioManager = null;
[SerializeField] GameObject pousePanel 		= null;
int gameState								= 0;
EventSystem eventSystem 					= null;
string touchLayerName						= "";
Status status 								= Status.PLAYING;
int score									= 0;

float[] playerMPs							= new float[4]; // 仮のMPです


enum Status{
		PLAYING,
		POUSE
	}


	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		if(status == Status.PLAYING){
			Play();
		}
		//Debug.Log(playerMPs[0]+" "+playerMPs[1]+" "+playerMPs[2]+" "+playerMPs[3]);
	}

	private void Play(){
		_uiManager.EditHpGauge(_player.hp, 0);
		MpCal(0, 1f);
		MpCal(1, 2f);
		MpCal(2, 3f);
		MpCal(3, 4f);

		//画面タップ時プレイヤーがジャンプ、アイコンの上ではジャンプしない
		if(Input.GetMouseButtonDown(0)) {
			touchLayerName = "";
　　　		 PointerEventData pointer = new PointerEventData(EventSystem.current);
        	pointer.position = Input.mousePosition;
        	List<RaycastResult> result = new List<RaycastResult>();
        	EventSystem.current.RaycastAll(pointer, result);

        	foreach (RaycastResult raycastResult in result)
        	{
            	touchLayerName = LayerMask.LayerToName(raycastResult.gameObject.layer);
        	}
			if(touchLayerName != "Icon"){
				_player.Jump();
			}
　		}
	}


	public void Init(){
		status = Status.PLAYING;
		eventSystem = EventSystem.current;
		pousePanel.SetActive(false);
		for(int i = 0; i<4; i++){
			playerMPs[i] = 0.5f;
			_uiManager.EditMpGauge(playerMPs[i], i);
		}
	}

	private void MpCal(int playerNum, float speed){
		int playerPos = _uiManager.SearchPlayerPos(playerNum);
		if(playerPos != 0){
			if(playerMPs[playerNum] < 1){
				playerMPs[playerNum] += 0.0005f*speed;
			}else{
				playerMPs[playerNum] = 1;
			}
			_uiManager.EditMpGauge(playerMPs[playerNum], playerPos);
		}else{
			_uiManager.EditMpGauge(playerMPs[playerNum], 0);
		}	
	}

	public void ScoreCal(){
		score += 1;
		_uiManager.EditScore(score);
	}

	public void SceneChange(int id){
		//SceneManager.LoadScene ("scene" + id.ToString());
	}

	public void OnPauseButton(){
		Time.timeScale = 0f;
		status = Status.POUSE;
		pousePanel.SetActive(true);
	}
	
	public void OnRestartButton(){
		Time.timeScale = 1f;
		status = Status.PLAYING;
		pousePanel.SetActive(false);
	}

	public void OnSkilButton(){
		int playerNum = _uiManager.playerPos[0];
		if(playerMPs[playerNum] >= 1){
			playerMPs[playerNum] = 0;
		}
	}
}
