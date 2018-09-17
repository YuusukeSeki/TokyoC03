using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBullet : MonoBehaviour {



	// Use this for initialization
	void Start () {
		Destroy(this.gameObject,5f);
	}
	
	// Update is called once per frame
	void Update () {
		this.gameObject.transform.position += new Vector3(0.5f,0,0);
	}
}
