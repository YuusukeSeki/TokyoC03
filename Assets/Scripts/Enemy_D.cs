using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_D : Enemy {

    [SerializeField] GameObject _shotPos;
    [SerializeField] GameObject _enemyBullet;

    [SerializeField] Vector2 _bulletVector;        // 弾速
    [SerializeField] float _shot_PerSeconds;    // 秒間発射数
    float _cntTime;
    float _halfWidth;


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
        float op = Random.value - 0.5f >= 0 ? 1 : -1;
        pos.x += Mathf.Sin(Random.value) * op * _halfWidth;

        // 生成（座標と回転付き）
        GameObject bullet = Instantiate(_enemyBullet, pos, transform.rotation);

        // 左半分なら左へ飛ばす
        Vector2 speed = _bulletVector * _bulletSpeed;
        speed.x *= op;

        // 設定
        bullet.GetComponent<EnemyBullet02>().SetBullet(speed, _attackPower);

    }

}
