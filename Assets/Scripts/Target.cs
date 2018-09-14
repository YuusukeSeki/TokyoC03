using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour {

    Score _letter;

    // Use this for initialization
    void Start () {
        _letter = GameObject.Find("Main Camera").GetComponent<Score>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            _letter.AddScore();

            Destroy(gameObject);
        }

    }


}
