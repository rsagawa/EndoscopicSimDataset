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
    [SerializeField] private string Save_Folder_Path = "";
    [SerializeField] private string Load_Camera_Pose_Path = "";
    [SerializeField] private bool Pattern = false;
#endif

    public ClampedFloatParameter depthDistance = new ClampedFloatParameter(1f, 0f, 32f);

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
        GameObject parentObject = this.gameObject;

        if (parentObject != null)
        {
            Transform TurnOnObjectTransform = parentObject.transform.Find("SpotLight");
            Transform TurnOffObjectTransform = parentObject.transform.Find("SpotLightpattern");

            if (TurnOnObjectTransform != null && TurnOffObjectTransform != null)
            {
                GameObject TurnOnObject = TurnOnObjectTransform.gameObject;
                GameObject TurnOffObject = TurnOffObjectTransform.gameObject;

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

    void save_rgb(string path)
    {
        ScreenCapture.CaptureScreenshot(path);
        UnityEngine.Debug.Log("Saved: " + path);
    }

    IEnumerator capture(List<List<string>> poses, string save_dir, string filename)
    {
        for (int i = 0; i < poses.Count; i++)
        {
            yield return null;

            Vector3 pos = new Vector3(float.Parse(poses[i][0]), float.Parse(poses[i][1]), float.Parse(poses[i][2]));
            Quaternion rot = new Quaternion(float.Parse(poses[i][10]), float.Parse(poses[i][11]), float.Parse(poses[i][12]), float.Parse(poses[i][13]));

            this.transform.position = pos;
            transform.localRotation = rot;

            string outputFile = filename + "_" + i.ToString("D5");
            string save_rgb_path = save_dir + "/" + outputFile + ".png";

            save_rgb(save_rgb_path);
        }
    }

    IEnumerator original_unit_proc(string input_csv_path, string Save_Folder_Path)
    {
        List<List<string>> poses = load_csv_original(input_csv_path);

        //var wait = new WaitForSeconds((float)0.1);

        string filename = Path.GetFileNameWithoutExtension(input_csv_path);

        string save_dir = Save_Folder_Path + "/" + filename + "_RGB" + "/";

        Directory.CreateDirectory(save_dir);

        yield return capture(poses, save_dir, filename);
    }

    IEnumerator default_unit_proc(string filename, string Save_Folder_Path)
    {
        List<List<string>> poses = load_csv_default(filename);

        string save_dir = Save_Folder_Path + "/" + filename + "_RGB" + "/";

        Directory.CreateDirectory(save_dir);

        //var wait = new WaitForSeconds((float)0.1);

        yield return capture(poses, save_dir, filename);
    }

    IEnumerator original_process(string input_csv_path, string Save_Folder_Path)
    {
        yield return original_unit_proc(input_csv_path, Save_Folder_Path);
        UnityEngine.Debug.Log("done.");
    }

    IEnumerator default_process(string Save_Folder_Path)
    {
        foreach (string filename in filenames)
        {
            yield return default_unit_proc(filename, Save_Folder_Path);
            UnityEngine.Debug.Log("done.");
        }
    }

    IEnumerator Record()
    {
        if (!Directory.Exists(Save_Folder_Path))
        {
            UnityEngine.Debug.Log("Please Set Corrected Save Directory.");
            yield break;
        }

        if (!string.IsNullOrEmpty(Load_Camera_Pose_Path))
        {
            if (File.Exists(Load_Camera_Pose_Path))
            {
                yield return original_process(Load_Camera_Pose_Path, Save_Folder_Path);
            }
            else
            {
                UnityEngine.Debug.Log("Faild load camera-path-file. Please Set corrected path.");
                yield break;
            }
        }
        else
        {
            yield return default_process(Save_Folder_Path);
        }
    }

    void Start()
    {
        SwitchSpotLight();

        StartCoroutine(Record());
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

        if (!EditorApplication.isPlaying)
        {
            setter.SwitchSpotLight();
        }
    }
}
#endif