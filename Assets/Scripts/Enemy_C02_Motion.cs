using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_C02_Motion : MonoBehaviour {

    [SerializeField] List<GameObject> _obj;

    struct MotionKey
    {
        public Vector3 position;
        public Quaternion quaternion;
    };

    struct MotionKey_Frame
    {
        public float time;
        public MotionKey[] key;
    };

    MotionKey_Frame[] _motion;
    float _cntTime = 0;
    int _key = 0;

    int KEY_MAX = 2;


    // Use this for initialization
    void Start () {
        //_obj[0].transform.parent = null;
        //_obj[1].transform.parent = _obj[0].transform;
        //_obj[2].transform.parent = _obj[1].transform;
        //_obj[3].transform.parent = _obj[1].transform;
        //_obj[4].transform.parent = _obj[0].transform;
        //_obj[5].transform.parent = _obj[4].transform;
        //_obj[6].transform.parent = _obj[4].transform;
        //_obj[7].transform.parent = _obj[0].transform;
        //_obj[8].transform.parent = _obj[7].transform;
        //_obj[9].transform.parent = _obj[7].transform;
        //_obj[10].transform.parent = _obj[0].transform;
        //_obj[11].transform.parent = _obj[10].transform;
        //_obj[12].transform.parent = _obj[10].transform;

        
        _motion = new MotionKey_Frame[KEY_MAX];
        _motion[0].key = new MotionKey[_obj.Count];
        _motion[1].key = new MotionKey[_obj.Count];

        _motion[0].time = 1;
        _motion[1].time = 1;

        for (int i = 0; i < KEY_MAX; i++)
        {
            for(int j = 0; j < _obj.Count; ++j)
            {
                _motion[i].key[j].position = _obj[j].transform.position;
                _motion[i].key[j].quaternion = Quaternion.AngleAxis(0, new Vector3(0, 0, 1));
            }
        }

        _motion[1].key[0].position  = new Vector3(0, 1, 0);
        _motion[1].key[1].position  = new Vector3(0, 0.8f, 0);
        _motion[1].key[2].position  = new Vector3(0, 0.8f, 0);
        _motion[1].key[3].position  = new Vector3(0, 0.8f, 0);
        _motion[1].key[4].position  = new Vector3(0, 0.8f, 0);
        _motion[1].key[5].position  = new Vector3(0, 0.8f, 0);
        _motion[1].key[6].position  = new Vector3(0, 0.8f, 0);
        _motion[1].key[7].position  = new Vector3(0, 0.8f, 0);
        _motion[1].key[8].position  = new Vector3(0, 0.8f, 0);
        _motion[1].key[9].position  = new Vector3(0, 0.8f, 0);
        _motion[1].key[10].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[11].position = new Vector3(0, 0.8f, 0);
        _motion[1].key[12].position = new Vector3(0, 0.8f, 0);

        _motion[1].key[0].quaternion  = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1));
        _motion[1].key[1].quaternion  = Quaternion.AngleAxis(-20, new Vector3(0, 0, 1));
        _motion[1].key[2].quaternion  = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1));
        _motion[1].key[3].quaternion  = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1));
        _motion[1].key[4].quaternion  = Quaternion.AngleAxis(-30, new Vector3(0, 0, 1));
        _motion[1].key[5].quaternion  = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1));
        _motion[1].key[6].quaternion  = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1));
        _motion[1].key[7].quaternion  = Quaternion.AngleAxis(+20, new Vector3(0, 0, 1));
        _motion[1].key[8].quaternion  = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1));
        _motion[1].key[9].quaternion  = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1));
        _motion[1].key[10].quaternion = Quaternion.AngleAxis(+30, new Vector3(0, 0, 1));
        _motion[1].key[11].quaternion = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1));
        _motion[1].key[12].quaternion = Quaternion.AngleAxis(  0, new Vector3(0, 0, 1));

        Init();
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < _obj.Count; ++i)
        {
            //_obj[i].transform.position = Vector3.Lerp(_motion[_key].key[i].position, _motion[_key + 1 >= KEY_MAX ? 0 : _key + 1].key[i].position, _cntTime);
            _obj[i].transform.localRotation = Quaternion.Lerp(_motion[_key].key[i].quaternion, _motion[_key + 1 >= KEY_MAX ? 0 : _key + 1].key[i].quaternion, _cntTime);

        }

        //_obj[1].transform.rotation = Quaternion.Lerp(_motion[_key].key[1].quaternion, _motion[_key + 1 >= KEY_MAX ? 0 : _key + 1].key[1].quaternion, _cntTime);
        //_obj[2].transform.rotation = Quaternion.Lerp(_motion[_key].key[2].quaternion, _motion[_key + 1 >= KEY_MAX ? 0 : _key + 1].key[2].quaternion, _cntTime);
        //_obj[3].transform.rotation = Quaternion.Lerp(_motion[_key].key[3].quaternion, _motion[_key + 1 >= KEY_MAX ? 0 : _key + 1].key[3].quaternion, _cntTime);


        _cntTime += Time.deltaTime;

        if (_cntTime >= _motion[_key].time)
        {
            _key = _key + 1 >= KEY_MAX ? 0 : _key + 1;
            _cntTime = 0;
        }

    }

    public void Motion()
    {

    }

    void Init()
    {
        

    }





}
