﻿// This script controls the guided instructions with the avatar.

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

    public static Vector3 RIndexProx_rot;   // Right index finger
    public static Vector3 RIndexInt_rot;
    public static Vector3 RIndexDist_rot;
    public static Vector3 RRingProx_rot;    // Right ring finger
    public static Vector3 RRingInt_rot;
    public static Vector3 RRingDist_rot;
    public static Vector3 RMiddleProx_rot;  // Right middle finger
    public static Vector3 RMiddleInt_rot;
    public static Vector3 RMiddleDist_rot;
    public static Vector3 RLittleProx_rot;  // Right little finger
    public static Vector3 RLittleInt_rot;
    public static Vector3 RLittleDist_rot;
    public static Vector3 RThumbProx_rot;   // Right thumb
    public static Vector3 RThumbInt_rot;
    public static Vector3 RThumbDist_rot;

    float secondsNow = 0;
    float secondsChange = 0;
    int makeTransition = Animator.StringToHash("MakeTransition");
    bool reverseMotion = false;
    float elbowSpeed = 1.5f;   // Changed initial elbowSpeed from 0.25f to 1.5f so that it is faster
    int elbowSpeedCounter = 0;
    float elbowMotion = 120f;

    Color trans_snow = new Color(252 / 255f, 239 / 255f, 239 / 255f, 0.2f);     // Colour for the mesh renderer
    Color trans_maxblue = new Color(71 / 255f, 186 / 255f, 210 / 255f, 0.2f);
    Color trans_fuchsia = new Color(255 / 255f, 0 / 255f, 255 / 255f, 0.2f);

    // ===================== Start is called before the first frame update =====================
    void Start()
    {
        anim = GetComponent<Animator>();

        //lying in Default Humanoid pose on Ground
        body_rot = new Vector3(0f, 85f, 0f);
        body_posX = -19f;
        body_posY = -2f;
        body_posZ = -0.4f; //-1.5f;

        // Initial location and rotation of entire body
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

        RIndexProx_rot = new Vector3(0, -15, 0);  // Relax right hand
        RIndexInt_rot = new Vector3(0, -15, 0);
        RMiddleProx_rot = new Vector3(0, -15, 0);
        RMiddleInt_rot = new Vector3(0, -15, 0);
        RRingProx_rot = new Vector3(0, -15, 0);
        RRingInt_rot = new Vector3(0, -15, 0);
        RLittleProx_rot = new Vector3(0, -15, 0);
        RLittleInt_rot = new Vector3(0, -15, 0);
        RThumbProx_rot = new Vector3(0, -5, 0);
        RThumbInt_rot = new Vector3(0, -15, 0);
        RThumbDist_rot = new Vector3(0, -15, 0);
    }

    // ===================== Update is called once per frame =====================
    void Update() {
        
        float move = Input.GetAxis("Vertical");
        anim.SetFloat(makeTransition, move); 

        // Set model colour
        ModelColour setColour = new ModelColour();
        setColour.setModelColour(trans_snow);

        // Start of timer
        secondsNow = secondsNow + Time.deltaTime;

        switch (routineStage) {
            // Case 0
            case (0): {
                instruction1 = "Welcome to Piego";
                if (secondsNow >= 2) {
                   routineStage = 1;
                }
                break;}

            // Case 1
            case (1): {
                instruction1 = "I'm your Instructor, Milo";
                // Tilt head towards the camera
                if (head_rot.x > -40) {
                        head_rot = new Vector3(head_rot.x - 5f, 0, 0);
                }
                if (secondsNow >= 4.5f) {
                    routineStage = 2;
                }
                break;}

            // Case 2
            case (2): {
                instruction1 = "Let's begin your neuro assessment!";
                if (secondsNow >= 7) {
                    routineStage = 3;
                }
                break;}

            // Case 3 - Extend arm
            case (3): {
                instruction1 = "First, extend your right arm parallel to the floor";
                if (head_rot.y < 10) {
                    head_rot = new Vector3(head_rot.x, head_rot.y+2, 0);     // Look down slightly to hand
                }

                setColour.setModelColour(trans_fuchsia);

                if (rightShoulder_rot.y < 90) {
                    rightShoulder_rot = new Vector3(0, rightShoulder_rot.y + 5, 0);   
                }

                if (secondsNow >= 9) {
                    secondsChange = secondsNow;
                    routineStage = 4;
                }

                break;}

            // Case 4 - Supinate Wrist
            case (4): {
                instruction1 = "Supinate your wrist";

                UnityEngine.Debug.Log("Shoulder X: " + rightShoulder_rot.x);
                UnityEngine.Debug.Log("Shoulder Y: " + rightShoulder_rot.y);
                UnityEngine.Debug.Log("Shoulder Z: " + rightShoulder_rot.z);

                setColour.setModelColour(trans_maxblue);


                if (rightShoulder_rot.x < 65) {
                    rightShoulder_rot = new Vector3(rightShoulder_rot.x+5, rightShoulder_rot.y, rightShoulder_rot.z);
                }

                if (rightElbow_rot_routine.x < 100) {
                    rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x + 10, 0, 0);   
                }

                if ((secondsNow - secondsChange) >= 3) {
                    secondsChange = secondsNow;
                    routineStage = 5;
                }
                break;}

            // Case 5
            case (5):
                {
                    instruction1 = "Now, watch carefully how I perform the task";
                    if ((secondsNow - secondsChange) >= 3)
                    {
                        secondsChange = secondsNow;
                        routineStage = 6;
                    }
                    break;
                }

            // Case 2
            case (6):
                {
                    instruction1 = "When asked, bend your elbow at that same speed";
                    if ((secondsNow - secondsChange) >= 3)
                    {
                        instruction1 = "Watch closely";
                        routineStage = 7;
                    }
                    break;
                }

            // Case 5 - Wait
            case (7): {
                if ((secondsNow - secondsChange) >= 3) {
                    secondsChange = secondsNow;
                    routineStage = 8;
                }
                break;}

            // Case 6 - Elbow Bend
            case (8): {
                if (rightElbow_rot_routine.y < elbowMotion) {
                    rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, 
                        rightElbow_rot_routine.y + elbowSpeed, 
                        0);}

                if (rightElbow_rot_routine.y >= elbowMotion) {
                    if ((secondsNow - secondsChange) < 3) {
                        // Wait
                    }

                    else {
                        routineStage = 9;
                        secondsChange = secondsNow;
                    }
                }
                break;}

            // Case 7 - Wait
            case (9): {
                instruction1 = "Now's your turn, bend your elbow with me";
                rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, 0, 0);   // Return to original elbow position

                if ((secondsNow - secondsChange) > 3) {
                    routineStage = 10;
                }
                break;}

            // Case 8 - Your turn
            case (10): {
                //instruction1 = "Your turn, bend your elbow with me";
                if (rightElbow_rot_routine.y < elbowMotion) {
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, rightElbow_rot_routine.y + elbowSpeed, 0);
                }

                if (rightElbow_rot_routine.y >= elbowMotion) {
                    rightElbow_rot_routine.y = 0;
                    secondsChange = secondsNow;

                    if (elbowSpeedCounter == 0) {
                        elbowSpeed = 3.0f;
                        elbowSpeedCounter = 1;
                        instruction1 = "Watch how I bend my elbow faster";
                        routineStage = 7;
                    }

                    else if (elbowSpeedCounter == 1) {
                        elbowSpeed = 5.0f;
                        elbowSpeedCounter = 2;
                        instruction1 = "Watch how I bend my elbow even faster";
                        routineStage = 7;
                    }

                    else if (elbowSpeedCounter == 2) {
                         routineStage = 11;
                    }
                }
                break;}

            case (11):
                {
                    instruction1 = "Saving patient data, please wait...";
                    if ((secondsNow - secondsChange) > 4)
                    {
                            routineStage = 12;
                    }
                    break;
                }

            case (12):
                {
                    SaveRoutine save = new SaveRoutine();
                    save.emgCSVsave();  // Call function to save the raw and processed EMG CSVs

                    routineStage = 13;
                    break;
                }

            // Save the processed EMG data in a CSV file at the end of the routine
            case (13):
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
        anim.SetBoneLocalRotation(HumanBodyBones.RightIndexProximal, Quaternion.Euler(RIndexProx_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightIndexIntermediate, Quaternion.Euler(RIndexInt_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightIndexDistal, Quaternion.Euler(RIndexDist_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightMiddleProximal, Quaternion.Euler(RMiddleProx_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightMiddleIntermediate, Quaternion.Euler(RMiddleInt_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightMiddleDistal, Quaternion.Euler(RMiddleDist_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightRingProximal, Quaternion.Euler(RRingProx_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightRingIntermediate, Quaternion.Euler(RRingInt_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightRingDistal, Quaternion.Euler(RRingDist_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLittleProximal, Quaternion.Euler(RLittleProx_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLittleIntermediate, Quaternion.Euler(RLittleInt_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLittleDistal, Quaternion.Euler(RLittleDist_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightThumbProximal, Quaternion.Euler(RThumbProx_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightThumbIntermediate, Quaternion.Euler(RThumbInt_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightThumbDistal, Quaternion.Euler(RThumbDist_rot));

    }
}
