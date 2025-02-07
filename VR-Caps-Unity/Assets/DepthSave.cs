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
// using MathNet.Numerics.Interpolation;

[Serializable, VolumeComponentMenu("Post-processing/Custom/DepthShader")]

public class DepthSave : MonoBehaviour, IPostProcessComponent
{
#if UNITY_EDITOR
    [SerializeField] private string Save_Depth_DIR = "";
    [SerializeField] private string Load_Camera_Path = "";
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

    TextAsset load_csv(string input_path)
    {
        string fileContent = File.ReadAllText(input_path);
        TextAsset csvFile = new TextAsset(fileContent);

        UnityEngine.Debug.Log("Loaded original camera path");

        return csvFile;
    }

    List<List<string>> load_csv_original(string input_csv_path)
    {
        string fileContent = File.ReadAllText(input_csv_path);
        TextAsset csv_str = new TextAsset(fileContent);

        StringReader reader = new StringReader(csv_str.text);

        List<List<string>> poses = new List<List<string>>();

        bool isFirstLine = true;

        while (reader.Peek() != -1)
        {
            if (isFirstLine)
            {
                isFirstLine = false;
                reader.ReadLine();    //skip first line.
                continue;
            }

            string line = reader.ReadLine();
            List<string> list = new List<string>();
            list = line.Split(',').ToList();

            poses.Add(list);

        }
        reader.Close();

        UnityEngine.Debug.Log("Loaded original camera path");
        UnityEngine.Debug.Log("Loaded path len: " + poses.Count);

        return poses;
    }

    List<List<string>> load_csv_default(string filename)
    {

        TextAsset csv_str = Resources.Load(filename) as TextAsset;

        StringReader reader = new StringReader(csv_str.text);

        List<List<string>> poses = new List<List<string>>();

        bool isFirstLine = true;

        while (reader.Peek() != -1)
        {
            if (isFirstLine)
            {
                isFirstLine = false;
                reader.ReadLine();    //skip first line.
                continue;
            }

            string line = reader.ReadLine();
            List<string> list = new List<string>();
            list = line.Split(',').ToList();

            poses.Add(list);

        }
        reader.Close();

        UnityEngine.Debug.Log("Loaded default camera path");
        UnityEngine.Debug.Log("Loaded path len: " + poses.Count);

        return poses;
    }

    (double[], double[]) load_depth_info()
    {

        double[] readData(TextAsset filename)
        {
            StringReader datreader = new StringReader(filename.text);
            // ファイルの各行を読み込んで、配列に格納する
            string[] lines = datreader.ReadToEnd().Split('\n');

            // 出力データを格納するための配列を作成する
            double[] data = new double[lines.Length];

            // 各行を処理して、出力データを取得する
            for (int i = 0; i < lines.Length - 1; i++)
            {
                // Debug.Log(lines[i]);
                data[i] = Convert.ToDouble(lines[i]);
            }
            return data;
        }

        TextAsset zbuffFile = Resources.Load("zbuff") as TextAsset;
        TextAsset depthFile = Resources.Load("depth") as TextAsset;
        double[] zbuff_val = readData(zbuffFile);
        double[] depth_val = readData(depthFile);

        return (zbuff_val, depth_val);
    }

    void save_depth(string path, byte[] bytes)
    {
        File.WriteAllBytes(path, bytes);
        UnityEngine.Debug.Log("Saved: " + path);
    }

    IEnumerator capture(List<List<string>> poses, string save_dir, string filename)
    {
        UnityEngine.Debug.Log("Capture Start.");

        (double[] zbuff_val, double[] depth_val) = load_depth_info();

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

        for (int i = 0; i < poses.Count; i++)
        {
            yield return null;

            Vector3 pos = new Vector3(float.Parse(poses[i][0]), float.Parse(poses[i][1]), float.Parse(poses[i][2]));
            Quaternion rot = new Quaternion(float.Parse(poses[i][10]), float.Parse(poses[i][11]), float.Parse(poses[i][12]), float.Parse(poses[i][13]));

            this.transform.position = pos;
            transform.localRotation = rot;

            UnityEngine.Debug.Log(poses[i][0] + " " + poses[i][1] + " " + poses[i][2]);

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

            for (int k = 0; k < pix.Length; k++)
            {
                int m = FindSegment(zbuff_val, pix[k].r);
                double dx = (pix[k].r - zbuff_val[m]) / (zbuff_val[m + 1] - zbuff_val[m]);
                pix[k].r = (float)((1 - dx) * depth_val[m] + dx * depth_val[m + 1]);

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


            string outputFile = filename + "_" + i.ToString("D5");
            string save_depth_path = save_dir + "/" + outputFile + ".exr";

            save_depth(save_depth_path, bytes);
        }

        UnityEngine.Debug.Log("Capture End.");
    }

    IEnumerator original_unit_proc(string input_csv_path, string save_depth_dir)
    {
        List<List<string>> poses = load_csv_original(input_csv_path);

        //var wait = new WaitForSeconds((float)0.1);

        string filename = Path.GetFileNameWithoutExtension(input_csv_path);

        string save_dir = save_depth_dir + "/" + filename + "_EXR" + "/";

        Directory.CreateDirectory(save_dir);

        yield return capture(poses, save_dir, filename);
    }

    IEnumerator default_unit_proc(string filename, string save_depth_dir)
    {
        List<List<string>> poses = load_csv_default(filename);

        string save_dir = save_depth_dir + "/" + filename + "_EXR" + "/";

        Directory.CreateDirectory(save_dir);

        //var wait = new WaitForSeconds((float)0.1);

        yield return capture(poses, save_dir, filename);
    }

    IEnumerator original_process(string input_csv_path, string save_depth_dir)
    {
        UnityEngine.Debug.Log("process start: " + input_csv_path);
        yield return original_unit_proc(input_csv_path, save_depth_dir);
        UnityEngine.Debug.Log("process end: " + input_csv_path);
    }

    IEnumerator default_process(string save_depth_dir)
    {
        foreach (string filename in filenames)
        {
            UnityEngine.Debug.Log("process start: " + filename);
            yield return default_unit_proc(filename, save_depth_dir);
            UnityEngine.Debug.Log("process end: " + filename);
        }
    }

    IEnumerator Record()
    {
        if (Directory.Exists(Save_Depth_DIR))
        {
            UnityEngine.Debug.Log("Detect Save Directory.");
        }
        else
        {
            UnityEngine.Debug.Log("Please Set Save Directory.");
            yield break;
        }

        if (!string.IsNullOrEmpty(Load_Camera_Path))
        {
            if (File.Exists(Load_Camera_Path))
            {
                yield return original_process(Load_Camera_Path, Save_Depth_DIR);
            }
            else
            {
                UnityEngine.Debug.Log("Faild load camera-path-file. Please Set corrected path.");
                yield break;
            }
        }
        else
        {
            yield return default_process(Save_Depth_DIR);
        }
    }

    void Start()
    {

        StartCoroutine(Record());
    }

    public void Cleanup() => CoreUtils.Destroy(m_Material);
}