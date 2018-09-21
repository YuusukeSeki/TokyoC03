using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

[SerializeField] AudioClip[] audioClips 	 = null; //0:BGM 1:caharaswitch 2:damage 3:enemyshot 4:jump 5:letter 6:retire 7:slide 8:start 9:stop
AudioSource audioSource 				 	 = null;

	// Use this for initialization
	void Start () {
		audioSource = this.gameObject.GetComponent<AudioSource>();
		audioSource.clip = audioClips[0];
		audioSource.Play();//BGM1
	}

	public void OnCharaSwitchPlay(){
		audioSource.PlayOneShot(audioClips[1]);
	}

	public void OnDamagePlay(){
		audioSource.PlayOneShot(audioClips[2]);
	}

	public void OnEnemyShotPlay(){
		audioSource.PlayOneShot(audioClips[3]);
	}

	public void OnJumpPlay(){
		audioSource.PlayOneShot(audioClips[4]);
	}

	public void OnLetterPlay(){
		audioSource.PlayOneShot(audioClips[5]);
	}

	public void OnRetirePlay(){
		audioSource.PlayOneShot(audioClips[6]);
	}

	public void OnSlidePlay(){
		audioSource.PlayOneShot(audioClips[7]);
	}

	public void OnStartPlay(){
		audioSource.PlayOneShot(audioClips[8]);
	}

	public void OnStopPlay(){
		audioSource.PlayOneShot(audioClips[9]);
	}
}
