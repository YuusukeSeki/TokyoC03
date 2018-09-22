using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Barrier : Skill
{
    [SerializeField] GameObject _barrierPrefab;
    Barrier _barrier;

	// Use this for initialization
	void Start () {
        _type = TYPE.NONE;

	}
	
	// Update is called once per frame
	void Update () {

	}

    // スキルを使う
    public override void UseSkill()
    {
        base.UseSkill();

        // バリアを生成する
        GameObject obj = Instantiate(_barrierPrefab, transform.position, Quaternion.identity);
        _barrier = obj.GetComponent<Barrier>();

        // バリアの追従対象を設定する
        _barrier.SetBarrier(gameObject);

    }

}
