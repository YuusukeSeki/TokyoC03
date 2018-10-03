using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

[SerializeField] AudioClip[] audioClips 	 = null; //0:BGM 1:caharaswitch 2:damage 3:enemyshot 4:jump 5:letter 6:retire 7:slide 8:start 9:stop 10:title 11:stagechoice 12:result 13:tapse 14:boss_bgm 15:clear
AudioSource audioSource 				 	 = null;
[SerializeField] int scene					 = 0; //0:title 1:select 2:play 3:boss

	// Use this for initialization
	void Start () {
		audioSource = this.gameObject.GetComponent<AudioSource>();
		if(scene == 0){
			audioSource.clip = audioClips[10];
		}else if(scene == 1){
			audioSource.clip = audioClips[11];
		}else if(scene == 2){
			audioSource.clip = audioClips[0];
		}else if(scene == 3){
			audioSource.clip = audioClips[14];
		}
		audioSource.Play();
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

	public void OnResultPlay(){
		audioSource.clip = audioClips[12];
		audioSource.Play();
	}

	public void OnTapPlay(){
		audioSource.PlayOneShot(audioClips[13]);
	}

	public void OnClearPlay(){
		audioSource.PlayOneShot(audioClips[15]);
	}
}
