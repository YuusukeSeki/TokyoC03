using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHitCheck : MonoBehaviour {

 
public void OnCollisionEnter2D(Collision2D collision){
	// レイヤー名を取得
    //string layerName = LayerMask.LayerToName(collision.gameObject.layer);
	if (collision.gameObject.tag == "Obstacle"){
		Debug.Log("Obstacle");
		DoSomeEvent(3);
	}else if(collision.gameObject.tag == "Enemy"){
		Debug.Log("Enemy");
		DoSomeEvent(4);
	}
}


public void OnTriggerEnter2D(Collider2D collider){
	// レイヤー名を取得
    //string layerName = LayerMask.LayerToName(collider.gameObject.tag);
	if (collider.gameObject.tag == "Player"){
		//Debug.Log("trigger");
		DoSomeEvent(0);
	}
	else if(collider.gameObject.tag == "LetterBullet"){
		DoSomeEvent(1);
	}
	else if(collider.gameObject.tag == "Target"){
		DoSomeEvent(2);
	}
	//else if(collider.gameObject.tag == "Ground"){
		//DoSomeEvent(3);
	//}
}

public virtual void DoSomeEvent(int patern){
	Debug.Log("collision");
}

}
