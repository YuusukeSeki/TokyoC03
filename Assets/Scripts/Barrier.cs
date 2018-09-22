using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {

    GameObject _followObject;

	// Use this for initialization
	void Start () {
        // 以前に貼ったバリアを破棄
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        foreach (GameObject barrier in barriers)
        {
            if (barrier.gameObject != gameObject)
                Destroy(barrier.gameObject);
        }
    }

    // Update is called once per frame
    void Update () {
        if (!_followObject)
            return;

        //// 親のアクティブがOFFなら自分は消える
        //if (!_followObject.activeSelf)
        //    enabled = false;
        //else
        //    enabled = true;

        transform.position = _followObject.transform.position;

    }

    public void SetBarrier(GameObject followObject)
    {
        _followObject = followObject;
    }

    void OnColiisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Obstacle")
        {
            _followObject.GetComponent<Player>().ReceiveDamage(0);

            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Bullet(Enemy)")
        {
            Destroy(col.gameObject);

            _followObject.GetComponent<Player>().ReceiveDamage(0);

            Destroy(gameObject);
        }
        else if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Obstacle")
        {
            _followObject.GetComponent<Player>().ReceiveDamage(0);

            Destroy(gameObject);
        }
    }

}
