using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {

    GameObject _followObject = null;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (!_followObject)
            return;

        // 親のアクティブがOFFなら自分は消える
        if (!_followObject.activeSelf)
            Destroy(gameObject);

        transform.position = _followObject.transform.position;

    }

    public void SetBarrier(GameObject followObject)
    {
        _followObject = followObject;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet(Enemy)")
        {// 被弾判定
            Destroy(col.gameObject);
            Destroy(gameObject);
        }

    }

}
