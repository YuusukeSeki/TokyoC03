using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour {

    public bool _isSearch;
    public Vector3 _targetPos;

	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {
        _isSearch = false;
        _targetPos = new Vector3(0, 0, 0);
    }

    // プレイヤー感知
    void OnTriggerEnter2D(Collider2D col)
    {
        if (_isSearch)
            return;

        if (col.gameObject.tag == "Player")
        {
            _isSearch = true;
            _targetPos = col.transform.position;
        }

    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (_isSearch)
            return;

        if (col.gameObject.tag == "Player")
        {
            _isSearch = true;
            _targetPos = col.transform.position;
        }

    }


}
