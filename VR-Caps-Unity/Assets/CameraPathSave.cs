using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using System.Threading;
using System.Diagnostics;

//Save camera path

public class CameraPathSave : MonoBehaviour//MonoBehavior is basically activated during Play mode
{

#if UNITY_EDITOR
    [SerializeField] private string Save_Path = "";
#endif

    private bool isRecording = false;

    private List<string> poses = new List<string>();

    //Camera movement amount
    [SerializeField, Range(0.01f, 100.0f)]
    private float _positionStep = 0.1f;

    [SerializeField, Range(0f, 2f)]
    private float _mouseSensitive = 0.05f;

    //Enable/disable camera control
    private bool _cameraMoveActive = true;

    //Camera transform
    private Transform _camTransform;
    private Transform _camTransformf;

    private Vector3 _startTransform;

    //Mouse starting point
    private Vector3 _startMousePos;
    //Camera Rotation Start Information
    private Vector3 _presentCamRotation;
    private Vector3 _presentCamPos;

    //Initial State Rotation
    private Quaternion _initialCamRotation;

    //Display UI Message
    //private bool _uiMessageActiv;

    void Start()
    {
        StartCoroutine("Capture");
    }

    IEnumerator Capture()
    {
        while (true)
        {
            yield return null;

            if (Input.GetKey(KeyCode.Space))
            {
                var wait = new WaitForSeconds((float)0.5);
                isRecording = true;
                yield return wait;
                yield return Record();
                UnityEngine.Debug.Log($"<color=red>{"●Recording Stop."}</color>");
                var wait2 = new WaitForSeconds((float)0.5);
                yield return wait2;
            }

            //if (Input.GetKey(KeyCode.N))
            //{
            //    string write_path = Save_Path;
            //    string Save_CSV_DIR = Path.GetDirectoryName(write_path);
            //    if (!Directory.Exists(Save_CSV_DIR))
            //    {
            //        UnityEngine.Debug.Log("Cant Find Save CSV Path.");
            //        yield break;
            //    }
            //    yield return write_csv(poses, write_path);
            //    isRecording = false;
            //    var wait2 = new WaitForSeconds((float)0.5);
            //    yield return wait2;
            //}
        }
    }

    IEnumerator write_csv(List<string> poses, string path)
    {

        UnityEngine.Debug.Log("Saved: " + path);

        StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("Shift_JIS"));

        string[] s1 = { "Pos_X", "Pos_Y", "Pos_Z", "localPos_X", "localPos_Y", "localPos_Z", "Rot_X", "Rot_Y", "Rot_Z", "Rot_W", "localRot_X", "localRot_Y", "localRot_Z", "localRot_W", "count" };
        string s2 = string.Join(",", s1);

        sw.WriteLine(s2);

        for (int i = 0; i < poses.Count; i++)
        {
            sw.WriteLine(poses[i]);
        }

        sw.Close();

        yield return null;
    }

    IEnumerator Record()
    {
        UnityEngine.Debug.Log($"<color=red>{"●Recording start."}</color>");

        var wait = new WaitForSeconds((float)0.1);


        while (isRecording)
        {

            yield return wait;

            _camTransform = this.transform;

            Vector3 campos = _camTransform.localPosition;

            float pX = transform.position.x;
            float pY = transform.position.y;
            float pZ = transform.position.z;


            float lpX = transform.localPosition.x;
            float lpY = transform.localPosition.y;
            float lpZ = transform.localPosition.z;

            float rX = transform.rotation.x;
            float rY = transform.rotation.y;
            float rZ = transform.rotation.z;
            float rW = transform.rotation.w;


            float lrX = transform.localRotation.x;
            float lrY = transform.localRotation.y;
            float lrZ = transform.localRotation.z;
            float lrW = transform.localRotation.w;

            Vector3 right;
            Vector3 up;
            Vector3 forward;

            right = _camTransform.InverseTransformDirection(Vector3.right);
            up = _camTransform.InverseTransformDirection(Vector3.up);
            forward = _camTransform.InverseTransformDirection(Vector3.forward);


            string[] str = { (pX).ToString(), (pY).ToString(), (pZ).ToString(), (lpX).ToString(), (lpY).ToString(), (lpZ).ToString(), (rX).ToString(), (rY).ToString(), (rZ).ToString(), (rW).ToString(), (lrX).ToString(), (lrY).ToString(), (lrZ).ToString(), (lrW).ToString() };
            string str2 = string.Join(",", str);

            poses.Add(str2);

            if (Input.GetKeyDown(KeyCode.D))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyUp(KeyCode.Q))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                poses.Add(str2);
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                poses.Add(str2);
            }

            _camTransform.localPosition = campos;

            //if (Input.GetKey(KeyCode.Space))
            //{
            //    isRecording = false;

            //    var wait2 = new WaitForSeconds((float)0.5);
            //    yield return wait2;
            //}

            //if (Input.GetKey(KeyCode.N))
            if (Input.GetKey(KeyCode.Space))
                {
                string write_path = Save_Path;

                string Save_CSV_DIR = Path.GetDirectoryName(write_path);

                if (!Directory.Exists(Save_CSV_DIR))
                {
                    UnityEngine.Debug.Log($"<color=red>{"Can't find path to save csv file."}</color>");
                    yield break;
                }

                yield return write_csv(poses, write_path);

                isRecording = false;

                var wait2 = new WaitForSeconds((float)0.5);
                yield return wait2;
            }

        }
    }
}