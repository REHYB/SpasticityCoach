using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class RadialRoutineMarker : MonoBehaviour
{
    public static Image marker;
    public static bool showMarker;
    public static float clientElbow_rot_routine;

    Color solid_Snow = new Color(252 / 255f, 239 / 255f, 239 / 255f, 1);     // Colour for the mesh renderer
    Color solid_Maxblue = new Color(71 / 255f, 186 / 255f, 210 / 255f, 1);
    Color solid_RedSalsa = new Color(249 / 255f, 65 / 255f, 68 / 255f, 1);
    Color solid_CrayolaMaize = new Color(249 / 255f, 199 / 255f, 79 / 255f, 1);
    Color solid_Zomp = new Color(67 / 255f, 170 / 255f, 139 / 255f, 1);

    void Start()
    {
        // Set as transparent
        marker = GetComponent<Image>();
        marker.color = new Color(marker.color.r, marker.color.g, marker.color.b, 0f);

        // Set show marker as false
        showMarker = false;
        clientElbow_rot_routine = -90;
    }

    // Update is called once per frame
    void Update()
    {
        if (showMarker == false)
        {
            // Set as transparent
            marker.color = new Color(marker.color.r, marker.color.g, marker.color.b, 0f);
        }

        if (showMarker == true)
        {
            // Set as opaque
            marker = GetComponent<Image>();
            marker.color = new Color(marker.color.r, marker.color.g, marker.color.b, 1f);

            GetComponent<Transform>().localRotation = Quaternion.Euler(0, 0, clientElbow_rot_routine);
            
        }
    }
}
