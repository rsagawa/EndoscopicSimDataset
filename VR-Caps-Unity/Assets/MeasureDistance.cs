using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// レーザー距離計
public class MeasureDistance : MonoBehaviour
{
    public const float NOTHING = -1;    // 計測不能

    public float maxDistance = 30;      // 計測可能な最大距離
    public float distance;              // 計測距離

    // 距離計測
    void Update() {
        // 前方ベクトル計算
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        // 距離計算
        RaycastHit hit;
        if ( Physics.Raycast(transform.position, fwd, out hit, maxDistance) ) {
            distance = hit.distance;
        } else {
            distance = NOTHING;
        }
    }
}