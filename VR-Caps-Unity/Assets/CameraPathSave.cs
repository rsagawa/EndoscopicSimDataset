using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using System.Threading;
using System.Diagnostics;

//カメラパスの保存

public class CameraPathSave : MonoBehaviour////MonoBehaviorは基本的にはPlay中に起動する
{

#if UNITY_EDITOR
    [SerializeField] private string Save_Path = "";
#endif

    private bool isRecording = false;

    private List<string> poses = new List<string>();

    //StreamWriter sw;

    //private float count = 0;

    //カメラの移動量
    [SerializeField, Range(0.01f, 100.0f)]

    private float _positionStep = 0.1f;////////////////////

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
    //private bool _uiMessageActiv;


    //private Quaternion q;
    //float X;
    //float Y;
    //float Z;




    void Start()
    {
        //StartCoroutine("WriteCSV");
        StartCoroutine("Capture");
    }

    IEnumerator Capture()
    {
        while (true)
        {
            yield return null;

            if (Input.GetKey(KeyCode.Space))
            {
                //UnityEngine.Debug.Log("Capture Space.");
                var wait = new WaitForSeconds((float)0.5);
                isRecording = true;
                yield return wait;
                yield return Record();
                UnityEngine.Debug.Log("●Recording done.");
            }

            if (Input.GetKey(KeyCode.N))
            {
                string write_path = Save_Path;

                string Save_CSV_DIR = Path.GetDirectoryName(write_path);

                if (!Directory.Exists(Save_CSV_DIR))
                {
                    UnityEngine.Debug.Log("Cant Find Save CSV Path.");
                    yield break;
                }

                yield return write_csv(poses, write_path);

                isRecording = false;

                var wait2 = new WaitForSeconds((float)0.5);
                yield return wait2;
            }
        }
    }

    IEnumerator write_csv(List<string> poses, string path)
    {
        //UnityEngine.Debug.Log("Writing CSV file...");
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

        //UnityEngine.Debug.Log("Writing done.");

        yield return null;
    }

    IEnumerator Record()
    {
        UnityEngine.Debug.Log("●Recording start.");

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
                    UnityEngine.Debug.Log("Cant Find Save CSV Path.");
                    yield break;
                }

                yield return write_csv(poses, write_path);

                isRecording = false;

                var wait2 = new WaitForSeconds((float)0.5);
                yield return wait2;
            }

        }
    }

    //IEnumerator WriteCSV()
    //{

    //    UnityEngine.Debug.Log("●Recording start");

    //    //TextAsset csvFile = Resources.Load("test_csv") as TextAsset;////Resourcesフォルダにcsvを入れておく
    //    //StringReader reader = new StringReader(csvFile.text);//【読み込んでreaderとする】

    //    StringReader reader = new StringReader(@"test_csv.csv");//【読み込んでreaderとする】



    //    var wait = new WaitForSeconds((float)0.2);////////////////////////()の中の秒数ごとに書かれる #0.1


    //    while (true)
    //    {

    //        yield return wait;
    //        //yield return null;

    //        _camTransform = this.transform;



    //        Vector3 campos = _camTransform.localPosition;
    //        //campos = transform.InverseTransformPoint(campos);///////////////

    //        //　方向ベクトルをワールド→ローカルへと変える transform.InverseTransformDirection(Vector3の値);
    //        //　位置をワールド→ローカルへと変える transform.InverseTransformPoint(Vector3の値);

    //        //Quaternion q = Quaternion.Inverse(_camTransform.localRotation);//

    //        float pX = transform.position.x;
    //        float pY = transform.position.y;
    //        float pZ = transform.position.z;


    //        float lpX = transform.localPosition.x;
    //        float lpY = transform.localPosition.y;
    //        float lpZ = transform.localPosition.z;

    //        float rX = transform.rotation.x;
    //        float rY = transform.rotation.y;
    //        float rZ = transform.rotation.z;
    //        float rW = transform.rotation.w;


    //        float lrX = transform.localRotation.x;
    //        float lrY = transform.localRotation.y;
    //        float lrZ = transform.localRotation.z;
    //        float lrW = transform.localRotation.w;

    //        Vector3 right;
    //        Vector3 up;
    //        Vector3 forward;

    //        right = _camTransform.InverseTransformDirection(Vector3.right);
    //        up = _camTransform.InverseTransformDirection(Vector3.up);
    //        forward = _camTransform.InverseTransformDirection(Vector3.forward);


    //        string[] str = { (pX).ToString(), (pY).ToString(), (pZ).ToString(), (lpX).ToString(), (lpY).ToString(), (lpZ).ToString(), (rX).ToString(), (rY).ToString(), (rZ).ToString(), (rW).ToString(), (lrX).ToString(), (lrY).ToString(), (lrZ).ToString(), (lrW).ToString() };
    //        string str2 = string.Join(",", str);

    //        //if (sw != null && !sw.BaseStream.CanWrite)
    //        //{
    //        sw.WriteLine(str2);
    //        //}

    //        if (Input.GetKeyDown(KeyCode.D))//押した瞬間
    //        {

    //            sw.WriteLine(str2);

    //        }

    //        if (Input.GetKeyUp(KeyCode.D))//キーを離した瞬間
    //        {

    //            sw.WriteLine(str2);

    //        }

    //        if (Input.GetKeyDown(KeyCode.A))
    //        {

    //            sw.WriteLine(str2);

    //        }

    //        if (Input.GetKeyUp(KeyCode.A))
    //        {

    //            sw.WriteLine(str2);

    //        }

    //        if (Input.GetKeyDown(KeyCode.E))
    //        {

    //            sw.WriteLine(str2);

    //        }

    //        if (Input.GetKeyUp(KeyCode.E))
    //        {

    //            sw.WriteLine(str2);

    //        }

    //        if (Input.GetKeyDown(KeyCode.Q))
    //        {

    //            sw.WriteLine(str2);

    //        }

    //        if (Input.GetKeyUp(KeyCode.Q))
    //        {

    //            sw.WriteLine(str2);

    //        }

    //        if (Input.GetKeyDown(KeyCode.W))
    //        {

    //            sw.WriteLine(str2);

    //        }//

    //        if (Input.GetKeyUp(KeyCode.W))
    //        {

    //            sw.WriteLine(str2);

    //        }//

    //        if (Input.GetKeyDown(KeyCode.S))
    //        {

    //            sw.WriteLine(str2);

    //        }//

    //        if (Input.GetKeyUp(KeyCode.S))
    //        {

    //            sw.WriteLine(str2);

    //        }//

    //        _camTransform.localPosition = campos;





    //        //if (Input.GetKey(KeyCode.Space))
    //        //{
    //        //    sw.Close();
    //        //    //swと同じファイル名前にする
    //        //    StreamReader sr = new StreamReader(@"shudo1_csv.csv", Encoding.GetEncoding("Shift_JIS"));

    //        //    UnityEngine.Debug.Log("Writing CSV file...");

    //        //    string line;
    //        //    while ((line = sr.ReadLine()) != null)
    //        //    {
    //        //        //Debug.Log(line);
    //        //    }
    //        //    sr.Close();

    //        //    UnityEngine.Debug.Log("done.");
    //        //    //break;
    //        //    yield break;
    //        //}

    //        if (Input.GetKeyDown(KeyCode.L))
    //        {
    //            //if (Time.time - lastPressTime > debounceTime)
    //            //{
    //            //    //isRecording = true;
    //            //    lastPressTime = Time.time;
    //            //    sw.Close();
    //            //    //swと同じファイル名前にする
    //            //    StreamReader sr = new StreamReader(@"shudo1_csv.csv", Encoding.GetEncoding("Shift_JIS"));

    //            //    UnityEngine.Debug.Log("Writing CSV file...");

    //            //    string line;
    //            //    while ((line = sr.ReadLine()) != null)
    //            //    {
    //            //        //Debug.Log(line);
    //            //    }
    //            //    sr.Close();

    //            //    UnityEngine.Debug.Log("done.");
    //            //    isRecording = false;
    //            //    yield break;
    //            //}
    //            //isRecording = true;
    //            //UnityEngine.Debug.Log("isRecording: " + isRecording);
    //            isRecording = true;
    //        }

    //        if (Input.GetKeyDown(KeyCode.L) && isRecording)
    //        {
    //            //UnityEngine.Debug.Log("Writing CSV file...");

    //            isRecording = false;

    //            sw.Close();

    //            UnityEngine.Debug.Log("done.");

    //            yield return wait;

    //            yield break;

    //            //yield return Write();
    //        }

    //    }
    //}//WriteCSV終わり

}