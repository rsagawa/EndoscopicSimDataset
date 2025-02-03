using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;
using System.Threading;

//カメラパスの保存

public class CameraPathSave : MonoBehaviour////MonoBehaviorは基本的にはPlay中に起動する
{
    StreamWriter sw;

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
//csvが生成される
sw = new StreamWriter(@"shudo1_csv.csv", false, Encoding.GetEncoding("Shift_JIS"));//VR-Caps-Unityのこれに保存される。

        // CSVのヘッダー部分
        string[] s1 = { "Pos_X", "Pos_Y", "Pos_Z", "localPos_X", "localPos_Y", "localPos_Z", "Rot_X", "Rot_Y", "Rot_Z", "Rot_W", "localRot_X", "localRot_Y", "localRot_Z", "localRot_W", "count" };
        string s2 = string.Join(",", s1);
        sw.WriteLine(s2);


        StartCoroutine("WriteCSV");
    }



    IEnumerator WriteCSV()
    {

        TextAsset csvFile = Resources.Load("shudo1_csv") as TextAsset;////Resourcesフォルダにcsvを入れておく

        StringReader reader = new StringReader(csvFile.text);//【読み込んでreaderとする】



        var wait = new WaitForSeconds((float)0.1);////////////////////////()の中の秒数ごとに書かれる


        while (true)
        {

            yield return wait;


            _camTransform = this.transform;



            Vector3 campos = _camTransform.localPosition;
            //campos = transform.InverseTransformPoint(campos);///////////////

            //　方向ベクトルをワールド→ローカルへと変える transform.InverseTransformDirection(Vector3の値);
            //　位置をワールド→ローカルへと変える transform.InverseTransformPoint(Vector3の値);

            //Quaternion q = Quaternion.Inverse(_camTransform.localRotation);//

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

            sw.WriteLine(str2);


            if (Input.GetKeyDown(KeyCode.D))//押した瞬間
            {

                sw.WriteLine(str2);

            }

            if (Input.GetKeyUp(KeyCode.D))//キーを離した瞬間
            {

                sw.WriteLine(str2);

            }

            if (Input.GetKeyDown(KeyCode.A))
            {

                sw.WriteLine(str2);

            }

            if (Input.GetKeyUp(KeyCode.A))
            {

                sw.WriteLine(str2);

            }

            if (Input.GetKeyDown(KeyCode.E))
            {

                sw.WriteLine(str2);

            }

            if (Input.GetKeyUp(KeyCode.E))
            {

                sw.WriteLine(str2);

            }

            if (Input.GetKeyDown(KeyCode.Q))
            {

                sw.WriteLine(str2);

            }

            if (Input.GetKeyUp(KeyCode.Q))
            {

                sw.WriteLine(str2);

            }

            if (Input.GetKeyDown(KeyCode.W))
            {

                sw.WriteLine(str2);

            }//

            if (Input.GetKeyUp(KeyCode.W))
            {

                sw.WriteLine(str2);

            }//

            if (Input.GetKeyDown(KeyCode.S))
            {

                sw.WriteLine(str2);

            }//

            if (Input.GetKeyUp(KeyCode.S))
            {

                sw.WriteLine(str2);

            }//

            _camTransform.localPosition = campos;





            if (Input.GetKey(KeyCode.Space))
            {
                sw.Close();
//swと同じファイル名前にする
StreamReader sr = new StreamReader(@"shudo1_csv.csv", Encoding.GetEncoding("Shift_JIS"));



                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //Debug.Log(line);
                }
                sr.Close();


                Debug.Log("書き込み完了");
                break;
            }
        }
    }//WriteCSV終わり

}