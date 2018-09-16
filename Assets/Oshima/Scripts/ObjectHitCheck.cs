using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHitCheck : MonoBehaviour {

/* 
public void OnCollisionEnter2D(Collision2D collision){
	// レイヤー名を取得
    string layerName = LayerMask.LayerToName(collision.gameObject.layer);
	if (layerName == "Player"){
		Debug.Log("collision");
		DoSomeEvent();
	}
}
*/

public void OnTriggerEnter2D(Collider2D collider){
	// レイヤー名を取得
    string layerName = LayerMask.LayerToName(collider.gameObject.layer);
	if (layerName == "Player"){
		//Debug.Log("trigger");
		DoSomeEvent();
	}
}

public virtual void DoSomeEvent(){
	Debug.Log("collision");
}

}
