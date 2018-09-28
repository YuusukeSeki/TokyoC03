using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFlag : MonoBehaviour {

    [SerializeField] PlayerManager _playerManager;
    [SerializeField] GameObject _effect;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_playerManager._state == PlayerManager.State.CLEAR)
        {
            _effect.gameObject.SetActive(true);
        }
        else
        {
            _effect.gameObject.SetActive(false);
        }

    }
}
