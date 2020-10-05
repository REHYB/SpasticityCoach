// This script controls the guided instructions with the avatar.

using AwesomeCharts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;


public class ClientRoutine1 : MonoBehaviour
{
    public static string instruction1 = "Welcome to PIEGO"; //Personalized Independent Exercise Goals and Occupations
    public static Quaternion elbowMyo = new Quaternion(0, 0, 0, 0);
    public static int routineStage = 0;

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
    
    public List<float> prc_emg_Pod01;
    public List<float> prc_emg_Pod02;
    public List<float> prc_emg_Pod03;
    public List<float> prc_emg_Pod04;
    public List<float> prc_emg_Pod05;
    public List<float> prc_emg_Pod06;
    public List<float> prc_emg_Pod07;
    public List<float> prc_emg_Pod08;
    public List<float> prc_emg_time;

    public List<int> raw_emg_Pod01;
    public List<int> raw_emg_Pod02;
    public List<int> raw_emg_Pod03;
    public List<int> raw_emg_Pod04;
    public List<int> raw_emg_Pod05;
    public List<int> raw_emg_Pod06;
    public List<int> raw_emg_Pod07;
    public List<int> raw_emg_Pod08;
    public List<DateTime> raw_emg_time;


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
                    if ((secondsNow - secondsChange) > 4)
                    {
                            routineStage = 9;
                    }
                    break;
                }

            case (9):
                {
                    // ---------- Save moving average values to CSV ----------

                    // Get raw EMG pod data values
                    raw_emg_Pod01 = StoreEMG.storeEMG01;
                    raw_emg_Pod02 = StoreEMG.storeEMG02;
                    raw_emg_Pod03 = StoreEMG.storeEMG03;
                    raw_emg_Pod04 = StoreEMG.storeEMG04;
                    raw_emg_Pod05 = StoreEMG.storeEMG05;
                    raw_emg_Pod06 = StoreEMG.storeEMG06;
                    raw_emg_Pod07 = StoreEMG.storeEMG07;
                    raw_emg_Pod08 = StoreEMG.storeEMG08;
                    raw_emg_time = StoreEMG.timestamp;


                    // Get processed EMG pod data values
                    prc_emg_Pod01 = LineChartController_EMG01.avg_emg_Pod01;
                    prc_emg_Pod02 = LineChartController_EMG02.avg_emg_Pod02;
                    prc_emg_Pod03 = LineChartController_EMG03.avg_emg_Pod03;
                    prc_emg_Pod04 = LineChartController_EMG04.avg_emg_Pod04;
                    prc_emg_Pod05 = LineChartController_EMG05.avg_emg_Pod05;
                    prc_emg_Pod06 = LineChartController_EMG06.avg_emg_Pod06;
                    prc_emg_Pod07 = LineChartController_EMG07.avg_emg_Pod07;
                    prc_emg_Pod08 = LineChartController_EMG08.avg_emg_Pod08;

                    UnityEngine.Debug.Log("----------------------------------------------");
                    UnityEngine.Debug.Log("Size of Processed EMG 01: " + prc_emg_Pod01.Count);
                    UnityEngine.Debug.Log("Size of Processed EMG 02: " + prc_emg_Pod02.Count);
                    UnityEngine.Debug.Log("Size of Processed EMG 03: " + prc_emg_Pod03.Count);
                    UnityEngine.Debug.Log("Size of Processed EMG 04: " + prc_emg_Pod04.Count);
                    UnityEngine.Debug.Log("Size of Processed EMG 05: " + prc_emg_Pod05.Count);
                    UnityEngine.Debug.Log("Size of Processed EMG 06: " + prc_emg_Pod06.Count);
                    UnityEngine.Debug.Log("Size of Processed EMG 07: " + prc_emg_Pod07.Count);
                    UnityEngine.Debug.Log("Size of Processed EMG 08: " + prc_emg_Pod08.Count);
                    // The sizes of EMG 01 and 06 are +1 element bigger than the others


                    // ------------------------- Raw EMG -------------------------
                    // Write raw EMG into a CSV file
                    CsvReadWrite csv = new CsvReadWrite();
                    csv.saveRawCSV("EMG_data.csv", raw_emg_Pod01, raw_emg_Pod02, raw_emg_Pod03, raw_emg_Pod04, raw_emg_Pod05, raw_emg_Pod06, raw_emg_Pod07, raw_emg_Pod08, raw_emg_time);

                    // ------------------------- Processed EMG -------------------------
                    // Read timestamps for processed EMG
                    DataFltr csvFltr = new DataFltr();
                    var values = csvFltr.readEMGCSV("EMG_data.csv");
                    int len = prc_emg_Pod01.Count;

                    // Convert string back to timestamp for CSV. 
                    // Avoids the output in a CSV being System.string[] instead of the actual timestamp
                    DateTime[] prc_emg_time = new DateTime[len];
                    string[] emg_time = values.Item9;

                    int counter = 0;
                    for (int i=1; i<len+1; i++) {
                        prc_emg_time[counter] = DateTime.ParseExact(emg_time[i], "yyyy-MM-dd H:mm:ss.fff", null);
                        counter = counter + 1;
                    }
                    UnityEngine.Debug.Log("Size of Processed EMG Time: " + prc_emg_Pod08.Count);
                    
                    // Write processed EMG into a CSV file
                    csv.savePrcCSV("EMG_processed.csv", prc_emg_Pod01, prc_emg_Pod02, prc_emg_Pod03, prc_emg_Pod04, prc_emg_Pod05, prc_emg_Pod06, prc_emg_Pod07, prc_emg_Pod08, prc_emg_time);
                    
                    routineStage = 10;
                    break;
                }

            // Save the processed EMG data in a CSV file at the end of the routine
            case (10):
                {
                    instruction1 = "Assessment complete. Well Done!";
                    routineStage = 10;
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
