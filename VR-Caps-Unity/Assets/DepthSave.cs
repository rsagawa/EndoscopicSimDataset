using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;
// using MathNet.Numerics.Interpolation;

[Serializable, VolumeComponentMenu("Post-processing/Custom/DepthShader")]

public class DepthSave : MonoBehaviour, IPostProcessComponent
{
#if UNITY_EDITOR
    [SerializeField] private string SaveFilePath = "";    //Unityエディタ上で保存パスを指定
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

        // "depth_calib"
    };

    private int zBufferParamsID;
    
    Material m_Material;

    public bool IsActive() => m_Material != null;

    //public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    public void Setup()
    {
        if (Shader.Find("Hidden/Shader/DepthShader") != null)
            m_Material = new Material(Shader.Find("Hidden/Shader/DepthShader"));
    }


    IEnumerator ReadCSV()
    {
        //////////////////////////////
        Vector3 posi = this.transform.position;
        Vector3 posi3 = this.transform.position;
        //////////////////////////////


        //ここでファイル名を必要に応じて変える
        //Resourcesフォルダを作って読み込むcsvデータを入れておく, .csvの前までの名前を入れる
        // TextAsset csvFile = Resources.Load("shudo1_csv") as TextAsset;
        // TextAsset csvFile = Resources.Load("stop") as TextAsset;
        // TextAsset csvFile = Resources.Load("depth_calib") as TextAsset;


        double[] readData(TextAsset filename)
        {
            StringReader datreader = new StringReader(filename.text);
            // ファイルの各行を読み込んで、配列に格納する
            string[] lines = datreader.ReadToEnd().Split('\n');

            // 出力データを格納するための配列を作成する
            double[] data = new double[lines.Length];

            // 各行を処理して、出力データを取得する
            for (int i = 0; i < lines.Length-1; i++)
            {
                // Debug.Log(lines[i]);
                data[i] = Convert.ToDouble(lines[i]);
            }

            return data;
        }

        int FindSegment(double[] _x, double x)
        {
            int i = Array.BinarySearch(_x, x);
            if (i < 0)
                i = ~i - 1;
            if (i < 0)
                i = 0;
            else if (i >= _x.Length - 1)
                i = _x.Length - 2;
            return i;
        }
        
        TextAsset zbuffFile = Resources.Load("zbuff") as TextAsset;
        TextAsset depthFile = Resources.Load("depth") as TextAsset;
        double[] zbuff_val = readData(zbuffFile);
        double[] depth_val = readData(depthFile);

        // // スプライン補間のための関数を定義
        // CubicSpline spline = CubicSpline.InterpolateNatural(knot, coef);


        foreach (string filename in filenames)
        {

            TextAsset csvFile = Resources.Load(filename) as TextAsset;

            //保存先指定
            // string path = "C:\\Users\\juranmar7993\\Desktop\\VirtualCapsuleEndoscopy\\VR-Caps-Unity\\Assets\\ScreenShot_EXR";
            //string path = "C:\\Users\\rsagawa41766\\Documents\\src\\20230407_VR-Caps\\test";
            string path = SaveFilePath; // "D:\\torii\\projects\\VRcaps画像列抽出Unity\\data\\demo";


            path = path + "\\" + filename + "_EXR\\";

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


            // Debug.Log(count);//csvの行数カウント

            string line3 = reader3.ReadLine();//１行読み飛ばす
            while (reader3.Peek() != -1)
            {
                line3 = reader3.ReadLine();
                List<string> list3 = new List<string>();//【リストの定義】////////////////////////////////////////////

                list3 = line3.Split(',').ToList();//【分割してリスト化】//////////////////////////////////////////////

                csvDatas3.Add(list3);

            }


            posi3 = new Vector3(float.Parse(csvDatas3[1][0]), float.Parse(csvDatas3[1][1]), float.Parse(csvDatas3[1][2]));


            /////csv1行目を読み込んで初期位置指定/////
            this.transform.position = posi3;
            transform.localRotation = new Quaternion(float.Parse(csvDatas3[1][10]), float.Parse(csvDatas3[1][11]), float.Parse(csvDatas3[1][12]), float.Parse(csvDatas3[1][13]));
            //////////////////////////////////////////



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
                

                //////////////////////////////
                this.transform.position = posi;
                transform.localRotation = new Quaternion(float.Parse(csvDatas[i][10]), float.Parse(csvDatas[i][11]), float.Parse(csvDatas[i][12]), float.Parse(csvDatas[i][13]));
                //////////////////////////////


                //ファイル名を変更する
                // string outputFile = "20230308_colon2_" + counts.ToString("00000");
                // string outputFile = "20230416_calib_depth" + counts.ToString("00000");
                string outputFile = filename + "_" + counts.ToString("00000");



                var mainCamObj = GameObject.FindGameObjectWithTag("Player");
                var cam = mainCamObj.GetComponent<Camera>();
                RenderTexture rTex = cam.targetTexture;

                Texture2D tex = new Texture2D(320, 320, TextureFormat.RGBAFloat, false);///////////////////////////////////////////
                // Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBAFloat, false);///////////////////////////////////////////
                // RenderTexture.active = rTex;

                var rt = new RenderTexture(320, 320, 32);
                cam.targetTexture = rt;
                cam.Render();
                cam.targetTexture = rTex;
                RenderTexture.active = rt;

                // tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
                tex.ReadPixels(new Rect(0, 0, 320, 320), 0, 0);
                tex.Apply();

                // Debug.Log(tex.GetPixel(160,160));
                // Debug.Log(cam.nearClipPlane);
                // Debug.Log(cam.farClipPlane);
                // Debug.Log(cam.projectionMatrix);

                // Matrix4x4 projMatrix = cam.projectionMatrix;
                // float sign = Mathf.Sign(projMatrix[2, 3]);
                // Vector4 zBufferParams = new Vector4(
                //     -sign * cam.nearClipPlane / projMatrix[0, 0],
                //     -sign * cam.nearClipPlane / projMatrix[1, 1],
                //     -sign,
                //     -sign * cam.farClipPlane / (cam.farClipPlane - cam.nearClipPlane)
                // );

                // float zc0, zc1;
                // // OpenGL would be this:
                // // zc0 = (1.0 - m_FarClip / m_NearClip) / 2.0;
                // // zc1 = (1.0 + m_FarClip / m_NearClip) / 2.0;
                // // D3D is this:

                // float mag = 0.1f;
                // float farClipPlane = cam.farClipPlane * mag;
                // float nearClipPlane = cam.nearClipPlane * mag;
                // zc0 = 1.0f - farClipPlane / nearClipPlane;
                // zc1 = farClipPlane / nearClipPlane;
                // // now set _ZBufferParams with (zc0, zc1, zc0/m_FarClip, zc1/m_FarClip);            
                // Vector4 zBufferParams = new Vector4(zc0, zc1, zc0/farClipPlane, zc1/farClipPlane);            

                // Debug.Log(zBufferParams.ToString("F4"));
                // Debug.Log(cam.nearClipPlane);
                // Debug.Log(cam.farClipPlane);

                Color[] pix = tex.GetPixels(0, 0, 320, 320);

                // Debug.Log(tex.GetPixel(160,160).r);

                for (int k = 0; k < pix.Length; k++) {
                    int m = FindSegment(zbuff_val, pix[k].r);
                    double dx = (pix[k].r - zbuff_val[m]) / (zbuff_val[m+1] - zbuff_val[m]);
                    pix[k].r = (float) ((1-dx)*depth_val[m] + dx*depth_val[m+1]);

                    // if(k == 160 + 160*320)
                    // {
                    //     Debug.Log(m);
                    //     Debug.Log(dx);
                    //     Debug.Log(pix[k].r);
                    // }

                    // pix[k] = pix[k] * mag;

                    // 線形化された奥行き値を算出
                    // pix[k].r = 2.0f * nearClipPlane * farClipPlane / (farClipPlane + nearClipPlane - depth * (farClipPlane - nearClipPlane));

                    // pix[k].r = 1.0f / (zBufferParams.z * pix[k].r + zBufferParams.w);
                    // pix[k].r = 1.0f / (zBufferParams.x * pix[k].r + zBufferParams.y);
                }
               tex.SetPixels(pix);

                // Debug.Log(tex.GetPixel(160,160).ToString("F8"));
                // Debug.Log(SystemInfo.usesReversedZBuffer);

                byte[] bytes = tex.EncodeToEXR(Texture2D.EXRFlags.OutputAsFloat);
                GameObject.Destroy(tex);


                //深度画像(.exr)保存
                // File.WriteAllBytes(path + "/EXR" + outputFile + ".exr", bytes);
                File.WriteAllBytes(path + outputFile + ".exr", bytes);



                //float tX = transform.position.x;
                //float tY = transform.position.y;
                //float tZ = transform.position.z;

                //float ltX = transform.localPosition.x;
                //float ltY = transform.localPosition.y;
                //float ltZ = transform.localPosition.z;

                //float rX = this.transform.rotation.x;
                //float rY = this.transform.rotation.y;
                //float rZ = this.transform.rotation.z;
                //float rW = this.transform.rotation.w;

                //Debug.Log("saveEXRの回数  " + counts + " :    " + tX + "  ,  " + tY + "  ,  " + tZ);

                counts++;

                ////Debug.Log(i+ "   経過時間:     " + ts.TotalMilliseconds + "    :NewBehavior読み込み  " + float.Parse(csvDatas[i][0]));
                //Debug.Log("csv読み込み    　" + i + " :    " + float.Parse(csvDatas[i][0]) + "  ,  " + float.Parse(csvDatas[i][1]) + "  ,  " + float.Parse(csvDatas[i][2]));
                //Debug.Log("csv読み込み    　" + i + " :    " + tX + "  ,  " + tY + "  ,  " + tZ + "  ,  " + rX + "  ,  " + rY + "  ,  " + rZ + "  ,  " + rW);

            }

            Debug.Log("End");/////////////////////////////////////////////////////////////////////
        }
    }


    void Start()
    {

        StartCoroutine("ReadCSV");
    }

    public void Cleanup() => CoreUtils.Destroy(m_Material);

}