﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prick : ObjectHitCheck {

[SerializeField] Player _player = null;

public override void DoSomeEvent(int num){
	Debug.Log("prick");
}
}
