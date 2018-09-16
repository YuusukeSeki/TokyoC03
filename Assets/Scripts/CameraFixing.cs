using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixing : MonoBehaviour {

    PlayerManager _pm;
    float _offsetX;
    public Vector2 _screenSize;

    enum State
    {
        NORMAL,
        CHARACTER_CHANGE,
    };

    State _state;
    Vector2 _movePos;
    float _cntTime;
    float _charaChangeTime;

	// Use this for initialization
	void Start () {
        _pm = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        Vector3 leftTop = GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero);
        Vector3 rightBottom = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        Vector3 size = rightBottom - leftTop;
        _screenSize = new Vector2(size.x, size.y);
        _offsetX = _screenSize.x * 0.25f;
        //_offsetX = 0;

        _state = State.NORMAL;
        _movePos = new Vector2(0, 0);
        _cntTime = 0;

    }

    // Update is called once per frame
    void Update () {
        Vector3 pos = new Vector3(0, 0, 0);
        pos = transform.position;

        if (_state == State.NORMAL)
        {
            pos.x = _pm._charaLists[_pm._nowChara].transform.position.x + _offsetX;
            pos.y = 0;

            transform.position = pos;
        }
        else if(_state == State.CHARACTER_CHANGE)
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
    }

    // 強制的にカメラを動かしたいときに呼ぶやつ
    public void CalledChangeCharacter(Vector2 movePos, float time)
    {
        _state = State.CHARACTER_CHANGE;

        // 交代にかかる時間と移動量を保存
        _charaChangeTime = time;
        _movePos = movePos;

    }


}
