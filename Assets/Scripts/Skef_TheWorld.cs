using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skef_TheWorld : MonoBehaviour {

    Vector3 _scale;
    float _angle;
    float _alpha;

    bool _start;

    Color _color;

	// Use this for initialization
	void Start () {
        _color = GetComponent<SpriteRenderer>().color;

        Init();

    }

    // Update is called once per frame
    void Update () {
        if (!_start)
            return;

        Quaternion rotate = Quaternion.AngleAxis(_angle, new Vector3(0, 0, 1));
        _angle += 360 * Time.deltaTime;
        transform.rotation = rotate;

        _scale += new Vector3(30,30,30) * Time.deltaTime;
        transform.localScale = _scale;

        if(_scale.x > 20)
        {
            _alpha -= Time.deltaTime;
            Color color = _color;
            color.a = _alpha;
            GetComponent<SpriteRenderer>().color = color;


            if(_alpha < 0)
            {
                EndEffect();
            }

        }

    }

    void Init()
    {
        _angle = 0;
        _scale = new Vector3(0, 0, 0);
        transform.localScale = _scale;

        _alpha = 1;

        _start = false;

    }

    public void StartEffect()
    {
        Init();
        _start = true;
        gameObject.SetActive(true);
    }

    public void EndEffect()
    {
        Init();
        gameObject.SetActive(false);
    }

}
