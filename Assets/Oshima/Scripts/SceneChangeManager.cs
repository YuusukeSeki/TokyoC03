using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour {
[SerializeField] GameObject _fadePanael = null;
public bool changeFlag 					= false;
private int Id 							= 0;
	
	void Update(){
		if(_fadePanael.GetComponent<FadeScript>().GetFadeState() == FadeScript.FadeState.FADE_OUT_COMPRETED) 
		{ 
			SceneManager.LoadScene ("scene" + Id.ToString());
		}
	}

	public void SceneChange(int id){
		Id = id;
		changeFlag = true;
		_fadePanael.GetComponent<FadeScript>().StartFadeOut();
	}
}
