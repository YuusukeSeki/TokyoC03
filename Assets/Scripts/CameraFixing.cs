﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixing : MonoBehaviour
{

    [SerializeField] PlayerManager _playerManager = null;
    float _offsetX, _initOffsetX;
    public Vector2 _screenSize;

    public enum State
    {
        NORMAL,
        CHARACTER_CHANGE,
        TUKKAETERU,
    };

    public State _state;
    Vector2 _movePos;
    float _cntTime;
    float _charaChangeTime;

    Vector3 _prePosition;

    // Use this for initialization
    void Start()
    {
        Vector3 leftTop = GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero);
        Vector3 rightBottom = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        Vector3 size = rightBottom - leftTop;
        _screenSize = new Vector2(size.x, size.y);
        _offsetX = _screenSize.x * 0.25f;
        _initOffsetX = _offsetX;

        _state = State.NORMAL;
        _movePos = new Vector2(0, 0);
        _cntTime = 0;

        _prePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _prePosition = transform.position;
        Vector3 pos = transform.position;


        if (_state == State.NORMAL)
        {
            if (_offsetX != _initOffsetX)
            {
                _offsetX -= 1.0f * Time.deltaTime;

                if (_offsetX < _initOffsetX)
                {
                    _offsetX = _initOffsetX;
                }
            }

            pos.x = _playerManager._charaLists[_playerManager._nowChara].transform.position.x + _offsetX;
            pos.y = 0;

            transform.position = pos;
        }
        else if (_state == State.CHARACTER_CHANGE)
        {
            _cntTime += Time.deltaTime;

            if (_cntTime >= _charaChangeTime)
            {
                _state = State.NORMAL;
                _cntTime = 0;
            }
            else
            {
                pos.x += _movePos.x * (Time.deltaTime / _charaChangeTime);
                pos.y += _movePos.y * (Time.deltaTime / _charaChangeTime);
            }

            transform.position = pos;
        }
        else if (_state == State.TUKKAETERU)
        {
            _offsetX += 1.0f * Time.deltaTime;

            pos.x = _playerManager._charaLists[_playerManager._nowChara].transform.position.x + _offsetX;
            pos.y = 0;

            transform.position = pos;

            if (_playerManager.GetMainCharacterPosition().x + _playerManager._charaLists[_playerManager._nowChara].GetComponent<SpriteRenderer>().bounds.size.x
                < transform.position.x - _screenSize.x * 0.5f)
            {
                Debug.Log("画面外まで押し出されました。GameOverです");
                _playerManager._state = PlayerManager.State.GAMEOVER;
            }
        }
    }

    // 強制的にカメラを動かしたいときに呼ぶやつ
    public void CalledChangeCharacter(Vector2 movePos, float time)
    {
        _state = State.CHARACTER_CHANGE;

        // 交代にかかる時間と移動量を保存
        _charaChangeTime = time;
        _movePos = movePos;

    }

    public void FocusGameObject(GameObject obj)
    {
        _state = State.NORMAL;

        Vector3 pos = transform.position;

        pos.x = obj.transform.position.x + _offsetX;
        pos.y = 0;

        transform.position = pos;

    }

    public void SetState(State state)
    {
        _state = state;
    }

    public Vector3 Sabun()
    {
        return transform.position - _prePosition;
    }


}