using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_C02 : Enemy {

    [SerializeField] float _amplitude;  // 振幅
    Vector3 _basePosition;              // 基点
    float _angle = 0;

    [SerializeField] float _moveSpeed;  // 移動速度

    static float _limitPosY;    // Y座標の下限
    static float _spece = 0.1f; // 下限に加える余白


    // Use this for initialization
    void Start()
    {
        // Y座標の下限を決める
        // 地面のY座標と高さを取得
        // プレイヤーの高さを取得
        // 自分の高さを取得
        // _limitPosY = 地面のY座標 + 地面の高さ * 0.5f + プレイヤーの高さ + 自分の高さ * 0.5f + 余分な幅
        float groundPosY = GameObject.Find("floor").transform.position.y;
        float groundHalfHeight = GameObject.Find("floor").GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;
        float playerHeight = GameObject.Find("PlayerManager").GetComponent<PlayerManager>()._charaLists[0].GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;

        //_limitPosY = groundPosY + groundHalfHeight + playerHeight + GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;
        _limitPosY = groundPosY + groundHalfHeight + playerHeight;
        _limitPosY += _spece;

        // 基点の設定
        _basePosition = transform.position;

        // 初期化処理
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 左右に振動させる（ふわふわを表現）
        _angle += _moveSpeed * Time.deltaTime;
        float posXSin = Mathf.Sin(_angle) * _amplitude;

        // 基点に振動を足した座標を求める
        Vector3 pos = _basePosition;
        pos.x += posXSin;

        // 進行方向に応じて向きを変える
        Vector3 scale = transform.localScale;
        if (pos.x < transform.position.x)
        {
            scale.x = 1;
        }
        else
        {
            scale.x = -1;
        }

        // 下限判定
        if (pos.y < _limitPosY)
        {
            pos.y = _limitPosY;
        }

        // Objectに適用
        transform.position = pos;
        transform.localScale = scale;

    }

    // 初期化処理
    public override void Init()
    {
        base.Init();

        _angle = 0;
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
                _moveSpeed *= Skill_TheWorld._magMoveSpeed;

                break;

        }
    }
}
