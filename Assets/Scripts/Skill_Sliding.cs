using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sliding : Skill
{
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] ChangeSprite _changeSprite;
    [SerializeField] BoxCollider2D _boxCollider2D;
    Vector2 _before_bc2D_offset;
    Vector2 _before_bc2D_size;
    [SerializeField] CapsuleCollider2D _capsuleCollider2D;

    [SerializeField] float _time;   // 効果時間
    float _cntTime;                 // 時間で解除


    // Use this for initialization
    void Start () {
        _cntTime = -1;

        _before_bc2D_offset = _boxCollider2D.offset;
        _before_bc2D_size  = _boxCollider2D.size;

    }

    // Update is called once per frame
    void Update () {

        // 使われていなければ無処理
        if (_cntTime < 0)
            return;

        // 効果時間を超えたら終了
        _cntTime -= Time.deltaTime;
        if(_cntTime < 0)
        {
            // 元に戻す
            Reset();

        }

	}

    // スキルを使う
    public override void UseSkill()
    {
        base.UseSkill();

        _cntTime = _time;

        SetSliding();

    }

    // スライディング状態に切り替える
    void SetSliding()
    {
        // 画像を切り替える
        Change(ChangeSprite.Switch.SLIDE);

        // コリジョンを修正する
        ResizeCollider(false);

        // 座標を修正する
        transform.position += new Vector3(0, -_spriteRenderer.bounds.size.y * 0.5f, 0);

    }

    // 元に戻す
    void Reset()
    {
        // 画像を戻す
        Change(ChangeSprite.Switch.STAND);

        // コリジョンを戻す
        ResizeCollider(true);

        // 座標を修正する
        transform.position += new Vector3(0, +_spriteRenderer.bounds.size.y * 0.5f, 0);

    }

    // コリジョンの修正
    void ResizeCollider(bool reset)
    {
        if(!reset)
        {
            _boxCollider2D.offset = new Vector2(0, 0);
            _boxCollider2D.size = _spriteRenderer.bounds.size;
            _capsuleCollider2D.enabled = false;

        }
        else
        {
            _boxCollider2D.offset = _before_bc2D_offset;
            _boxCollider2D.size = _before_bc2D_size;
            _capsuleCollider2D.enabled = true;
        }

    }

    // 画像の切り替え
    void Change(ChangeSprite.Switch cs_switch)
    {
        _changeSprite.Change(cs_switch);

    }

}
