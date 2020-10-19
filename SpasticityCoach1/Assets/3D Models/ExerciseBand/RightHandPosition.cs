using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEditor;

public class RightHandPosition : MonoBehaviour
{
    public static Vector3 rightHand_pos;

    public void Update()
	{
        rightHand_pos = new Vector3(GetComponent<Transform>().position.x,
            GetComponent<Transform>().position.y,
            GetComponent<Transform>().position.z);

        // UnityEngine.Debug.Log("Right Hand Pos: " + rightHand_pos);
    }
}
