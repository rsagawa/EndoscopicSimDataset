using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;
using System.Diagnostics;

[Serializable, VolumeComponentMenu("Post-processing/Custom/DepthShader")]

public class RGBSave : MonoBehaviour, IPostProcessComponent
{
#if UNITY_EDITOR
    [SerializeField] private string SaveFilePath = "";    //Unityエディタ上で保存パスを指定
    [SerializeField] private bool Pattern = false;
#endif

    public ClampedFloatParameter depthDistance = new ClampedFloatParameter(1f, 0f, 32f);

    // private List<List<string>> csvDatas = new List<List<string>>();
    // private List<List<string>> csvDatas3 = new List<List<string>>();

    List<string> filenames = new List<string>
    {
        "colon_polyp1",
        "colon_polyp2",
        "colon1",
        "colon2",
        "colon3",
        "colon4",
        "colon5",
        "colon6",
        "intestine1",
        "intestine2",
        "intestine3",
        "intestine4",
        "intestine5",
        "intestine6",
        "small_intestine1",
        "stomach1",
        "stomach2",
        "stomach3"
    };


    //DateTime startDt = DateTime.Now;

    Material m_Material;

    public bool IsActive() => m_Material != null;

    //public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    public void SwitchSpotLight()
    {
        GameObject parentObject = this.gameObject;   //GameObject.Find("Capsule");
        //parentObject = GameObject.Find(SpotLightPath);

        if (parentObject != null)
        {
            // 階層全体のパスを使ってオブジェクトを探す
            Transform TurnOnObjectTransform = parentObject.transform.Find("SpotLight");
            Transform TurnOffObjectTransform = parentObject.transform.Find("SpotLightpattern");

            if (TurnOnObjectTransform != null && TurnOffObjectTransform != null)
            {
                GameObject TurnOnObject = TurnOnObjectTransform.gameObject;
                GameObject TurnOffObject = TurnOffObjectTransform.gameObject;

                // スポットライトの有効/無効を切り替える
                TurnOnObject.SetActive(!Pattern);
                TurnOffObject.SetActive(Pattern);
            }
            else
            {
                try
                {
                    if (TurnOnObjectTransform == null)
                    {
                        UnityEngine.Debug.Log("Target SpotLight object not found");
                    }
                    if (TurnOffObjectTransform == null)
                    {
                        UnityEngine.Debug.Log("Target SpotLightPattern object not found");
                    }
                }
                catch (FileNotFoundException ex)
                {
                    // オブジェクトが見つからない場合の処理
                    UnityEngine.Debug.Log("Target Light objects not found");
                }
            }
        }
        else
        {
            UnityEngine.Debug.Log("Parent object of SpotLight not found");
        }
    }

    public void Setup()
    {
        if (Shader.Find("Hidden/Shader/DepthShader") != null)
            m_Material = new Material(Shader.Find("Hidden/Shader/DepthShader"));
    }


    IEnumerator ReadCSV()
    {
        UnityEngine.Debug.Log("inCSV");

        //////////////////////////////
        Vector3 posi = this.transform.position;
        Vector3 posi3 = this.transform.position;
        //////////////////////////////

        //ここでファイル名を必要に応じて変える
        //Resourcesフォルダを作って読み込むcsvデータを入れておく, .csvの前までの名前を入れる
        //TextAsset csvFile = Resources.Load("shudo1_csv") as TextAsset;
        // TextAsset csvFile = Resources.Load("stop") as TextAsset;
        // TextAsset csvFile = Resources.Load("depth_calib") as TextAsset;

        foreach (string filename in filenames)
        {

            TextAsset csvFile = Resources.Load(filename) as TextAsset;

            //保存先指定
            //string path = "C:\\Users\\juranmar7993\\github\\pushtest\\V2\\VR-Caps-Unity\\Assets\\ScreenShot_PNG";
            //        string path = "C:\\Users\\juranmar7993\\Desktop\\VirtualCapsuleEndoscopy\\VR-Caps-Unity\\Assets\\ScreenShot_PNG\\";
            //string path = "C:\\Users\\rsagawa41766\\Documents\\src\\20230407_VR-Caps\\test";
            string path = SaveFilePath; // "D:\\torii\\projects\\VRcaps画像列抽出Unity\\data\\demo";

            path = path + "\\" + filename + "_RGB\\";
            //path = path + "\\" + filename + "_RGB_pat\\";

            Directory.CreateDirectory(path);

            StringReader reader = new StringReader(csvFile.text);//【読み込んでreaderとする】
            StringReader reader2 = new StringReader(csvFile.text);//【読み込んでreader2とする】
            StringReader reader3 = new StringReader(csvFile.text);//【読み込んでreader3とする】

            List<List<string>> csvDatas = new List<List<string>>();
            List<List<string>> csvDatas3 = new List<List<string>>();

            int count = 0;
            int counts = 0;

            var wait = new WaitForSeconds((float)0.1);

            while (reader2.Peek() != -1)
            {
                string line2 = reader2.ReadLine();//【一行ごとに読み込み(line2:string型)】///////////////////////////////////////////////
                count++;
            }
            // reader2.Close();


            UnityEngine.Debug.Log(count);//csvの行数カウント

            string line3 = reader3.ReadLine();//１行読み飛ばす
            while (reader3.Peek() != -1)
            {
                line3 = reader3.ReadLine();
                List<string> list3 = new List<string>();//【リストの定義】////////////////////////////////////////////

                list3 = line3.Split(',').ToList();//【分割してリスト化】//////////////////////////////////////////////

                csvDatas3.Add(list3);

            }
            // reader3.Close();


            posi3 = new Vector3(float.Parse(csvDatas3[1][0]), float.Parse(csvDatas3[1][1]), float.Parse(csvDatas3[1][2]));

            //////////////////////////////
            this.transform.position = posi3;

            transform.rotation = new Quaternion(float.Parse(csvDatas3[1][3]), float.Parse(csvDatas3[1][4]), float.Parse(csvDatas3[1][5]), float.Parse(csvDatas3[1][6]));
            //Quaternion r3 = Quaternion.Euler(0, 180, 0);

            //this.transform.rotation = transform.rotation * r3;
            //////////////////////////////



            string line = reader.ReadLine();//１行読み飛ばす

            for (int i = 0; i < count - 1; i++)//
            {
                // Debug.Log(i);
                // yield return wait;
                yield return null;
                line = reader.ReadLine();//【一行ごとに読み込み(line:string型)】////////////////////////////////////

                List<string> list = new List<string>();/////////【リストの定義】////////////////////////////////////

                list = line.Split(',').ToList();//【分割してリスト化】//////////////////////////////////////////////

                csvDatas.Add(list);

                posi = new Vector3(float.Parse(csvDatas[i][0]), float.Parse(csvDatas[i][1]), float.Parse(csvDatas[i][2]));
                //posi = new Vector3(float.Parse(csvDatas[i][3]), float.Parse(csvDatas[i][4]), float.Parse(csvDatas[i][5]));

                //////////////////////////////
                this.transform.position = posi;
                //transform.rotation = new Quaternion(float.Parse(csvDatas[i][3]), float.Parse(csvDatas[i][4]), float.Parse(csvDatas[i][5]), float.Parse(csvDatas[i][6]));
                transform.localRotation = new Quaternion(float.Parse(csvDatas[i][10]), float.Parse(csvDatas[i][11]), float.Parse(csvDatas[i][12]), float.Parse(csvDatas[i][13]));
                //this.transform.rotation = transform.rotation * r3;
                //////////////////////////////


                //ファイル名を変更する
                string outputFile = filename + "_" + counts.ToString("00000");


                //RGB画像(.png)保存
                ScreenCapture.CaptureScreenshot(path + outputFile + ".png");



                // var mainCamObj = GameObject.FindGameObjectWithTag("Player");
                // var cam = mainCamObj.GetComponent<Camera>();
                // RenderTexture rTex = cam.targetTexture;

                // Texture2D tex = new Texture2D(320, 320, TextureFormat.RGBAFloat, false);///////////////////////////////////////////
                // //RenderTexture.active = rTex;
                // //tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
                // //tex.Apply();

                // byte[] bytes = tex.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);
                // //GameObject.Destroy(tex);



                // ////深度画像(.exr)保存
                // //File.WriteAllBytes(path + "/EXR" + outputFile + ".exr", bytes);

                // float tX = transform.position.x;
                // float tY = transform.position.y;
                // float tZ = transform.position.z;

                // float ltX = transform.localPosition.x;
                // float ltY = transform.localPosition.y;
                // float ltZ = transform.localPosition.z;

                // float rX = this.transform.rotation.x;
                // float rY = this.transform.rotation.y;
                // float rZ = this.transform.rotation.z;
                // float rW = this.transform.rotation.w;

                //Debug.Log("saveEXRの回数  " + counts + " :    " + tX + "  ,  " + tY + "  ,  " + tZ);

                counts++;

                ////Debug.Log(i+ "   経過時間:     " + ts.TotalMilliseconds + "    :NewBehavior読み込み  " + float.Parse(csvDatas[i][0]));
                //Debug.Log("csv読み込み    　" + i + " :    " + float.Parse(csvDatas[i][0]) + "  ,  " + float.Parse(csvDatas[i][1]) + "  ,  " + float.Parse(csvDatas[i][2]));
                //Debug.Log("csv読み込み    　" + i + " :    " + tX + "  ,  " + tY + "  ,  " + tZ + "  ,  " + rX + "  ,  " + rY + "  ,  " + rZ + "  ,  " + rW);

            }

            // reader.Close();

            UnityEngine.Debug.Log("End");/////////////////////////////////////////////////////////////////////
        }
    }

    void Start()
    {
        SwitchSpotLight();

        StartCoroutine("ReadCSV");
        // ReadCSV0();
    }

    public void Cleanup() => CoreUtils.Destroy(m_Material);

}

#if UNITY_EDITOR
[CustomEditor(typeof(RGBSave))]
public class RGBSaveWindow : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var setter = target as RGBSave;


        //if (!EditorApplication.isPlaying && GUILayout.Button("Reload Spot Light Pattern"))
        //{
        //    setter.SwitchSpotLight();
        //}
        if (!EditorApplication.isPlaying)
        {
            setter.SwitchSpotLight();
        }
    }
}
#endif