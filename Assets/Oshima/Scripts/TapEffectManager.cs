using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapEffectManager : MonoBehaviour {

[SerializeField] GameObject tapEffect = null;
	// Update is called once per frame
	void Update () {
		if(Input.touchCount > 0){
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Began){
				Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				GameObject Effect = Instantiate(tapEffect,new Vector3(tapPoint.x,tapPoint.y,0),Quaternion.identity);
				Debug.Log("tap");
				Debug.Log(new Vector3(tapPoint.x,tapPoint.y,0));
			}
		}
	}
}
