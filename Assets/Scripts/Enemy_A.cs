using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_A : Enemy {

    [SerializeField] List<GameObject> _shotPos;
    [SerializeField] GameObject _enemyBullet;

    [SerializeField] float _shot_PerSeconds;    // 秒間発射数
    float _cntTime;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        _cntTime += Time.deltaTime;

        if(_cntTime >= _shot_PerSeconds)
        {
            ShotBullet();

            _cntTime = 0;

        }

        Quaternion _rotate = transform.rotation;
        _rotate.z = _rotate.z + 0.3f * Time.deltaTime >= 1 ? -1 : _rotate.z + 0.3f * Time.deltaTime;
        transform.rotation = _rotate;

    }

    public override void Init()
    {
        base.Init();

        _cntTime = 0;

    }

    void ShotBullet()
    {
        for (int i = 0; i < _shotPos.Count; i++)
        {
            // 生成（座標と回転付き）
            GameObject bullet = Instantiate(_enemyBullet, _shotPos[i].transform.position, transform.rotation);

            // 飛ぶ方向算出
            Vector3 vec = _shotPos[i].transform.position - transform.position;
            vec.Normalize();

            // 設定
            bullet.GetComponent<EnemyBullet>().SetBullet(vec, _bulletSpeed, _attackPower); ;

        }


    }

}
