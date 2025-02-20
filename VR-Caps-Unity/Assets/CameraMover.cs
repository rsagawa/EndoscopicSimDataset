using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;


public class CameraMover : MonoBehaviour
{

    //Camera movement amount
    [SerializeField, Range(0.01f, 10.0f)]
    private float _positionStep = 0.1f;

    //Mouse sensitivity
    [SerializeField, Range(0f, 2f)]
    private float _mouseSensitive = 0.05f;

    //Enable/disable camera control
    private bool _cameraMoveActive = true;
    //Camera transform  
    private Transform _camTransform;
    private Transform _camTransformf;

    private Quaternion q;

    private Vector3 _startTransform;

    //Mouse starting point
    private Vector3 _startMousePos;
    //Camera rotation starting point information
    private Vector3 _presentCamRotation;
    private Vector3 _presentCamPos;

    //Initial state rotation
    private Quaternion _initialCamRotation;
    //Display UI message
    private bool _uiMessageActiv;


    void Start()
    {
        _camTransform = this.transform;

        float tX = transform.position.x;
        float tY = transform.position.y;
        float tZ = transform.position.z;

        float rX = this.transform.rotation.x;
        float rY = this.transform.rotation.y;
        float rZ = this.transform.rotation.z;
        float rW = this.transform.rotation.w;
    }

    void Update()
    {
        if (_cameraMoveActive)
        {
            CameraRotationMouseControl();

            CameraPositionKeyControl();

            float tX = transform.position.x;
            float tY = transform.position.y;
            float tZ = transform.position.z;

            if (Input.GetMouseButtonUp(0))
            {
                float rX = transform.rotation.x;
                float rY = transform.rotation.y;
                float rZ = transform.rotation.z;
                float rW = transform.rotation.w;
            }
        }
    }

    private void CameraRotationMouseControl()
    {
        if (Input.GetMouseButtonDown(0))//Left mouse button press
        {
            _startMousePos = Input.mousePosition;
            _presentCamRotation.x = _camTransform.transform.eulerAngles.x;
            _presentCamRotation.y = _camTransform.transform.eulerAngles.y;
        }

        if (Input.GetMouseButton(0))//Held down Left mouse button 
        {
            //Normalized by (starting position - current mouse position) / resolution
            float x = (_startMousePos.x - Input.mousePosition.x) / Screen.width;
            float y = -(_startMousePos.y - Input.mousePosition.y) / Screen.height;

            //Rotation start angle (at the time of click) + mouse movement amount * mouse sensitivity
            float eulerX = Input.GetAxis("Mouse X") + y * _mouseSensitive;
            float eulerY = Input.GetAxis("Mouse Y") + x * _mouseSensitive;

            Quaternion beforeR = _camTransform.localRotation;//Mouse click coordinates

            Quaternion Rotate = Quaternion.Euler(eulerY, -eulerX, 0);//Rotation amount

            _camTransform.localRotation = beforeR * Rotate;//Specify camera angle with rotation after rotating
            //_camTransform.rotation= beforeR * Rotate;//When using world coordinates, the screen becomes erratic during clicks.
        }
    }

    //Camera local movement key
    private void CameraPositionKeyControl()
    {
        _camTransform = this.transform;
        //Before = this._camTransform.position;

        Vector3 campos = _camTransform.localPosition;

        //　Convert position from world to local using transform.InverseTransformPoint(Vector3 value);
        //  campos = transform.InverseTransformPoint(campos);

        float rX = _camTransform.rotation.x;
        float rY = _camTransform.rotation.y;
        float rZ = _camTransform.rotation.z;
        float rW = _camTransform.rotation.w;

        Vector3 right;
        Vector3 up;
        Vector3 forward;

        //　Convert direction vector from world to local using transform.InverseTransformDirection(Vector3 value)
        right = _camTransform.InverseTransformDirection(Vector3.right);
        up = _camTransform.InverseTransformDirection(Vector3.up);
        forward = _camTransform.InverseTransformDirection(Vector3.forward);

        //Quaternion q = Quaternion.Inverse(_camTransform.localRotation);
        Quaternion q = _camTransform.localRotation;

        if (Input.GetKey(KeyCode.D))
        {
            campos += q * Vector3.right * Time.deltaTime * _positionStep;
        }
        if (Input.GetKey(KeyCode.A))
        {
            campos -= q * Vector3.right * Time.deltaTime * _positionStep;
        }
        if (Input.GetKey(KeyCode.E))
        {
            campos -= q * Vector3.up * Time.deltaTime * _positionStep;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            campos += q * Vector3.up * Time.deltaTime * _positionStep;
        }
        if (Input.GetKey(KeyCode.W))
        {
            campos += q * Vector3.forward * Time.deltaTime * _positionStep;
        }
        if (Input.GetKey(KeyCode.S))
        {
            campos -= q * Vector3.forward * Time.deltaTime * _positionStep;
        }

        _camTransform.localPosition = campos;
    }
}
