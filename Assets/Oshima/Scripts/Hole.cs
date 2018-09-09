using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : Obstacle {

[SerializeField] GameManager _gameManager = null;

public override void DoSomeEvent(){
	Debug.Log("hole");
}

}
