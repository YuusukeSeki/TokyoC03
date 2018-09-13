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
int gameState								= 0;
EventSystem eventSystem 					= null;
string touchLayerName						= "";


	// Use this for initialization
	void Start () {
		eventSystem = EventSystem.current;
	}
	
	// Update is called once per frame
	void Update () {
		_uiManager.EditHpGauge(_player.maxHp, _player.hp);

		if(Input.GetMouseButtonDown(0)) {
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

	public void SceneChange(int id){
		//SceneManager.LoadScene ("scene" + id.ToString());
	}

}
