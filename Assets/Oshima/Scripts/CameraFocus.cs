using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour {

    Camera _camera;
    [SerializeField] List<GameObject> _obj; // 照準合わせたい対象
    int _index = 0;                         // 照準合わせたい対象の番号
    bool _isLiner = false;                  // 次の対象への線形補間中かどうか
    float _cntLiner = 0;                    // 線形補間用の時間計測バッファ

    Vector3 _prePos, _tagPos;               // 出発点, 照準を合わせたい点
    float _preOrthoSize, _tagOrthoSize;     // 出発時の画面範囲, 合わせたい範囲

    Vector3 _baseSize;  // 画面範囲が１のときの範囲（この数値を基準とします）
    float span = 1.0f;
    float currentTime = 0f;


    // Use this for initialization
    void Start () {
        // 基準となる範囲を求める
        {
            // Camera コンポーネント取得
            _camera = GetComponent<Camera>();

            // 元の画面範囲を保存
            float buf = _camera.orthographicSize;

            // 基準となる範囲を求める
            _camera.orthographicSize = 1;   // 画面範囲"1"を基準とする
            Vector3 leftTop = GetComponent<Camera>().ScreenToWorldPoint(Vector3.zero);  // 左上座標
            Vector3 rightBottom = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));    // 右下座標
            Vector3 size = rightBottom - leftTop;   // 差分の算出
            _baseSize = size;   // 保存

            //Debug.Log(_baseSize);

        }


    }

    // Update is called once per frame
    void Update () {

        bool isInput = true;
        int index = _index;

        currentTime += Time.deltaTime;

        if(currentTime > span && index < 4){
            ++index;
            currentTime = 0f;
        }

        // 照準を合わせる対象の変更（例として入力制御にしてます）
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ++index;
            isInput = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            --index;
            isInput = true;
        }

        // 目標座標と描画範囲を算出
        if (isInput)
        {
            // 照準を合わせたい対象番号を変更
            ChangeFocusObj(index);

            // 目標座標の設定
            SetAtPosition();

            // 目標画面範囲の設定
            SetDrawSize();

            // カウンターの初期化
            _cntLiner = 0;

            // カメラ移動開始
            _isLiner = true;

        }

        // 座標と描画範囲の変更
        if (_isLiner)
        {
            // 0.5秒で変更終了の場合
            _cntLiner += Time.deltaTime / 0.5f;

            // 線形補間の終了判定
            if (_cntLiner >= 1)
            {// 終了
                transform.position = _tagPos;
                _isLiner = false;
            }
            else
            {// 継続
                transform.position = Vector3.Lerp(_prePos, _tagPos, _cntLiner * (2 - _cntLiner));
                _camera.orthographicSize = Mathf.Lerp(_preOrthoSize, _tagOrthoSize, _cntLiner * (2 - _cntLiner));
            }
        }
    }

    // 照準を合わせたい対象の変更
    void ChangeFocusObj(int index)
    {
        if (index >= _obj.Count)
            index = 0;
        else if (index < 0)
            index = _obj.Count - 1;

        _index = index;

    }

    // 目標座標の設定
    void SetAtPosition()
    {
        // 目標座標を求める（今回はオブジェクトの中心点にしてます）
        Vector3 tagPos = _obj[_index].transform.position;
        tagPos.z = _camera.transform.position.z;

        // 線形補間時の開始座標の設定
        _prePos = transform.position;

        // 目標座標を設定
        _tagPos = tagPos;
    }

    // 目標範囲の設定
    void SetDrawSize()
    {
        // 目標画面範囲を求める（今回はオブジェクトの大きさにしてます）
        Vector3 tagSize = _obj[_index].GetComponent<SpriteRenderer>().bounds.size;
        tagSize.x = tagSize.x / _baseSize.x;
        tagSize.y = tagSize.y / _baseSize.y;

        // 目標画面範囲の補正（今回はＹ軸とＸ軸で、大きいほうに合わせてます）
        float ortho = tagSize.x > tagSize.y ? tagSize.x : tagSize.y;

        // 線形補間時の開始範囲の設定
        _preOrthoSize = _camera.orthographicSize;

        // 目標範囲の設定
        _tagOrthoSize = ortho;
    }


}
