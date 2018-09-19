using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_C : Enemy {

    // 一定距離まで近づいたらプレイヤー座標へ向けて移動、辿り着いたらそのまま左に行く
    // 重力有り、感知範囲あり、目指す位置あり、到達フラグ有り

    [SerializeField] Sensor _sensor;

    [SerializeField] float _moveForce;
    [SerializeField] float _moveMaxSpeed;
    bool _isArrived;

    bool _isSearch;
    Vector3 _targetPos;
    Vector3 _targetVec;
    float _bufLength;

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

    public override void Init()
    {
        base.Init();

        _moveSpeed = new Vector3(0, 0, 0);
        _isArrived = false;

        _isSearch = false;
        _targetPos = new Vector3(0, 0, 0);
        _targetVec = new Vector3(0, 0, 0);
        _bufLength = 0;
    }

    void Move()
    {
        if(!_isArrived)
        {
            _moveSpeed += _moveForce * _targetVec * Time.deltaTime;

            if (Mathf.Abs(_moveSpeed.magnitude) > _moveMaxSpeed)
            {
                _moveSpeed = _moveMaxSpeed * _targetVec;
            }

            Vector3 bufPos = transform.position;

            transform.position += _moveSpeed * Time.deltaTime;

            _bufLength -= (transform.position - bufPos).magnitude;

            if (_bufLength <= 0)
                _isArrived = true;

        }
        else
        {
            transform.position += new Vector3(_moveSpeed.x * Time.deltaTime, 0, 0);
        }

    }

    void SetTargetPosition()
    {
        _isSearch = true;
        _targetPos = _sensor._targetPos;
        _targetVec = _sensor._targetPos - transform.position;
        _targetVec.Normalize();

        _bufLength = (_targetPos - transform.position).magnitude;

    }

    public override void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
