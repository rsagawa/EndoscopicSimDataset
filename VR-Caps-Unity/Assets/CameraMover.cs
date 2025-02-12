using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

//内視鏡

public class CameraMover : MonoBehaviour
{

    //カメラの移動量
    [SerializeField, Range(0.01f, 10.0f)]
    private float _positionStep = 0.1f;////////////////////

    //マウス感度

    [SerializeField, Range(0f, 2f)]
    private float _mouseSensitive = 0.05f;

    //カメラ操作の有効無効
    private bool _cameraMoveActive = true;
    //カメラのtransform  
    private Transform _camTransform;
    private Transform _camTransformf;


    private Vector3 _startTransform;

    //マウスの始点 
    private Vector3 _startMousePos;
    //カメラ回転の始点情報
    private Vector3 _presentCamRotation;
    private Vector3 _presentCamPos;

    //初期状態 Rotation
    private Quaternion _initialCamRotation;
    //UIメッセージの表示
    private bool _uiMessageActiv;


    private Quaternion q;





    void Start()
    {

        _camTransform = this.transform;

        //Debug.Log("Start");


        float tX = transform.position.x;
        float tY = transform.position.y;
        float tZ = transform.position.z;


        float rX = this.transform.rotation.x;
        float rY = this.transform.rotation.y;
        float rZ = this.transform.rotation.z;
        float rW = this.transform.rotation.w;




        //Debug.Log("初期Quaternion　　" + rX + "　,　" + rY + "　,　" + rZ + "　,　" + rW);//初期状態のクオータニオン

        //Debug.Log("初期Quaternion　　" + Quaternion.x + "　,　" + Quaternion.y + "　,　" + Quaternion.z + "　,　" + Quaternion.w);//初期状態のクオータニオン
        //Debug.Log("初期localPosition　" + _camTransform.localPosition.x + "　,　" + _camTransform.localPosition.y + "　,　" + _camTransform.localPosition.z);
        //Debug.Log("初期localRotation　" + _camTransform.localRotation.x + "　,　" + _camTransform.localRotation.y + "　,　" + _camTransform.localRotation.z + "　,　" + _camTransform.localRotation.w);
        //Debug.Log("--------------------------------------------------------");



    }

    void Update()
    {
        //CamControlIsActive(); //カメラ操作の有効無効

        if (_cameraMoveActive)
        {
            CameraRotationMouseControl(); //カメラの回転 マウス///////////////////////////////////////////////

            CameraPositionKeyControl(); //カメラのローカル移動 キー

            float tX = transform.position.x;
            float tY = transform.position.y;
            float tZ = transform.position.z;



            if (Input.GetMouseButtonUp(0))
            {
                float rX = transform.rotation.x;
                float rY = transform.rotation.y;
                float rZ = transform.rotation.z;
                float rW = transform.rotation.w;


                //Debug.Log(rX + "　,　" + rY + "　,　" + rZ + "　,　" + rW);//回転後クオータニオン
            }
        }


    }

    //カメラの回転 マウス
    private void CameraRotationMouseControl()
    {
        if (Input.GetMouseButtonDown(0))//左押された。
        {
            //Debug.Log("in");
            _startMousePos = Input.mousePosition;
            _presentCamRotation.x = _camTransform.transform.eulerAngles.x;
            _presentCamRotation.y = _camTransform.transform.eulerAngles.y;


        }

        if (Input.GetMouseButton(0))//押されたまま。
        {
            //(移動開始座標 - マウスの現在座標) / 解像度 で正規化
            float x = (_startMousePos.x - Input.mousePosition.x) / Screen.width;
            float y = -(_startMousePos.y - Input.mousePosition.y) / Screen.height;

            //回転開始角度（クリックしたときの） ＋ マウスの変化量 * マウス感度





            float eulerX = Input.GetAxis("Mouse X") + y * _mouseSensitive;
            float eulerY = Input.GetAxis("Mouse Y") + x * _mouseSensitive;

            //Debug.Log(_mouseSensitive);

            Quaternion beforeR = _camTransform.localRotation;//マウスクリックした時の座標

            Quaternion Rotate = Quaternion.Euler(eulerY, -eulerX, 0);//回転分

            _camTransform.localRotation = beforeR * Rotate;//回転させた後のrotationでカメラの角度指定



            //_camTransform.rotation= beforeR * Rotate;//World座標にしてみるとクリック中に画面が荒ぶる。


        }
    }

    //カメラのローカル移動 キー
    private void CameraPositionKeyControl()
    {

        //Debug.Log("inin");
        _camTransform = this.transform;


        //Before = this._camTransform.position;

        Vector3 campos = _camTransform.localPosition;
        //campos = transform.InverseTransformPoint(campos);///////////////

        //　方向ベクトルをワールド→ローカルへと変える transform.InverseTransformDirection(Vector3の値);
        //　位置をワールド→ローカルへと変える transform.InverseTransformPoint(Vector3の値);




        float rX = _camTransform.rotation.x;
        float rY = _camTransform.rotation.y;
        float rZ = _camTransform.rotation.z;
        float rW = _camTransform.rotation.w;


        if (Input.GetKeyDown(KeyCode.D))//押した瞬間
        {
            //Debug.Log("移動前D　" + this._camTransform.localPosition.x + " , " + this._camTransform.localPosition.y + " , " + this._camTransform.localPosition.z);
            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Debug.Log("移動前A　" + this._camTransform.localPosition.x + " , " + this._camTransform.localPosition.y + " , " + this._camTransform.localPosition.z);
            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("移動前E　" + this._camTransform.localPosition.x + " , " + this._camTransform.localPosition.y + " , " + this._camTransform.localPosition.z);
            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Debug.Log("移動前Q　" + this._camTransform.localPosition.x + " , " + this._camTransform.localPosition.y + " , " + this._camTransform.localPosition.z);
            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //Debug.Log("移動前W　" + this._camTransform.localPosition.x + " , " + this._camTransform.localPosition.y + " , " + this._camTransform.localPosition.z);
            //Debug.Log("--------------------------------------------------------");
        }//
        if (Input.GetKeyDown(KeyCode.S))
        {
            //Debug.Log("移動前S　" + this._camTransform.localPosition.x + " , " + this._camTransform.localPosition.y + " , " + this._camTransform.localPosition.z);
            //Debug.Log("--------------------------------------------------------");
        }//

        Vector3 right;
        Vector3 up;
        Vector3 forward;

        right = _camTransform.InverseTransformDirection(Vector3.right);
        up = _camTransform.InverseTransformDirection(Vector3.up);
        forward = _camTransform.InverseTransformDirection(Vector3.forward);


        /////////////////////

        //Quaternion q = Quaternion.Inverse(_camTransform.localRotation);
        Quaternion q = _camTransform.localRotation;

        /////////////////////

        if (Input.GetKey(KeyCode.D))
        {
            campos += q * Vector3.right * Time.deltaTime * _positionStep;


            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKey(KeyCode.A))
        {
            campos -= q * Vector3.right * Time.deltaTime * _positionStep;


            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKey(KeyCode.E))
        {
            campos -= q * Vector3.up * Time.deltaTime * _positionStep;


            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKey(KeyCode.Q))
        {
            campos += q * Vector3.up * Time.deltaTime * _positionStep;


            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKey(KeyCode.W))
        {
            campos += q * Vector3.forward * Time.deltaTime * _positionStep;


            //Debug.Log("--------------------------------------------------------");
        }//
        if (Input.GetKey(KeyCode.S))
        {
            campos -= q * Vector3.forward * Time.deltaTime * _positionStep;


            //Debug.Log("--------------------------------------------------------");
        }//






        if (Input.GetKeyUp(KeyCode.D))//キーを離した瞬間
        {
            //Debug.Log("D:　+" + q * Vector3.right);

            //Debug.Log("移動後Quaternion　　" + rX + "　,　" + rY + "　,　" + rZ + "　,　" + rW);

            //Debug.Log("移動後localPosition　" + _camTransform.localPosition.x + "　,　" + _camTransform.localPosition.y + "　,　" + _camTransform.localPosition.z);
            //Debug.Log("移動後localRotation　" + _camTransform.localRotation.x + "　,　" + _camTransform.localRotation.y + "　,　" + _camTransform.localRotation.z + "　,　" + _camTransform.localRotation.w);
            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            //Debug.Log("A:　-" + q * Vector3.right);

            //Debug.Log("移動後Quaternion　　" + rX + "　,　" + rY + "　,　" + rZ + "　,　" + rW);

            //Debug.Log("移動後localPosition　" + _camTransform.localPosition.x + "　,　" + _camTransform.localPosition.y + "　,　" + _camTransform.localPosition.z);
            //Debug.Log("移動後localRotation　" + _camTransform.localRotation.x + "　,　" + _camTransform.localRotation.y + "　,　" + _camTransform.localRotation.z + "　,　" + _camTransform.localRotation.w);
            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            //Debug.Log("E:　-" + q * Vector3.up);

            //Debug.Log("移動後Quaternion　　" + rX + "　,　" + rY + "　,　" + rZ + "　,　" + rW);

            //Debug.Log("移動後localPosition　" + _camTransform.localPosition.x + "　,　" + _camTransform.localPosition.y + "　,　" + _camTransform.localPosition.z);
            //Debug.Log("移動後localRotation　" + _camTransform.localRotation.x + "　,　" + _camTransform.localRotation.y + "　,　" + _camTransform.localRotation.z + "　,　" + _camTransform.localRotation.w);
            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            //Debug.Log("Q:　+" + q * Vector3.up);

            //Debug.Log("移動後Quaternion　　" + rX + "　,　" + rY + "　,　" + rZ + "　,　" + rW);

            //Debug.Log("移動後localPosition　" + _camTransform.localPosition.x + "　,　" + _camTransform.localPosition.y + "　,　" + _camTransform.localPosition.z);
            //Debug.Log("移動後localRotation　" + _camTransform.localRotation.x + "　,　" + _camTransform.localRotation.y + "　,　" + _camTransform.localRotation.z + "　,　" + _camTransform.localRotation.w);
            //Debug.Log("--------------------------------------------------------");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            //Debug.Log("W:　+" + q * Vector3.forward);

            // Debug.Log("移動後Quaternion　　" + rX + "　,　" + rY + "　,　" + rZ + "　,　" + rW);

            //Debug.Log("移動後localPosition　" + _camTransform.localPosition.x + "　,　" + _camTransform.localPosition.y + "　,　" + _camTransform.localPosition.z);
            //Debug.Log("移動後localRotation　" + _camTransform.localRotation.x + "　,　" + _camTransform.localRotation.y + "　,　" + _camTransform.localRotation.z + "　,　" + _camTransform.localRotation.w);
            //Debug.Log("--------------------------------------------------------");
        }//
        if (Input.GetKeyUp(KeyCode.S))
        {
            //Debug.Log("S:　-" + q * Vector3.forward);

            //Debug.Log("移動後Quaternion　　" + rX + "　,　" + rY + "　,　" + rZ + "　,　" + rW);

            //Debug.Log("移動後localPosition　" + _camTransform.localPosition.x+"　,　" + _camTransform.localPosition.y + "　,　" + _camTransform.localPosition.z);
            //Debug.Log("移動後localRotation　" + _camTransform.localRotation.x+"　,　" + _camTransform.localRotation.y + "　,　" + _camTransform.localRotation.z + "　,　" + _camTransform.localRotation.w);
            //Debug.Log("--------------------------------------------------------");
        }//


        //Vector3(0,0,1).forward

        _camTransform.localPosition = campos;

    }
}
