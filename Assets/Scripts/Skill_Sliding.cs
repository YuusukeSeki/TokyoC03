using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sliding : Skill
{
    //SpriteRenderer _spriteRenderer;
    //ChangeSprite _changeSprite;
    //BoxCollider2D _boxCollider2D;
    //CapsuleCollider2D _capsuleCollider2D;
    //Vector2 _before_bc2D_offset;
    //Vector2 _before_bc2D_size;

    [SerializeField] float _time;   // 効果時間
    float _cntTime;                 // 時間計測

    public enum State
    {
        MAE,
        USHIRO,
        ZENKEI,
    }

    [SerializeField] State _state;          // どう回転するのか
    [SerializeField] bool _isLinear;        // 線形補間する、しない
    [SerializeField] float _linearTime;     // 目標回転率になるまでにかかる時間（線形補間をしない場合は、関係ない）
    float _cntLinearTime;                   // 時間計測
    [SerializeField] float _zenkeiKakduo;   // 前傾姿勢時の目標角度

    Quaternion _q1;             // 現在の回転率
    Quaternion _q2;             // 目標の回転率

    enum RotateState
    {
        NORMAL,
        START,
        STOP,
        END,
    }

    RotateState _rotateState;


    // Use this for initialization
    void Start()
    {
        //_spriteRenderer    = GetComponent<SpriteRenderer>();
        //_changeSprite      = GetComponent<ChangeSprite>();
        //_boxCollider2D     = GetComponent<BoxCollider2D>();
        //_capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        //_before_bc2D_offset = _boxCollider2D.offset;
        //_before_bc2D_size  = _boxCollider2D.size;

        Init();

    }

    // Update is called once per frame
    void Update()
    {
        if (_cntTime <= 0)
            return;

        // 状態の変化
        ChangeState();

        // 回転率の変化
        ChangeRotate();




        //// 効果時間外
        //if (_cntTime <= 0)
        //    return;

        //// 効果時間内
        //_cntTime -= Time.deltaTime;
        //if (_cntTime <= 0)
        //{
        //    // 元に戻す
        //    Reset();

        //}

    }

    protected override void Init()
    {
        base.Init();

        _type = TYPE.NONE;

        _cntTime = -1;

        switch (_state)
        {
            case State.MAE:
                _q2 = Quaternion.Euler(0f, 0f, -90f); ;
                break;

            case State.USHIRO:
                _q2 = Quaternion.Euler(0f, 0f,  90f); ;
                break;

            case State.ZENKEI:
                _q2 = Quaternion.Euler(0f, 0f, -_zenkeiKakduo); ;
                break;

        }

        _rotateState = RotateState.NORMAL;

        _cntLinearTime = 0;

    }

    // スキルを使う
    public override void UseSkill()
    {
        base.UseSkill();

        SetSliding();

        _audioManager.OnSlidePlay();
    }

    // スライディング状態に切り替える
    void SetSliding()
    {
        _cntTime = _time;
        _cntLinearTime = 0;

        // 目標回転率の設定
        switch (_state)
        {
            case State.MAE:
                _q2 = Quaternion.Euler(0f, 0f, -90f); ;
                break;

            case State.USHIRO:
                _q2 = Quaternion.Euler(0f, 0f, 90f); ;
                break;

            case State.ZENKEI:
                _q2 = Quaternion.Euler(0f, 0f, -_zenkeiKakduo); ;
                break;

        }

        // 初期回転率
        _q1 = Quaternion.Euler(0, 0, 0);
        transform.rotation = _q1;
        _rotateState = RotateState.START;



        //// 画像を切り替える
        //Change(ChangeSprite.Switch.SLIDE);

        //// コリジョンを修正する
        //ResizeCollider(false);

        //// 座標を修正する
        //transform.position += new Vector3(0, -_spriteRenderer.bounds.size.y * 0.5f, 0);

    }

    //// 元に戻す
    //void Reset()
    //{
    //    // 画像を戻す
    //    Change(ChangeSprite.Switch.STAND);

    //    // コリジョンを戻す
    //    ResizeCollider(true);

    //    // 座標を修正する
    //    //transform.position += new Vector3(0, +_spriteRenderer.bounds.size.y * 0.5f, 0);

    //}

    //// コリジョンの修正
    //void ResizeCollider(bool reset)
    //{
    //    //if(!reset)
    //    //{
    //    //    _boxCollider2D.offset = new Vector2(0, 0);
    //    //    _boxCollider2D.size = _spriteRenderer.bounds.size;
    //    //    _capsuleCollider2D.enabled = false;

    //    //}
    //    //else
    //    //{
    //    //    _boxCollider2D.offset = _before_bc2D_offset;
    //    //    _boxCollider2D.size = _before_bc2D_size;
    //    //    _capsuleCollider2D.enabled = true;
    //    //}

    //}

    //// 画像の切り替え
    //void Change(ChangeSprite.Switch cs_switch)
    //{
    //    //_changeSprite.Change(cs_switch);

    //}

    // 状態の変化
    void ChangeState()
    {
        _cntTime -= Time.deltaTime;

        RotateState rs = _rotateState;

        // 線形補間の有無
        if (_isLinear)
        {// 有り
            if (_cntTime <= 0)
                _rotateState = RotateState.NORMAL;

            else if (_cntTime >= _time - _linearTime)
                _rotateState = RotateState.START;

            else if (_cntTime <= _linearTime)
                _rotateState = RotateState.END;

            else
                _rotateState = RotateState.STOP;

        }
        else
        {// 無し
            if (_cntTime <= 0)
            {
                transform.rotation = _q1;

                Vector3 pos = transform.position;
                pos.y += GetComponent<SpriteRenderer>().bounds.size.y * 0.5f;
                transform.position = pos;
            }

            else
            {
                transform.rotation = _q2;

            }

        }

        if (_isLinear)
        {
            if (rs != _rotateState)
            {
                switch (_rotateState)
                {
                    case RotateState.NORMAL:
                        _cntLinearTime = 0;
                        transform.rotation = _q1;
                        break;

                    case RotateState.STOP:
                        _cntLinearTime = 0;
                        transform.rotation = _q2;
                        break;

                    case RotateState.END:
                        _cntLinearTime = 0;
                        break;
                }
            }

        }

    }

    // 回転率の変化
    void ChangeRotate()
    {
        if (_isLinear)
        {
            switch (_rotateState)
            {
                case RotateState.NORMAL:
                    _cntLinearTime = 0;
                    transform.rotation = _q1;
                    break;

                case RotateState.START:
                    _cntLinearTime += Time.deltaTime / _linearTime;
                    transform.rotation = Quaternion.Lerp(_q1, _q2, _cntLinearTime);
                    break;

                case RotateState.STOP:
                    _cntLinearTime = 0;
                    transform.rotation = _q2;
                    break;

                case RotateState.END:
                    _cntLinearTime += Time.deltaTime / _linearTime;
                    transform.rotation = Quaternion.Lerp(_q2, _q1, _cntLinearTime);
                    break;

            }
        }

    }

}
