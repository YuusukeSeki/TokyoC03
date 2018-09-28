using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ScoreRate : Skill {

    [SerializeField] float _time;   // 効果時間
    float _cntTime;                 // 時間計測

    [SerializeField] GameManager _gameManager;

    // Use this for initialization
    void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        if (_cntTime <= 0)
            return;

        _cntTime -= Time.deltaTime;

        if (_cntTime <= 0)
            _gameManager._scoreRate = 1;

    }

    protected override void Init()
    {
        base.Init();

        _type = TYPE.NONE;
        _cntTime = 0;

    }

    // スキルを使う
    public override void UseSkill()
    {
        base.UseSkill();

        _gameManager._scoreRate = 2;
        _cntTime = _time;

    }


}
