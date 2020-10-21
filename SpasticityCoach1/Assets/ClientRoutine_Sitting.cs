// This script controls the guided instructions with the avatar.

using AwesomeCharts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ClientRoutine_Sitting : MonoBehaviour
{
    public static string instruction1 = "Welcome to PIEGO"; //Personalized Independent Exercise Goals and Occupations
    public static Quaternion elbowMyo = new Quaternion(0, 0, 0, 0);
    public static int routineStage = 0;
    int elbowSpeedCounter = 0;

    Animator anim;

    // Define body joints
    public static Vector3 head_rot;
    public static Vector3 body_rot;
    public static float body_posX;
    public static float body_posY;
    public static float body_posZ;

    public static Vector3 leftElbow_rot;
    public static Vector3 rightShoulder_rot;
    public static Vector3 leftShoulder_rot;
    public static Vector3 rightElbow_rot_routine;

    public static Vector3 leftUpLeg_rot;    
    public static Vector3 rightUpLeg_rot;
    public static Vector3 leftKnee_rot;
    public static Vector3 rightKnee_rot;
    public static Vector3 hips_rot;

    // Define phantom patient joint
    public static float rightElbow_phantom;
    public static float clientElbow_error;


    // Arrays for the joinst in the right hand fingers
    // [0] thumb; [1] Index; [2] Middle; [3] Ring; [4] Little
    public static Vector3[] RProxFingers = new Vector3[5];
    public static Vector3[] RIntFingers = new Vector3[5];
    public static Vector3[] RDistFingers = new Vector3[5];
    
    float secondsNow = 0;
    float secondsChange = 0;
    int makeTransition = Animator.StringToHash("MakeTransition");
    bool reverseMotion = false;
    float elbowSpeed = 1.5f;   // Changed initial elbowSpeed from 0.25f to 1.5f so that it is faster
    float elbowMotion = 120f;

    Color trans_snow = new Color(252 / 255f, 239 / 255f, 239 / 255f, 0.2f);     // Colour for the mesh renderer
    Color trans_maxblue = new Color(71 / 255f, 186 / 255f, 210 / 255f, 0.2f);
    Color trans_fuchsia = new Color(255 / 255f, 0 / 255f, 255 / 255f, 0.2f);

    // ===================== Start is called before the first frame update =====================
    void Start()
    {
        anim = GetComponent<Animator>();

        // Initial location and rotation of entire body
        body_rot = new Vector3(0f, 85f, 0f);
        body_posX = -19f;
        body_posY = -2f;
        body_posZ = -0.4f; //-1.5f;

        GetComponent<Transform>().localPosition = new Vector3(GetComponent<Transform>().localPosition.x + body_posX, 
            GetComponent<Transform>().localPosition.y + body_posY, 
            GetComponent<Transform>().localPosition.z + body_posZ);

        GetComponent<Transform>().localRotation = Quaternion.Euler(body_rot);

        // Initial sitting pose for body model
        head_rot = new Vector3(head_rot.x - 1, 0, 0);
        rightShoulder_rot = new Vector3(0, 0, -58);
        leftShoulder_rot = new Vector3(0, -5, 50);
        rightElbow_rot_routine = new Vector3(0, 0, 0);

        leftUpLeg_rot = new Vector3(180, 100, 0);
        leftKnee_rot = new Vector3(0, -90, 0);
        rightUpLeg_rot = new Vector3(180, 100, 0);
        rightKnee_rot = new Vector3(0, -90, 0);

        // Relax right hand
        for (int i=0; i<5; i++)
        {
            RProxFingers[i] = new Vector3(0, -15, 0);
            RIntFingers[i] = new Vector3(0, -15, 0);
        }

        // Initialise the phantom patient joint, controlling the radial progress bar
        rightElbow_phantom = 0f;
    }

    // ===================== Update is called once per frame =====================
    void Update() {
        
        float move = Input.GetAxis("Vertical");
        anim.SetFloat(makeTransition, move); 

        // Set model colour
        //ModelColour setColour = new ModelColour();
        //setColour.setModelColour(trans_snow);

        // Start of timer
        secondsNow = secondsNow + Time.deltaTime;

        switch (routineStage) {
            // Case 0
            case (0):
                {
                    instruction1 = "Welcome to Piego";
                    if (secondsNow >= 4) {
                       routineStage = 1;
                    }
                    break;
                }

            // Case 1
            case (1):
                {
                    instruction1 = "I'm your Instructor, Milo";
                    // Tilt head towards the camera
                    if (head_rot.x > -40) {
                            head_rot = new Vector3(head_rot.x - 5f, 0, 0);
                    }
                    if (secondsNow >= 6) {
                        routineStage = 2;
                    }
                    break;
                }

            // Case 2
            case (2):
                {
                    instruction1 = "Let's begin your neuro assessment!";
                    if (secondsNow >= 9) {
                        routineStage = 3;
                    }
                    break;
                }

            // Case 3 - Extend arm
            case (3):
                {
                    instruction1 = "First, extend your right arm parallel to the floor";
                    if (head_rot.y < 10) {
                        head_rot = new Vector3(head_rot.x, head_rot.y+2, 0);     // Look down slightly to hand
                    }

                    //setColour.setModelColour(trans_fuchsia);

                    if (rightShoulder_rot.y < 90) {
                        rightShoulder_rot = new Vector3(0, rightShoulder_rot.y + 5, 0);   
                    }

                    if (secondsNow >= 12) {
                        secondsChange = secondsNow;
                        routineStage = 4;
                    }

                    break;
                }

            case (4):
                {
                    instruction1 = "Now, close your fist";
                    // Close fist

                    for (int i = 1; i < 5; i++)
                    {
                        if (RProxFingers[i].y > -90)
                        {
                            RProxFingers[i] = new Vector3(0, RProxFingers[i].y-10, 0);
                            RIntFingers[i] = new Vector3(0, RIntFingers[i].y-10, 0);
                            RDistFingers[i] = new Vector3(0, RDistFingers[i].y-10, 0);
                        }
                    }

                    RProxFingers[0] = new Vector3(0, 0, -20);   // Different values for thumb
                    RIntFingers[0] = new Vector3(0, -30, 0);
                    RDistFingers[0] = new Vector3(0, -50, 0);

                    if ((secondsNow - secondsChange) >= 3)
                    {
                        secondsChange = secondsNow;
                        routineStage = 5;
                    }
                    break;
                }

            // Case 5 - Supinate Wrist
            case (5):
                {
                    instruction1 = "Supinate your wrist";
                    //UnityEngine.Debug.Log("Shoulder X: " + rightShoulder_rot.x);
                    //UnityEngine.Debug.Log("Shoulder Y: " + rightShoulder_rot.y);
                    //UnityEngine.Debug.Log("Shoulder Z: " + rightShoulder_rot.z);

                    //setColour.setModelColour(trans_maxblue);


                    if (rightShoulder_rot.x < 65) {
                        rightShoulder_rot = new Vector3(rightShoulder_rot.x+5, rightShoulder_rot.y, rightShoulder_rot.z);
                    }

                    if (rightElbow_rot_routine.x < 100) {
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x + 10, 0, 0);   
                    }

                    if ((secondsNow - secondsChange) >= 3) {
                        secondsChange = secondsNow;
                        routineStage = 6;
                    }
                    break;
                }

            // Case 6
            case (6):
                {
                    instruction1 = "Now, watch carefully how I perform the task";
                    if ((secondsNow - secondsChange) >= 3)
                    {
                        secondsChange = secondsNow;
                        routineStage = 7;
                    }
                    break;
                }

            // Case 7
            case (7):
                {
                    instruction1 = "When asked, bend your elbow at that same speed";
                    if ((secondsNow - secondsChange) >= 3)
                    {
                        instruction1 = "Watch closely";
                        routineStage = 8;
                    }
                    break;
                }

            // Case 8 - Wait
            case (8):
                {
                    ProgressBar.showBar = true;
                    RadialProgressMarker.showMarker = true;

                    if ((secondsNow - secondsChange) >= 3) {
                        secondsChange = secondsNow;
                        routineStage = 9;
                    }
                    break;
                }

            // Case 9 - Elbow Bend
            case (9):
                {
                    // Move instructor avatar
                    if (rightElbow_rot_routine.y < elbowMotion) {
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, 
                            rightElbow_rot_routine.y + elbowSpeed, 
                            0);}

                    // Move radial diagram
                    clientElbow_error = rightElbow_rot_routine.y - (RadialProgressMarker.clientElbow_rot - 270);
                    ProgressBar.maximum = 360;  // x2 as we only want for the circle to reach 0-180º, not 360º
                    ProgressBar.minimum = 0;
                    ProgressBar.current = 180 - rightElbow_rot_routine.y;

                    RadialRoutineMarker.clientElbow_rot_routine = rightElbow_rot_routine.y - 90;  // Set rotation of pair equal to fill amount


                    // End case when elbow joint reaches target
                    if (rightElbow_rot_routine.y >= elbowMotion) {
                        if ((secondsNow - secondsChange) < 3) {
                            // Wait
                        }

                        else {
                            routineStage = 10;
                            secondsChange = secondsNow;
                        }
                    }
                    break;
                }

            // Case 10 - Wait
            case (10):
                {
                    instruction1 = "Now's your turn, bend your elbow with me";
                    rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, 0, 0);   // Return to original elbow position

                    if ((secondsNow - secondsChange) > 3) {
                        routineStage = 11;
                    }
                    break;
                }

            // Case 11 - Your turn
            case (11):
                {
                    //instruction1 = "Your turn, bend your elbow with me";
                    /*
                    if (rightElbow_rot_routine.y < elbowMotion) {
                            rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, rightElbow_rot_routine.y + elbowSpeed, 0);
                    }
                    */

                    // Move radial diagram
                    clientElbow_error = rightElbow_phantom - (RadialProgressMarker.clientElbow_rot - 270);

                    ProgressBar.maximum = 360;  // x2 as we only want for the circle to reach 0-180º, not 360º
                    ProgressBar.minimum = 0;
                    ProgressBar.current = 180 - rightElbow_phantom;

                    RadialRoutineMarker.clientElbow_rot_routine = rightElbow_phantom - 90;  // Set rotation of pair equal to fill amount


                    // clientElbow_error = 180 - ClientRoutine_Sitting.rightElbow_phantom - (clientElbow_rot - 270);
                    UnityEngine.Debug.Log("Elbow Error: " + clientElbow_error);


                    if (rightElbow_phantom < elbowMotion)
                    {
                        rightElbow_phantom = rightElbow_phantom + elbowSpeed;
                    }

                    if (rightElbow_phantom >= elbowMotion) {
                        rightElbow_phantom = 0f;
                        secondsChange = secondsNow;

                        if (elbowSpeedCounter == 0) {
                            elbowSpeed = 3.0f;
                            elbowSpeedCounter = 1;
                            instruction1 = "Watch how I bend my elbow faster";
                            routineStage = 8;
                        }

                        else if (elbowSpeedCounter == 1) {
                            elbowSpeed = 5.0f;
                            elbowSpeedCounter = 2;
                            instruction1 = "Watch how I bend my elbow even faster";
                            routineStage = 8;
                        }

                        else if (elbowSpeedCounter == 2) {
                             routineStage = 12;
                        }
                    }
                    break;
                }

            case (12):
                    {
                        instruction1 = "Saving patient data, please wait...";
                        if ((secondsNow - secondsChange) > 4)
                        {
                                routineStage = 12;
                        }
                        break;
                    }

            case (13):
                    {
                        SaveRoutine save = new SaveRoutine();
                        save.emgCSVsave();  // Call function to save the raw and processed EMG CSVs

                        routineStage = 14;
                        break;
                    }

            // Save the processed EMG data in a CSV file at the end of the routine
            case (14):
                    {
                        instruction1 = "Assessment complete. Well Done!";
                        routineStage = 13;
                        break;
                    }
        }
    }

    void OnAnimatorIK(int layerIndex) {
        //print("OnAnimatorIK - running");
        anim.SetBoneLocalRotation(HumanBodyBones.Head, Quaternion.Euler(head_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(rightElbow_rot_routine));
        anim.SetBoneLocalRotation(HumanBodyBones.RightUpperArm, Quaternion.Euler(rightShoulder_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.LeftUpperArm, Quaternion.Euler(leftShoulder_rot));

        anim.SetBoneLocalRotation(HumanBodyBones.LeftUpperLeg, Quaternion.Euler(leftUpLeg_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightUpperLeg, Quaternion.Euler(rightUpLeg_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerLeg, Quaternion.Euler(leftKnee_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLowerLeg, Quaternion.Euler(rightKnee_rot));

        // Right fingers
        anim.SetBoneLocalRotation(HumanBodyBones.RightThumbProximal, Quaternion.Euler(RProxFingers[0]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightThumbIntermediate, Quaternion.Euler(RIntFingers[0]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightThumbDistal, Quaternion.Euler(RDistFingers[0]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightIndexProximal, Quaternion.Euler(RProxFingers[1]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightIndexIntermediate, Quaternion.Euler(RIntFingers[1]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightIndexDistal, Quaternion.Euler(RDistFingers[1]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightMiddleProximal, Quaternion.Euler(RProxFingers[2]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightMiddleIntermediate, Quaternion.Euler(RIntFingers[2]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightMiddleDistal, Quaternion.Euler(RDistFingers[2]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightRingProximal, Quaternion.Euler(RProxFingers[3]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightRingIntermediate, Quaternion.Euler(RIntFingers[3]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightRingDistal, Quaternion.Euler(RDistFingers[3]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLittleProximal, Quaternion.Euler(RProxFingers[4]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLittleIntermediate, Quaternion.Euler(RIntFingers[4]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLittleDistal, Quaternion.Euler(RDistFingers[4]));
    }
}
