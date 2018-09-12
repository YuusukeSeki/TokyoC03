using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

[SerializeField] Player _player 			= null;
//[SerializeField] Boss _boss					= null;
[SerializeField] UIManager _uiManager 		= null;
[SerializeField] AudioManager _audioManager = null;
int gameState								= 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//_uiManager.EditHpGauge(_player.maxHP, _player.hp);
	}

	public void SceneChange(int id){
		//SceneManager.LoadScene ("scene" + id.ToString());
	}
}
