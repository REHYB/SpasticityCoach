// This script controls and mimics the elbow rotation of the patient and visually translates that into the patient avatar.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMotion : MonoBehaviour
{
    public static string instruction1 = "Welcome to PIEGO"; //Personalized Independent Exercise Goals and Occupations
    public static int routineStage = 0;

    Animator anim;
    //Transform head;
    Vector3 rightElbow_rot;

    float secondsNow = 0;
    float secondsChange = 0;
    int makeTransition = Animator.StringToHash("MakeTransition");
    bool reverseMotion = false;
    // Start is called before the first frame update

    void Start()
    {
        anim = GetComponent<Animator>();
        //lying in Default Humanoid pose on Ground
        ClientRoutine_Sitting.body_rot = new Vector3(-87f, 90f, 0f);
        
        // Local position and rotation of the complete body
        GetComponent<Transform>().localPosition = new Vector3(GetComponent<Transform>().localPosition.x, GetComponent<Transform>().localPosition.y - 0.2f, GetComponent<Transform>().localPosition.z);
        GetComponent<Transform>().localRotation = Quaternion.Euler(ClientRoutine_Sitting.body_rot);

        ClientRoutine_Sitting.rightShoulder_rot = new Vector3(0, 0, -58);
        ClientRoutine_Sitting.leftShoulder_rot = new Vector3(0, -5, 50);
    }

    // Update is called once per frame
    void Update()
    {
        // Fixed rotation of elbow by setting elbowMyo to the -y axis
        rightElbow_rot = new Vector3(90, -ClientRoutine_Sitting.elbowMyo.y * 180, 0);
        //        Debug.Log(ClientRoutine_Sitting.elbowMyo.x + " : " + ClientRoutine_Sitting.elbowMyo.y + " : " + ClientRoutine_Sitting.elbowMyo.z);
        
    }

    void OnAnimatorIK(int layerIndex)
    {
        //print("OnAnimatorIK - running");
        anim.SetBoneLocalRotation(HumanBodyBones.Head, Quaternion.Euler(ClientRoutine_Sitting.head_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightUpperArm, Quaternion.Euler(ClientRoutine_Sitting.rightShoulder_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.LeftUpperArm, Quaternion.Euler(ClientRoutine_Sitting.leftShoulder_rot));
        if (ClientRoutine_Sitting.routineStage < 4) {
            anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(ClientRoutine_Sitting.rightElbow_rot_routine));
        }
        else {
            anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(rightElbow_rot));
        }
    }
}
