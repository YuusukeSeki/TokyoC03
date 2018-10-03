using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skef_Score_LI : MonoBehaviour {

    Image _image;

    bool _flash;        // 点滅フラグ
    float _time;
    float _alpha;       // α値
    bool _reverse;      // 反転フラグ

    [SerializeField] float _alphaUpTime, _alphaDownTime;
    [SerializeField] GameObject _player;


    // Use this for initialization
    void Start()
    {
        _image = GetComponent<Image>();

        _flash = true;     // 点滅フラグ
        _alpha = 1;         // α値
        _reverse = true;    // 反転フラグ

    }

    // Update is called once per frame
    void Update()
    {
        if (!_flash)
            return;

        _time -= Time.deltaTime;

        if (_reverse)
        {
            _alpha -= Time.deltaTime / _alphaDownTime;   // 規則的

            if (_alpha <= 0)
            {
                _alpha = 0;
                _reverse = !_reverse;
            }
        }
        else
        {
            _alpha += Time.deltaTime / _alphaUpTime;   // 規則的

            if (_alpha >= 1)
            {
                _alpha = 1;
                _reverse = !_reverse;
            }
        }

        GetComponent<Image>().color = new Color(1, 1, 1, _alpha);

        if (_time <= 0)
        {
            _player.GetComponent<Skill_ScoreRate>().EndSkill();
        }

    }

    // 点滅開始
    public void StartFlash(float time)
    {
        _flash = true;
        _time = time;

    }

    // 点滅終了
    public void EndFlash()
    {
        _flash = false;
        _alpha = 0;
        _reverse = true;

        _image.color = new Color(1, 1, 1, _alpha);
    }

}
