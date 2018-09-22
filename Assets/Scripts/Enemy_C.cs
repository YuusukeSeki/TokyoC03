using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_C : Enemy {

    // 移動：〇、弾：×

    // プレイヤーを感知してそちらに向かう
    // 一度向かったら二度は追わない

    [SerializeField] float _moveSpeed;  // 移動速度

    [SerializeField] Sensor _sensor;    // プレイヤー検知用スクリプト
    bool _isSearch;     // プレイヤー検知フラグ
    Vector3 _targetVec; // 目標座標への方向
    float _bufLength;   // 現在地点から目標地点までの長さ
    bool _isArrived;    // 目標地点到達フラグ



    // Use this for initialization
    void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {

        // 未発見
        if(!_isSearch)
        {
            // センサーに反応があったか調べる
            if(_isSearch != _sensor._isSearch)
            {// 有り
                // 目標座標設定
                SetTargetPosition();
            }
            else
                // 無し
                return;
        }

        // 移動
        Move();

	}

    // 初期化処理
    public override void Init()
    {
        base.Init();

        _isSearch = false;
        _targetVec = new Vector3(0, 0, 0);
        _bufLength = 0;
        _isArrived = false;
    }

    // 移動処理
    void Move()
    {
        if(!_isArrived)
        {
            //_moveSpeed += _moveForce * _targetVec * Time.deltaTime;

            //if (Mathf.Abs(_moveSpeed.magnitude) > _moveMaxSpeed)
            //{
            //    _moveSpeed = _moveMaxSpeed * _targetVec;
            //}

            Vector3 bufPos = transform.position;

            transform.position += _moveSpeed * _targetVec * Time.deltaTime;

            _bufLength -= (transform.position - bufPos).magnitude;

            if (_bufLength <= 0)
                _isArrived = true;

        }
        else
        {
            transform.position += new Vector3(_moveSpeed * Time.deltaTime, 0, 0);
        }

    }

    // 目標座標の設定
    void SetTargetPosition()
    {
        _isSearch = true;
        _targetVec = _sensor._targetPos - transform.position;
        _targetVec.Normalize();

        _bufLength = (_sensor._targetPos - transform.position).magnitude;

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

    // センサーに引っかかった場合はダメージを与えない
    protected override void OnTriggerEnter2D(Collider2D col)
    {

    }
    protected override void OnTriggerStay2D(Collider2D col)
    {

    }

}
