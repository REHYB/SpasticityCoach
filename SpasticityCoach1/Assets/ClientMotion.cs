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
        ClientRoutine1.body_rot = new Vector3(-87f, 90f, 0f);
        GetComponent<Transform>().localPosition = new Vector3(GetComponent<Transform>().localPosition.x, GetComponent<Transform>().localPosition.y - 0.2f, GetComponent<Transform>().localPosition.z);
        GetComponent<Transform>().localRotation = Quaternion.Euler(ClientRoutine1.body_rot);
        ClientRoutine1.rightShoulder_rot = new Vector3(0, 0, -58);
        ClientRoutine1.leftShoulder_rot = new Vector3(0, -5, 50);
    }

    // Update is called once per frame
    void Update()
    {
        rightElbow_rot = new Vector3 (90, ClientRoutine1.elbowMyo.x * 180, 0);
//        Debug.Log(ClientRoutine1.elbowMyo.x + " : " + ClientRoutine1.elbowMyo.y + " : " + ClientRoutine1.elbowMyo.z);
    }

    void OnAnimatorIK(int layerIndex)
    {
        //print("OnAnimatorIK - running");
        anim.SetBoneLocalRotation(HumanBodyBones.Head, Quaternion.Euler(ClientRoutine1.head_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightUpperArm, Quaternion.Euler(ClientRoutine1.rightShoulder_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.LeftUpperArm, Quaternion.Euler(ClientRoutine1.leftShoulder_rot));
        if (ClientRoutine1.routineStage < 4) {
            anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(ClientRoutine1.rightElbow_rot_routine));
        }
        else {
            anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(rightElbow_rot));
        }
    }
}
