using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    public AudioManager _audioManager;

    // スキルの種類　※スキル効果を、他のオブジェクトに与えるときに使用
    public enum TYPE
    {
        NONE,
        THE_WORLD,
    };

    protected TYPE _type; // スキルの種類

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected virtual void Init()
    {

    }

    // スキルを使う
    public virtual void UseSkill()
    {

    }

}
