using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_D : Enemy {

    // 移動：×、弾：〇

    // 噴水のように、皿から弾を生み出す

    [SerializeField] GameObject _shotPos;
    [SerializeField] GameObject _enemyBullet;

    [SerializeField] Vector2 _maxBulletVector;     // 弾の方向
    [SerializeField] float _bulletSpeed;        // 弾速
    [SerializeField] float _shot_PerSeconds;    // 秒間発射数
    float _cntTime;     // 前回発射してからの時間
    float _halfWidth;   // 発射点のふり幅（X軸）


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

    }

    // 初期化処理
    public override void Init()
    {
        base.Init();

        _cntTime = 0;
        _halfWidth = GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;

    }

    void ShotBullet()
    {
        // 出現位置を補正値を求める
        Vector3 pos = _shotPos.transform.position;
        float op = Random.value >= 0.5f ? 1 : -1;
        pos.x += Mathf.Sin(Random.value) * op * _halfWidth;

        // 左半分なら左へ飛ばす
        Vector2 speed = _maxBulletVector * _bulletSpeed;
        speed.x *= op * Random.value;

        // 生成（座標と回転付き）
        GameObject bullet = Instantiate(_enemyBullet, pos, transform.rotation);

        // 設定
        bullet.GetComponent<EnemyBullet02>().SetBullet(speed, _attackPower);

        _audioManager.OnEnemyShotPlay();
    }

    // プレイヤーのスキル効果を受ける
    // type : スキルの種類
    public override void ReceiveSkill(Skill.TYPE type)
    {
        base.ReceiveSkill(type);

        switch (type)
        {
            case Skill.TYPE.THE_WORLD:
                // 弾速と射撃間隔の変更
                _bulletSpeed *= Skill_TheWorld._magBulletSpeed;
                _shot_PerSeconds *= Skill_TheWorld._magNextBullet;

                break;

        }
    }

}
