using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_E : Enemy {

    public float _amplitude;        // 振幅
    private int _frameCnt;          // フレームカウント
    private Vector3 _basePosition;  // 基点

    //public float _moveSpeed;

    [SerializeField] PlayerManager _playerManager;


    // Use this for initialization
    void Start () {
        Init();
	}

    // Update is called once per frame
    void Update()
    {
        _frameCnt++;
        if (10000 <= _frameCnt)
        {
            _frameCnt = 0;
        }

        // プレイヤーの方向を求める
        Vector3 targetVec = _playerManager._charaLists[_playerManager._nowChara].transform.position - _basePosition;
        targetVec.Normalize();

        // 移動の基点をプレイヤー方向へずらす
        _basePosition += targetVec * _moveSpeed.x * Time.deltaTime;

        // 左右に振動させる（ふわふわを表現）
        float posXSin = Mathf.Sin(2.0f * Mathf.PI * (float)(_frameCnt % 200) / (200.0f - 1.0f));
        posXSin *= _amplitude;

        // 基点に振動を足した座標を求める
        Vector3 pos = _basePosition;
        pos.x += posXSin;

        // 進行方向に応じて向きを変える
        Vector3 scale = transform.localScale;
        if (pos.x < transform.position.x)
            scale.x = 1;
        else
            scale.x = -1;

        // Objectに適用
        transform.position = pos;
        transform.localScale = scale;

    }

    public override void Init()
    {
        base.Init();

        _frameCnt = 0;
        _basePosition = transform.position;

    }
}
