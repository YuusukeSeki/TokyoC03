using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSprite : MonoBehaviour {
    public enum Switch
    {
        STAND,  // 立ち
        SLIDE,  // 滑り
        CHANGE, // ※切り替え
    };

    SpriteRenderer _sr;
    public Sprite _SpriteStanding;
    public Sprite _SpriteSliding;
    Switch _switch;


    // Use this for initialization
    void Start () {
        _sr = GetComponent<SpriteRenderer>();
        _switch = Switch.STAND;

	}
	
    // 画像の変更
    public void Change(Switch spriteSwitch)
    {
        if (spriteSwitch == _switch)
            return;

        switch(spriteSwitch)
        {
            case Switch.STAND:
                _sr.sprite = _SpriteStanding;
                _switch = spriteSwitch;
                break;

            case Switch.SLIDE:
                _sr.sprite = _SpriteSliding;
                _switch = spriteSwitch;
                break;

            case Switch.CHANGE:
                if(_switch != Switch.STAND)
                {
                    _sr.sprite = _SpriteStanding;
                    _switch = Switch.STAND;
                }
                else
                {
                    _sr.sprite = _SpriteSliding;
                    _switch = Switch.SLIDE;
                }
                break;

        }

    }

}
