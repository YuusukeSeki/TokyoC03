using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

[SerializeField] PlayerManager _playerManager 	= null;
//[SerializeField] Boss _boss					= null;
[SerializeField] UIManager _uiManager 			= null;
[SerializeField] AudioManager _audioManager 	= null;
[SerializeField] GameObject pousePanel 			= null;
[SerializeField] GameObject pousePanel2 		= null;
[SerializeField] GameObject resultPanel 		= null;
[SerializeField] GameObject letter				= null;
//int gameState									= 0;
string touchLayerName							= "";
Status status 									= Status.PLAYING;
int score										= 0;
public bool hitTarget							= false;
private bool isJump								= true;

public int sceneState 							= 0;


float[] playerMPs								= new float[4];


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
		if(_playerManager._state == PlayerManager.State.NONE){
			Play();
		}
		
		if(_playerManager._state == PlayerManager.State.CLEAR || _playerManager._state == PlayerManager.State.GAMEOVER){
			pousePanel2.SetActive(true);
			resultPanel.SetActive(true);
		}
		//Debug.Log(playerMPs[0]+" "+playerMPs[1]+" "+playerMPs[2]+" "+playerMPs[3]);
		//Debug.Log("0: hp "+_playerManager.GetCharacterParamater(0)._hp+" pos "+_uiManager.SearchPlayerPos(0));
		//Debug.Log("1: hp "+_playerManager.GetCharacterParamater(1)._hp+" pos "+_uiManager.SearchPlayerPos(1));
		//Debug.Log("2: hp "+_playerManager.GetCharacterParamater(2)._hp+" pos "+_uiManager.SearchPlayerPos(2));
		//Debug.Log("3: hp "+_playerManager.GetCharacterParamater(3)._hp+" pos "+_uiManager.SearchPlayerPos(3));
		//Debug.Log("mp " + _playerManager.GetCharacterParamater(2)._healMP_PerSeconds);
	}

	private void Play(){
		_uiManager.EditHpGauge(_playerManager.GetCharacterParamater(0)._hp, _uiManager.SearchPlayerPos(0));
		_uiManager.EditHpGauge(_playerManager.GetCharacterParamater(1)._hp, _uiManager.SearchPlayerPos(1));
		_uiManager.EditHpGauge(_playerManager.GetCharacterParamater(2)._hp, _uiManager.SearchPlayerPos(2));
		_uiManager.EditHpGauge(_playerManager.GetCharacterParamater(3)._hp, _uiManager.SearchPlayerPos(3));
		MpCal(0, _playerManager.GetCharacterParamater(0)._healMP_PerSeconds);
		MpCal(1, _playerManager.GetCharacterParamater(1)._healMP_PerSeconds);
		MpCal(2, _playerManager.GetCharacterParamater(2)._healMP_PerSeconds);
		MpCal(3, _playerManager.GetCharacterParamater(3)._healMP_PerSeconds);

		if(Input.touchCount > 0){
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Began){
			//アイコン以外をタップ時何をタップしているかの判定
				Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    			Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
    			if (collition2d) {
        			RaycastHit2D hitObject = Physics2D.Raycast(tapPoint,-Vector2.up);
        			if (hitObject) {
						if(hitObject.collider.gameObject.tag == "Target" && sceneState == 1){
							GameObject Letter = Instantiate(letter,_playerManager.GetMainCharacterPosition() + new Vector3(1f,0,0), Quaternion.identity);
							Letter.GetComponent<LetterBullet>().target = hitObject.collider.gameObject.transform;
							Letter.GetComponent<LetterBullet>().sceneState = sceneState;
							isJump = false;
						}
        			}
    			}

			//画面タップ時アイコンをタップしているかどうかの判定
				touchLayerName = "";
　　　		 	PointerEventData pointer = new PointerEventData(EventSystem.current);
        		pointer.position = Input.mousePosition;
        		List<RaycastResult> result = new List<RaycastResult>();
        		EventSystem.current.RaycastAll(pointer, result);

        		foreach (RaycastResult raycastResult in result)
        		{
            		touchLayerName = LayerMask.LayerToName(raycastResult.gameObject.layer);
        		}
				if(touchLayerName == "Icon"){
					isJump = false;
				}

				if(isJump == true){
					_playerManager.Jump();
					_audioManager.OnJumpPlay();
				}
			}else if (touch.phase == TouchPhase.Ended){
				isJump = true;
			}
　		}
	}


	public void Init(){
		status = Status.PLAYING;
		//eventSystem = EventSystem.current;
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
		hitTarget = false;
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
			_playerManager.Skill();
			playerMPs[playerNum] = 0;
		}
	}

	public void OnPostButton(){
        if (_playerManager._debufState == PlayerManager.DebufState.NG_LETTERBULLET)
            return;

		Instantiate(letter,_playerManager.GetMainCharacterPosition() + new Vector3(1f,0,0), Quaternion.identity);
		_audioManager.OnLetterPlay();
	}

	public void OnPostButton2(){
		if(hitTarget == true){
			ScoreCal();
		}
	}
}
