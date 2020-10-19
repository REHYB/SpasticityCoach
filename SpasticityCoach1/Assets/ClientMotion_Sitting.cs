// This script controls and mimics the elbow rotation of the patient and visually translates that into the patient avatar.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientMotion_Sitting : MonoBehaviour
{
    public static int routineStage = 0;

    Animator anim;
    public static Vector3 rightElbow_rot_client;

    float secondsNow = 0;
    float secondsChange = 0;
    int makeTransition = Animator.StringToHash("MakeTransition");
    bool reverseMotion = false;

    Color trans_snow = new Color(252 / 255f, 239 / 255f, 239 / 255f, 0.2f);     // Colour for the mesh renderer
    Color trans_maxblue = new Color(71 / 255f, 186 / 255f, 210 / 255f, 0.2f);
    Color trans_fuchsia = new Color(255 / 255f, 0 / 255f, 255 / 255f, 0.2f);

    void Start()
    {
        anim = GetComponent<Animator>();

        // Initial location and rotation of entire body
        Vector3 body_rot = new Vector3(0f, 85f, 0f);
        float body_posX = -19f;
        float body_posY = -2f;
        float body_posZ = -0.4f; //-1.5f;

        GetComponent<Transform>().localPosition = new Vector3(GetComponent<Transform>().localPosition.x + body_posX,
            GetComponent<Transform>().localPosition.y + body_posY,
            GetComponent<Transform>().localPosition.z + body_posZ);

        GetComponent<Transform>().localRotation = Quaternion.Euler(body_rot);

    }

    // Update is called once per frame
    void Update()
    {
        // Set model colour
        ModelColour setColour = new ModelColour();
        setColour.setModelColour(trans_snow);

        // Fixed rotation of elbow by setting elbowMyo to the -y axis
        // Note: Myo logo should be on the outer side of the forearm + blue rectangle down (close to elbow)
        rightElbow_rot_client = new Vector3(ClientRoutine_Sitting.rightElbow_rot_routine.x, 
            ClientRoutine_Sitting.elbowMyo.y * 180,
            ClientRoutine_Sitting.rightElbow_rot_routine.z);

        if (rightElbow_rot_client.y > ClientRoutine_Sitting.rightElbow_rot_routine.y)
        {
            setColour.setModelColour(trans_maxblue);
        }

        else {
            setColour.setModelColour(trans_fuchsia);
        }

    }

    void OnAnimatorIK(int layerIndex)
    {
        //print("OnAnimatorIK - running");
        anim.SetBoneLocalRotation(HumanBodyBones.Head, Quaternion.Euler(ClientRoutine_Sitting.head_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightUpperArm, Quaternion.Euler(ClientRoutine_Sitting.rightShoulder_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.LeftUpperArm, Quaternion.Euler(ClientRoutine_Sitting.leftShoulder_rot));

        anim.SetBoneLocalRotation(HumanBodyBones.LeftUpperLeg, Quaternion.Euler(ClientRoutine_Sitting.leftUpLeg_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightUpperLeg, Quaternion.Euler(ClientRoutine_Sitting.rightUpLeg_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerLeg, Quaternion.Euler(ClientRoutine_Sitting.leftKnee_rot));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLowerLeg, Quaternion.Euler(ClientRoutine_Sitting.rightKnee_rot));

        // Right fingers
        anim.SetBoneLocalRotation(HumanBodyBones.RightThumbProximal, Quaternion.Euler(ClientRoutine_Sitting.RProxFingers[0]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightThumbIntermediate, Quaternion.Euler(ClientRoutine_Sitting.RIntFingers[0]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightThumbDistal, Quaternion.Euler(ClientRoutine_Sitting.RDistFingers[0]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightIndexProximal, Quaternion.Euler(ClientRoutine_Sitting.RProxFingers[1]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightIndexIntermediate, Quaternion.Euler(ClientRoutine_Sitting.RIntFingers[1]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightIndexDistal, Quaternion.Euler(ClientRoutine_Sitting.RDistFingers[1]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightMiddleProximal, Quaternion.Euler(ClientRoutine_Sitting.RProxFingers[2]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightMiddleIntermediate, Quaternion.Euler(ClientRoutine_Sitting.RIntFingers[2]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightMiddleDistal, Quaternion.Euler(ClientRoutine_Sitting.RDistFingers[2]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightRingProximal, Quaternion.Euler(ClientRoutine_Sitting.RProxFingers[3]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightRingIntermediate, Quaternion.Euler(ClientRoutine_Sitting.RIntFingers[3]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightRingDistal, Quaternion.Euler(ClientRoutine_Sitting.RDistFingers[3]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLittleProximal, Quaternion.Euler(ClientRoutine_Sitting.RProxFingers[4]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLittleIntermediate, Quaternion.Euler(ClientRoutine_Sitting.RIntFingers[4]));
        anim.SetBoneLocalRotation(HumanBodyBones.RightLittleDistal, Quaternion.Euler(ClientRoutine_Sitting.RDistFingers[4]));

        // Right elbow motion dependent on Routine Stage
        if (ClientRoutine_Sitting.routineStage < 9) {
            anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(ClientRoutine_Sitting.rightElbow_rot_routine));
        }

        else if (ClientRoutine_Sitting.routineStage >= 12)
        {
            anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(ClientRoutine_Sitting.rightElbow_rot_routine));
        }

        else {
            anim.SetBoneLocalRotation(HumanBodyBones.RightLowerArm, Quaternion.Euler(rightElbow_rot_client));
        }
    }
}
