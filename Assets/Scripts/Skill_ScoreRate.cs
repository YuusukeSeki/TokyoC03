using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_ScoreRate : Skill {

    [SerializeField] float _time;   // 効果時間
    float _cntTime;                 // 時間計測

    [SerializeField] GameManager _gameManager;

    [SerializeField] List<GameObject> _effect;

    // Use this for initialization
    void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UseSkill();
        }

        if (_cntTime <= 0)
            return;

        _cntTime -= Time.deltaTime;

        if (_cntTime <= 0)
        {
            EndSkill();
        }

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

        _effect[0].GetComponent<Skef_Score_LI>().StartFlash(_time);
        for (int i = 0; i < _effect.Count; i++)
        {
            _effect[i].SetActive(true);
        }

    }

    public void EndSkill()
    {
        _gameManager._scoreRate = 1;
        _effect[0].GetComponent<Skef_Score_LI>().EndFlash();

        for (int i = 0; i < _effect.Count; i++)
        {
            _effect[i].SetActive(false);
        }
    }


}
