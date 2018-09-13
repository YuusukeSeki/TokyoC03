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
		_uiManager.EditHpGauge(_player.maxHp, _player.hp);

		if(Input.GetMouseButtonDown(0)) {
　　　		Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
　　　		Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
　　　		if (collition2d) {
			// レイヤー名を取得
    		string layerName = LayerMask.LayerToName(collition2d.transform.gameObject.layer);
　　　　　		Debug.Log(layerName);
　　　		}
　		}
	}

	public void SceneChange(int id){
		//SceneManager.LoadScene ("scene" + id.ToString());
	}
}
