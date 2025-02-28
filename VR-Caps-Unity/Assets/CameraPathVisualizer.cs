using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CameraPathVisualizer : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private string Load_Camera_Pose_Path = "";
#endif

    public class Waypoint
    {
        public Vector3 pos;
        public Quaternion rot;
    }

    public Waypoint[] waypoints;

    private LineRenderer lineRenderer;

    List<List<string>> load_csv(string input_csv_path)
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

        UnityEngine.Debug.Log("Loaded path len: " + poses.Count);

        return poses;
    }

    public Waypoint[] convert_waypoints(List<List<string>> poses)
    {
        waypoints = new Waypoint[poses.Count];

        for (int i = 0; i < poses.Count; i++)
        {
            Vector3 pos = new Vector3(float.Parse(poses[i][0]), float.Parse(poses[i][1]), float.Parse(poses[i][2]));
            Quaternion rot = new Quaternion(float.Parse(poses[i][10]), float.Parse(poses[i][11]), float.Parse(poses[i][12]), float.Parse(poses[i][13]));

            waypoints[i] = new Waypoint();
            waypoints[i].pos = pos;
            waypoints[i].rot = rot;
        }

        return waypoints;
    }

    public Waypoint[] load_waypoints(string input_csv_path)
    {
        List<List<string>> poses = load_csv(input_csv_path);

        Waypoint[] waypoints = convert_waypoints(poses);

        return waypoints;
    }

    private void Start()
    {
        
        if (!string.IsNullOrEmpty(Load_Camera_Pose_Path))
        {
            if (File.Exists(Load_Camera_Pose_Path))
            {
                Waypoint[] waypoints = load_waypoints(Load_Camera_Pose_Path);
                UnityEngine.Debug.Log("load done.(CameraPathVisualizer.cs)");
            }
            else
            {
                UnityEngine.Debug.Log("Faild load camera-path-file. Please Set corrected path.(CameraPathVisualizer.cs)");
                return;
            }
        }
        else
        {
            UnityEngine.Debug.Log("Not found camera-path-file.(CameraPathVisualizer.cs)");
            return;
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = waypoints.Length;

        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.useWorldSpace = true;

        for (int i = 0; i < waypoints.Length; i++)
        {
            lineRenderer.SetPosition(i, waypoints[i].pos);
            //UnityEngine.Debug.Log($"waypoints[i].pos: {i}, {waypoints[i].pos}");
        }
    }
}