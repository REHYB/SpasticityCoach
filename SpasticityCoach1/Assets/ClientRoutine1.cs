using System.Collections;
using System.Collections.Generic;
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
    float elbowSpeed = 0.25f;
    int elbowSpeedCounter = 0;
    float elbowMotion = 70f;
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
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x + 1, 0, 0);
                }
                if (head_rot.y < 40) {
                    head_rot = new Vector3(head_rot.x, head_rot.y + 1, 0);
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
                instruction1 = "Your turn, Bend your elbow with me";
                    rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, 0, 0);
                if ((secondsNow - secondsChange) > 3) {
                    routineStage = 7;
                }
                break;}
            case (7): {
                instruction1 = "Your turn, Bend your elbow with me";
                if (rightElbow_rot_routine.y < elbowMotion) {
                        rightElbow_rot_routine = new Vector3(rightElbow_rot_routine.x, rightElbow_rot_routine.y + elbowSpeed, 0);
                }
                if (rightElbow_rot_routine.y >= elbowMotion) {
                        rightElbow_rot_routine.y = 0;
                    secondsChange = secondsNow;
                    routineStage = 4;
                    if (elbowSpeedCounter == 0) {
                        elbowSpeed = 1.0f;
                        elbowSpeedCounter = 1;
                        instruction1 = "Bend your elbow faster";
                    }
                    else if (elbowSpeedCounter == 1) {
                        elbowSpeed = 3.0f;
                        elbowSpeedCounter = 2;
                        instruction1 = "Bend your elbow even faster";
                    }
                    else if (elbowSpeedCounter == 2) {
                         routineStage = 8;
                    }
                }
                break;}
            case (8): {
                instruction1 = "Assessment complete, Well Done!";
                routineStage = 8;
                break; }
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
