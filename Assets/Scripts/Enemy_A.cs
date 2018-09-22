using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_A : Enemy {

    // 移動：×、弾：〇

    // 弾を4方向に撃つ

    [SerializeField] List<GameObject> _shotPos; // 発射点
    [SerializeField] float _shot_PerSeconds;    // 秒間発射数
    float _cntTime;     // 前回発射してからの時間
    [SerializeField] float _bulletSpeed; // 弾速

    [SerializeField] GameObject _enemyBullet;   // 生成する弾
    [SerializeField] float _rotateSpeed;    // 回転速度
    float _angle = 0;


	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {

        _cntTime += Time.deltaTime;

        if(_cntTime >= _shot_PerSeconds)
        {
            ShotBullet();

            _cntTime = 0;

        }

        Quaternion rotate = Quaternion.AngleAxis(_angle, new Vector3(0, 0, 1));
        _angle += _rotateSpeed * Time.deltaTime;
        transform.rotation = rotate;

    }

    // 初期化処理
    public override void Init()
    {
        base.Init();

        _cntTime = 0;

    }

    // 弾生成、設定処理
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

            _audioManager.OnEnemyShotPlay();
        }
    }

    // プレイヤーのスキル効果を受ける
    // type : スキルの種類
    public override void ReceiveSkill(Skill.TYPE type)
    {
        base.ReceiveSkill(type);

        switch(type)
        {
            case Skill.TYPE.THE_WORLD:
                // 弾速と射撃間隔と回転速度の変更
                _bulletSpeed *= Skill_TheWorld._magBulletSpeed;
                _shot_PerSeconds *= Skill_TheWorld._magNextBullet;
                _rotateSpeed *= Skill_TheWorld._magMoveSpeed;

                break;
        }
    }


}
