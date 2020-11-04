// This script controls the guided instructions with the avatar.

using AwesomeCharts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ClientRoutine_KIRA : MonoBehaviour
{
    public static string instruction1 = "Welcome to PIEGO"; //Personalized Independent Exercise Goals and Occupations
    public static Quaternion elbowMyo = new Quaternion(0, 0, 0, 0);
    public static int routineStage = 0;
    int elbowSpeedCounter = 0;
    int practiceRoundsCounter = 0;

    Animator anim;

    // Define body joints
    public static Vector3 head_rot;
    public static Vector3 body_rot;
    public static Vector3 body_pos;

    public static Vector3 leftElbow_rot;
    public static Vector3 leftShoulder_rot;
    public static Vector3 rightElbow_rot_routine;
    public static Vector3 rightShoulder_rot;

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
    float elbowMotion = 135f;

    Color trans_snow = new Color(252 / 255f, 239 / 255f, 239 / 255f, 0.2f);     // Colour for the mesh renderer
    Color trans_maxblue = new Color(71 / 255f, 186 / 255f, 210 / 255f, 0.2f);
    Color trans_fuchsia = new Color(255 / 255f, 0 / 255f, 255 / 255f, 0.2f);

    // ===================== Start is called before the first frame update =====================
    void Start()
    {
        anim = GetComponent<Animator>();

        // Initial location and rotation of entire body
        body_rot = new Vector3(0, 90, 0);
        body_pos = new Vector3(0, 0, 0);
        UnityEngine.Debug.Log("Hello my fiend");

        GetComponent<Transform>().localPosition = new Vector3(GetComponent<Transform>().localPosition.x + body_pos.x,
            GetComponent<Transform>().localPosition.y + body_pos.y,
            GetComponent<Transform>().localPosition.z + body_pos.z);

        GetComponent<Transform>().localRotation = Quaternion.Euler(GetComponent<Transform>().localRotation.x + body_rot.x,
            GetComponent<Transform>().localRotation.y + body_rot.y,
            GetComponent<Transform>().localRotation.z + body_rot.z);

        // Initial sitting pose for body model
        head_rot = new Vector3(0, 0, 0);

        rightShoulder_rot = new Vector3(85, 0, 0);
        leftShoulder_rot = new Vector3(85, 0, 0);
        rightElbow_rot_routine = new Vector3(15, 40, 45);
        leftElbow_rot = new Vector3(15, -40, -45);
        //leftElbow_rot = new Vector3(-90, -40+90, -45);

        leftUpLeg_rot = new Vector3(leftUpLeg_rot.x+90, leftUpLeg_rot.y, leftUpLeg_rot.z+180);
        leftKnee_rot = new Vector3(leftKnee_rot.x + 100, leftKnee_rot.y, leftKnee_rot.z);
        rightUpLeg_rot = new Vector3(rightUpLeg_rot.x+90, rightUpLeg_rot.y, rightUpLeg_rot.z+180);
        rightKnee_rot = new Vector3(rightKnee_rot.x + 100, rightKnee_rot.y, rightKnee_rot.z);


        // Relax right hand
        for (int i = 0; i < 5; i++)
        {
            RProxFingers[i] = new Vector3(0, -15, 0);
            RIntFingers[i] = new Vector3(0, -15, 0);
        }

        // Initialise the phantom patient joint, controlling the radial progress bar
        rightElbow_phantom = 0f;

    }

    // ===================== Update is called once per frame =====================
    void Update()
    {

        float move = Input.GetAxis("Vertical");
        anim.SetFloat(makeTransition, move);

        // Set model colour
        //ModelColour setColour = new ModelColour();
        //setColour.setModelColour(trans_snow);

        // Start of timer
        secondsNow = secondsNow + Time.deltaTime;

        switch (routineStage)
        {
            // Case 0
            case (0):
                {
                    instruction1 = "Welcome to Piego";
                    if (secondsNow >= 5)
                    {
                        routineStage = routineStage + 1;
                    }
                    break;
                }

            // Case 1
            case (1):
                {
                    instruction1 = "I'm your Instructor, Milo";
                    // Tilt head towards the camera
                    if (head_rot.y < 80)
                    {
                        head_rot = new Vector3(0, head_rot.y + 2, 0);
                    }

                    if (secondsNow >= 6)
                    {
                        routineStage = routineStage + 1;
                    }
                    break;
                }

            // Case 2
            case (2):
                {
                    instruction1 = "Let's begin your neuro assessment!";
                    
                    if (secondsNow >= 9)
                    {
                        routineStage = routineStage + 1;
                    }
                    break;
                }

            // Case 3 - Extend arm
            case (3):
                {
                    instruction1 = "First, let your arms drop to the sides";

                    // Tilt head down to the hand
                    if (head_rot.x > -20)
                    {
                        head_rot = new Vector3(head_rot.x - 5f, head_rot.y - 2, 0);
                    }

                    // Arm to the sides
                    // Define transition steps for next animation
                    int steps_trans = 15;

                    if (rightElbow_rot_routine.x > 0)
                    {
                        rightShoulder_rot = new Vector3(rightShoulder_rot.x - (15/ steps_trans), 0, 0);
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x - (15/steps_trans),
                            rightElbow_rot_routine.y - (40 / steps_trans),
                            rightElbow_rot_routine.z - (45 / steps_trans));
                    }

                    /*
                    if (leftElbow_rot.x > 0)
                    {
                        leftShoulder_rot = new Vector3(leftShoulder_rot.x - (15 / steps_trans), 0, 0);
                        leftElbow_rot = new Vector3(leftElbow_rot.x - (15 / steps_trans),
                            leftElbow_rot.y - (-40 / steps_trans),
                            leftElbow_rot.z - (-45 / steps_trans));
                    }
                    */
                    leftShoulder_rot = new Vector3(70, 0, 0);
                    leftElbow_rot = new Vector3(15,0,0);

                    //Next step
                    if (secondsNow >= 12)
                    {
                        secondsChange = secondsNow;
                        routineStage = routineStage + 1;
                    }

                    break;
                }
                

            // Case 5 - Supinate Wrist
            case (4):
                {
                    instruction1 = "Rotate your palm to face forward and close your fist";                    

                    // Supinate
                    
                    if (rightElbow_rot_routine.y > -90)
                    {
                        rightElbow_rot_routine = new Vector3(0, rightElbow_rot_routine.y - 10, 0);

                    }

                    if ((secondsNow - secondsChange) >= 1.5f)
                    {
                        secondsChange = secondsNow;
                        routineStage = routineStage + 1;
                    }
                    break;
                }

            // Case 5 - Close fist
            case (5):
                {
                    //instruction1 = "Now, close your fist";

                    // Close fist
                    for (int i = 1; i < 5; i++)
                    {
                        if (RProxFingers[i].x < 90)
                        {
                            RProxFingers[i] = new Vector3(RProxFingers[i].x + 5, 0, 0);
                            RIntFingers[i] = new Vector3(RProxFingers[i].x + 5, 0, 0);
                            RDistFingers[i] = new Vector3(RProxFingers[i].x + 5, 0, 0);
                        }
                    }

                    //RProxFingers[0] = new Vector3(0, 0, 20);   // Different values for thumb
                    //RIntFingers[0] = new Vector3(0, 30, 0);
                    //RDistFingers[0] = new Vector3(0, 50, 0);


                    if ((secondsNow - secondsChange) >= 3)
                    {
                        secondsChange = secondsNow;
                        routineStage = routineStage + 1;
                    }
                    break;
                }

            // Case 6
            case (6):
                {
                    instruction1 = "In the first 2 rounds, we will practice together";

                    // Turn observe icon ON
                    ObserveTurnIcon.showIconImage = true;

                    if ((secondsNow - secondsChange) >= 4)
                    {
                        secondsChange = secondsNow;
                        routineStage = routineStage + 1;
                    }
                    break;
                }

            // Case 7
            case (7):
                {
                    instruction1 = "In the third round, you should perform the task as accurately as possible";

                    // Turn your-turn icon ON and observe icon OFF
                    YourTurnIcon.showIconImage = true;
                    ObserveTurnIcon.showIconImage = false;

                    if ((secondsNow - secondsChange) >= 7)
                    {
                        instruction1 = "The task is to bend your elbow to catch the pear";
                        ProgressBar.showBar = true;
                        RadialProgressMarker.showMarker = true;
                        RadialRoutineMarker.showMarker = true;
                        YourTurnIcon.showIconImage = false;


                        if ((secondsNow - secondsChange) >= 10)
                        {
                            secondsChange = secondsNow;
                            routineStage = routineStage + 1;
                        }
                    }
                    break;
                }

            // Case 8
            case (8):
                {
                    instruction1 = "A countdown will tell you when to start";
                    YourTurnIcon.showCountImage = true;

                    if ((secondsNow - secondsChange) >= 3)
                    {
                        instruction1 = "Now, practice with me by following my motion";

                        secondsChange = secondsNow;
                        routineStage = routineStage + 1;
                    }
                    break;
                }

            // Case 9 - Countdown
            case (9):
                {
                    // Move instructor avatar
                    rightElbow_rot_routine = new Vector3(0, -90, 0);   // Return to original elbow position

                    if ((secondsNow - secondsChange) >= 3)
                    {
                        ObserveTurnIcon.showCountImage = true;
                        ObserveTurnIcon.countNumber = 3;
                        if ((secondsNow - secondsChange) >= 4)
                        {
                            ObserveTurnIcon.countNumber = 2;
                            if ((secondsNow - secondsChange) >= 5)
                            {
                                ObserveTurnIcon.countNumber = 1;
                                if ((secondsNow - secondsChange) >= 6)
                                {
                                    ObserveTurnIcon.showCountImage = false;
                                    ObserveTurnIcon.countNumber = 4;   // Hide all countdown images

                                    secondsChange = secondsNow;
                                    routineStage = routineStage + 1;
                                }
                            }
                        }
                    }
                    break;
                }

            // Case 10 - Practice Elbow Bend
            case (10):
                {
                    if (rightElbow_rot_routine.x < elbowMotion)
                    {
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x + elbowSpeed,
                            rightElbow_rot_routine.y,
                            0);
                    }

                    // Move radial diagram
                    clientElbow_error = rightElbow_rot_routine.x - (RadialProgressMarker.clientElbow_rot - 270);
                    ProgressBar.maximum = 360;  // x2 as we only want for the circle to reach 0-180º, not 360º
                    ProgressBar.minimum = 0;
                    ProgressBar.current = (180 - rightElbow_rot_routine.x);

                    RadialRoutineMarker.clientElbow_rot_routine = rightElbow_rot_routine.x - 90;  // Set rotation of pair equal to fill amount


                    // End case when elbow joint reaches target
                    if (rightElbow_rot_routine.y >= elbowMotion)
                    {
                        if (practiceRoundsCounter == 0)
                        {
                            practiceRoundsCounter = 1;
                            instruction1 = "Let's practice this again";
                            secondsChange = secondsNow;
                            routineStage = 9;
                        }

                        else if (practiceRoundsCounter == 1)
                        {
                            practiceRoundsCounter = 0;
                            secondsChange = secondsNow;
                            routineStage = routineStage + 1;
                        }
                    }
                    break;
                }



            // Case 11 - Test Run
            case (11):
                {
                    instruction1 = "Now's let's do the test run. Get ready";
                    rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, 0, 0);   // Return to original elbow position

                    YourTurnIcon.showCountImage = true;
                    ObserveTurnIcon.showIconImage = false;

                    if ((secondsNow - secondsChange) >= 3)
                    {
                        YourTurnIcon.countNumber = 3;
                        if ((secondsNow - secondsChange) >= 5)
                        {
                            YourTurnIcon.countNumber = 2;
                            if ((secondsNow - secondsChange) >= 6)
                            {
                                YourTurnIcon.countNumber = 1;
                                if ((secondsNow - secondsChange) >= 7)
                                {
                                    YourTurnIcon.showCountImage = false;
                                    YourTurnIcon.countNumber = 4;

                                    secondsChange = secondsNow;
                                    routineStage = routineStage + 1;
                                }
                            }
                        }
                    }
                    break;
                }

            // Case 10 - Wait
            case (12):
                {
                    instruction1 = "Bend your elbow to catch the pear";

                    if ((secondsNow - secondsChange) > 0)
                    {
                        secondsChange = secondsNow;
                        routineStage = routineStage + 1;
                    }
                    break;
                }

            // Case 11 - Your turn
            case (13):
                {
                    // Move radial diagram
                    clientElbow_error = rightElbow_phantom - (RadialProgressMarker.clientElbow_rot - 270);

                    ProgressBar.maximum = 360;  // x2 as we only want for the circle to reach 0-180º, not 360º
                    ProgressBar.minimum = 0;
                    ProgressBar.current = 180 - rightElbow_phantom;

                    RadialRoutineMarker.clientElbow_rot_routine = rightElbow_phantom - 90;  // Set rotation of pair equal to fill amount
                    // UnityEngine.Debug.Log("Elbow Error: " + clientElbow_error);


                    if (rightElbow_phantom < elbowMotion)
                    {
                        rightElbow_phantom = rightElbow_phantom + elbowSpeed;
                    }

                    if (rightElbow_phantom >= elbowMotion)
                    {
                        rightElbow_phantom = 0;
                        secondsChange = secondsNow;

                        if (elbowSpeedCounter == 0)
                        {
                            elbowSpeed = 3.0f;
                            elbowSpeedCounter = 1;
                            instruction1 = "Great job! Now let's practice moving your elbow faster";
                            routineStage = 9;
                        }

                        else if (elbowSpeedCounter == 1)
                        {
                            elbowSpeed = 5.0f;
                            elbowSpeedCounter = 2;
                            instruction1 = "Fantastic! Let's practice moving your elbow even faster";
                            routineStage = 9;
                        }

                        else if (elbowSpeedCounter == 2)
                        {
                            routineStage = routineStage + 1;
                        }
                    }
                    break;
                }

            case (14):
                {
                    instruction1 = "Saving patient data, please wait...";
                    if ((secondsNow - secondsChange) > 4)
                    {
                        routineStage = routineStage+1;
                    }
                    break;
                }

            case (15):
                {
                    SaveRoutine save = new SaveRoutine();
                    save.emgCSVsave();  // Call function to save the raw and processed EMG CSVs

                    routineStage = routineStage + 1;
                    break;
                }

            // Save the processed EMG data in a CSV file at the end of the routine
            case (16):
                {
                    instruction1 = "Assessment complete. Well Done!";
                    routineStage = routineStage;
                    break;
                }
        }
    }

    void OnAnimatorIK(int layerIndex)
    {
        //print("OnAnimatorIK - running");
        anim.SetBoneLocalRotation(HumanBodyBones.Head, Quaternion.Euler(head_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(rightElbow_rot_routine));
        anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftElbow_rot));
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
