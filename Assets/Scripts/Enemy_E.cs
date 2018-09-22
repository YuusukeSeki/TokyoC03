using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_E : Enemy {

    // 移動：〇、弾：×

    // 横にふわふわしながらプレイヤーを追尾する

    public float _amplitude;        // 振幅
    private Vector3 _basePosition;  // 基点

    public float _homingSpeed;
    int _leftCnt, _rightCnt;

    [SerializeField] PlayerManager _playerManager;

    [SerializeField] float _rotateSpeed = 1;
    float _angle = 0;

    // Use this for initialization
    void Start () {
        Init();
	}

    // Update is called once per frame
    void Update()
    {
        // プレイヤーの方向を求める
        Vector3 targetVec = _playerManager._charaLists[_playerManager._nowChara].transform.position - _basePosition;
        targetVec.Normalize();

        // 移動の基点をプレイヤー方向へずらす
        _basePosition += targetVec * _homingSpeed * Time.deltaTime;

        // 左右に振動させる（ふわふわを表現）
        _angle += _rotateSpeed * Time.deltaTime;
        float posXSin = Mathf.Sin(_angle) * _amplitude;

        // 基点に振動を足した座標を求める
        Vector3 pos = _basePosition;
        pos.x += posXSin;

        // 進行方向に応じて向きを変える
        Vector3 scale = transform.localScale;
        if (pos.x < transform.position.x)
        {
            _leftCnt++;
            _rightCnt = 0;

            if(_leftCnt >= 3)
                scale.x = 1;
        }
        else
        {
            _leftCnt = 0;
            _rightCnt++;

            if (_rightCnt >= 3)
                scale.x = -1;
        }

        // Objectに適用
        transform.position = pos;
        transform.localScale = scale;

    }

    // 初期化処理
    public override void Init()
    {
        base.Init();

        _basePosition = transform.position;
        _leftCnt = 0;
        _rightCnt = 0;
    }

    // プレイヤーのスキル効果を受ける
    // type : スキルの種類
    public override void ReceiveSkill(Skill.TYPE type)
    {
        base.ReceiveSkill(type);

        switch (type)
        {
            case Skill.TYPE.THE_WORLD:
                // 移動速度の変更
                _homingSpeed *= Skill_TheWorld._magMoveSpeed;
                _rotateSpeed *= Skill_TheWorld._magMoveSpeed;

                break;

        }
    }

}
