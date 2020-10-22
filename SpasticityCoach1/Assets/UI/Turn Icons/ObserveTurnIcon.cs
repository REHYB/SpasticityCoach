using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class ObserveTurnIcon : MonoBehaviour
{
    public static bool showIconImage;
    public static bool showCountImage;
    public static int countNumber;

    public Image icon;
    public Image count3;
    public Image count2;
    public Image count1;


    void Start()
    {
        // Set as transparent
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0);
        count3.color = new Color(count3.color.r, count3.color.g, count3.color.b, 0);
        count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 0);
        count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 0);

        // Set show icon as false
        showIconImage = false;
    }

    void Update()
    {
        toggleIconShow(showIconImage);
        toggleCountShow(countNumber, showCountImage);
    }

    // Shows and hides the main icon
    public void toggleIconShow(bool showImage)
    {
        if (showImage == false)
        {
            // Set as transparent
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0);
        }

        if (showImage == true)
        {
            // Set as opaque
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1);
        }
    }

    // Shows and hides the main icon
    public void toggleCountShow(int countNumber, bool showImage)
    {
        if (countNumber == 3)
        {
            if (showImage == false)
            {
                // Set as transparent
                count3.color = new Color(count3.color.r, count3.color.g, count3.color.b, 0);
            }

            if (showImage == true)
            {
                // Set as opaque
                count3.color = new Color(count3.color.r, count3.color.g, count3.color.b, 1);
            }
        }

        if (countNumber == 2)
        {
            if (showImage == false)
            {
                // Set as transparent
                count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 0);
            }

            if (showImage == true)
            {
                // Set as opaque
                count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 1);
            }
        }

        if (countNumber == 1)
        {
            if (showImage == false)
            {
                // Set as transparent
                count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 0);
            }

            if (showImage == true)
            {
                // Set as opaque
                count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 1);
            }
        }

        if (countNumber == 4)
        {
            // Set all as transparent
            count3.color = new Color(count3.color.r, count3.color.g, count3.color.b, 0);
            count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 0);
            count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 0);
        }

    }
}
