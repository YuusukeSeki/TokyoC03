using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

	[SerializeField] GameObject text = null;

	public void StartAnim(){
		text.SetActive(true);
	}
}
