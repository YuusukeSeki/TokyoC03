using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Invisible : Skill {

    [SerializeField] float _time;
    float _cntTime;

    float _before_invincibleTime;


	// Use this for initialization
	void Start () {
        _type = TYPE.NONE;
        _cntTime = -1;

    }

    // Update is called once per frame
    void Update () {
        if (_cntTime < 0)
            return;

        _cntTime -= Time.deltaTime;

        if(_cntTime < 0)
        {
            gameObject.GetComponent<Player>()._invincibleTime = _before_invincibleTime;
        }

    }

    // スキルを使う
    public override void UseSkill()
    {
        base.UseSkill();

        _cntTime = _time;
        _before_invincibleTime = gameObject.GetComponent<Player>()._invincibleTime;
        gameObject.GetComponent<Player>()._invincibleTime = _time;
        gameObject.GetComponent<Player>().SetInvincible();
        gameObject.tag = "PlayerDamage";
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage"); ;

    }

}
