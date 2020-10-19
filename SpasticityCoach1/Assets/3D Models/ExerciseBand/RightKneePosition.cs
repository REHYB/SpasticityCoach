using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEditor;

public class RightKneePosition : MonoBehaviour
{
    public static Vector3 rightKnee_pos;

    public void Update()
    {
        rightKnee_pos = new Vector3(GetComponent<Transform>().position.x,
            GetComponent<Transform>().position.y,
            GetComponent<Transform>().position.z);

        // UnityEngine.Debug.Log("Right Knee Pos: " + rightKnee_pos);
    }
}
