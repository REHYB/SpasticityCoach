// This script controls the guided instructions with the avatar.

using AwesomeCharts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClientRoutine1 : MonoBehaviour
{
    public static string instruction1 = "Welcome to PIEGO"; //Personalized Independent Exercise Goals and Occupations
    public static Quaternion elbowMyo = new Quaternion(0, 0, 0, 0);
    public static int routineStage = 0;

    // Lets other script know if the routine has finished and if they should save CSV
    public static bool savePrcStat = false;

    Animator anim;
    //Transform head;
    public static Vector3 head_rot;
    Vector3 leftElbow_rot;
    public static Vector3 body_rot;
    public static Vector3 rightShoulder_rot;
    public static Vector3 leftShoulder_rot;
    public static Vector3 rightElbow_rot_routine;

    float secondsNow = 0;
    float secondsChange = 0;
    int makeTransition = Animator.StringToHash("MakeTransition");
    bool reverseMotion = false;
    float elbowSpeed = 1.5f;   // Changed initial elbowSpeed from 0.25f to 1.5f so that it is faster
    int elbowSpeedCounter = 0;
    float elbowMotion = 70f;
    
    public List<float> prc_emg_Pod01 = new List<float>();
    public List<float> prc_emg_Pod02 = new List<float>();
    public List<float> prc_emg_Pod03 = new List<float>();
    public List<float> prc_emg_Pod04 = new List<float>();
    public List<float> prc_emg_Pod05 = new List<float>();
    public List<float> prc_emg_Pod06 = new List<float>();
    public List<float> prc_emg_Pod07 = new List<float>();
    public List<float> prc_emg_Pod08 = new List<float>();
    public List<float> prc_emg_time;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //lying in Default Humanoid pose on Ground
        body_rot = new Vector3(-87f, 90f, 0f);
        GetComponent<Transform>().localPosition = new Vector3(GetComponent<Transform>().localPosition.x, GetComponent<Transform>().localPosition.y-0.2f, GetComponent<Transform>().localPosition.z);
        GetComponent<Transform>().localRotation = Quaternion.Euler(body_rot);
        rightShoulder_rot = new Vector3(0, 0, -58);
        leftShoulder_rot = new Vector3(0, -5, 50);
    }

    // Update is called once per frame
    void Update() {
        //Transform elbowMyo = transform.Find("Hub1").Find("Myo");
        //Debug.Log(elbowMyo.transform.localRotation.x);
        float move = Input.GetAxis("Vertical");
        anim.SetFloat(makeTransition, move);

        //Supinate wrist

        //if (Input.GetAxis("Horizontal") == 1) {
        //    head_rot = new Vector3(-40, 40, 0); // x side, y down, z twist? 
        //    rightElbow_rot = new Vector3(70, rightElbow_rot.y+1, 0); // x pronate, y up, no z
        //    rightShoulder_rot = new Vector3(0, -5, -58); // no x, y up, z across
        //    instruction1 = "Supinate Forearm";
        //}

        secondsNow = secondsNow + Time.deltaTime;

        switch (routineStage) {
            case (0): {
                //SpriteMeshType handColor = SpriteMeshRenderer.Find("Hub1").Find("Myo");
                instruction1 = "Welcome to Piego";
                if (secondsNow >= 2) {
                   routineStage = 1;
                }
                break;}

            case (1): {
                instruction1 = "I'm your Instructor, Milo";
                if (head_rot.x > -40) {
                    head_rot = new Vector3(head_rot.x - 1, 0, 0);
                }
                if (secondsNow >= 4) {
                    routineStage = 2;
                }
                break;}

            case (2): {
                instruction1 = "Let's begin your neuro assessment";
                if (secondsNow >= 6) {
                    routineStage = 3;
                }
                break;}

            case (3): {
                instruction1 = "Supinate your wrist";
                head_rot = new Vector3(-40, 40, 0);
                //Get Myo Y
                if (rightElbow_rot_routine.x < 70) {
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x + 5, 0, 0);   
                    }
                if (head_rot.y < 40) {
                    head_rot = new Vector3(head_rot.x, head_rot.y + 5, 0);
                }
                if (secondsNow >= 8) {
                    secondsChange = secondsNow;
                    routineStage = 4;
                    instruction1 = "Bend your elbow at this speed";
                }
                break;}

            case (4): {
                    rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, 0, 0);   
                if ((secondsNow - secondsChange) > 3) {
                    routineStage = 5;
                }
                break;}

            case (5): {
                if (rightElbow_rot_routine.y < elbowMotion) {
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, rightElbow_rot_routine.y + elbowSpeed, 0);
                }
                if (rightElbow_rot_routine.y >= elbowMotion) {
                        rightElbow_rot_routine.y = 0;
                    routineStage = 6;
                    secondsChange = secondsNow;
                }
                break;}

            case (6): {
                instruction1 = "Your turn, bend your elbow with me";
                    rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, 0, 0);
                if ((secondsNow - secondsChange) > 3) {
                    routineStage = 7;
                }
                break;}

            case (7): {
                instruction1 = "Your turn, bend your elbow with me";
                if (rightElbow_rot_routine.y < elbowMotion) {
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, rightElbow_rot_routine.y + elbowSpeed, 0);
                }
                if (rightElbow_rot_routine.y >= elbowMotion) {
                    rightElbow_rot_routine.y = 0;
                    secondsChange = secondsNow;
                    routineStage = 4;
                    if (elbowSpeedCounter == 0) {
                        elbowSpeed = 3.0f;
                        elbowSpeedCounter = 1;
                        instruction1 = "Bend your elbow faster";
                    }
                    else if (elbowSpeedCounter == 1) {
                        elbowSpeed = 5.0f;
                        elbowSpeedCounter = 2;
                        instruction1 = "Bend your elbow even faster";
                    }
                    else if (elbowSpeedCounter == 2) {
                         routineStage = 8;
                    }
                }
                break;}

            case (8):
                {
                    instruction1 = "Saving patient data, please wait...";

                    savePrcStat = true;
                    
                    // ---------- Save moving average values to CSV ----------
                    // Get EMG pod data values
                    //LineChartController_EMG02 LChart_EMG02 = new LineChartController_EMG02();
                    prc_emg_Pod02 = LineChartController_EMG02.avg_emg_Pod02;
                    UnityEngine.Debug.Log("prc_emg_Pod02 size: " + prc_emg_Pod02.Count);
                    UnityEngine.Debug.Log("prc_emg_Pod02 values: " + prc_emg_Pod02[3] + " and "+ prc_emg_Pod02[123]);

                    CsvReadWrite csv = new CsvReadWrite();                    // Elapsed time for saveCSV function: 4612 ms for 18,100 rows
                    csv.savePrcCSV("EMG_Avg.csv", prc_emg_Pod02, prc_emg_Pod02, prc_emg_Pod02, prc_emg_Pod02, prc_emg_Pod02, prc_emg_Pod02, prc_emg_Pod02, prc_emg_Pod02);
                    
                    /*
                    Debug.Log("Checking the size of avg_emg_Pod01: " + avg_emg_Pod01.Length);
                    Debug.Log("Checking the size of avg_emg_Pod02: " + avg_emg_Pod02.Length);
                    Debug.Log("Checking the size of avg_emg_Pod03: " + avg_emg_Pod03.Length);
                    Debug.Log("Checking the size of avg_emg_Pod04: " + avg_emg_Pod04.Length);
                    Debug.Log("Checking the size of avg_emg_Pod05: " + avg_emg_Pod05.Length);
                    Debug.Log("Checking the size of avg_emg_Pod06: " + avg_emg_Pod06.Length);
                    Debug.Log("Checking the size of avg_emg_Pod07: " + avg_emg_Pod07.Length);
                    Debug.Log("Checking the size of avg_emg_Pod08: " + avg_emg_Pod08.Length);
                    //Debug.Log("Checking the size of avg_timestamp: " + avg_timestamp.Length);
                    */

                    routineStage = 9;
                    break;
                }

            // Save the processed EMG data in a CSV file at the end of the routine
            case (9):
                {
                    instruction1 = "Assessment complete. Well Done!";
                    routineStage = 9;
                    savePrcStat = false;
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
    }
}
