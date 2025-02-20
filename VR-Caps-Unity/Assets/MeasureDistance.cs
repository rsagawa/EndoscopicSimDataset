using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Laser Distance Meter
public class MeasureDistance : MonoBehaviour
{
    public const float NOTHING = -1;    // Unable to Measure
    public float maxDistance = 30;      // Maximum Measurable Distance
    public float distance;              // Measurement Distance

    // Distance Measurement
    void Update() {
        // Forward Vector Calculation
        Vector3 fwd = transform.TransformDirection(Vector3.forward);

        // Distance Calculation
        RaycastHit hit;
        if ( Physics.Raycast(transform.position, fwd, out hit, maxDistance) ) {
            distance = hit.distance;
        } else {
            distance = NOTHING;
        }
    }
}