using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderBetweenTwoPoints : MonoBehaviour
{
    [SerializeField]
    private Transform cylinderPrefab;

    private GameObject leftSphere;
    private GameObject rightSphere;
    private GameObject cylinder;

    private Vector3 modelRightKnee_pos;
    private Vector3 modelRightHand_pos;
    private Vector3 cylinder_rot;


    private void Start()
    {
        leftSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rightSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leftSphere.transform.position = new Vector3(-1, 0, 0);
        rightSphere.transform.position = new Vector3(1, 0, 0);

        // InstantiateCylinder(cylinderPrefab, leftSphere.transform.position, rightSphere.transform.position);
        InstantiateCylinder(cylinderPrefab, RightKneePosition.rightKnee_pos, RightHandPosition.rightHand_pos);
    }

    private void Update()
    {
        // Retrieve global position of right hand and knee
        modelRightKnee_pos = new Vector3(RightKneePosition.rightKnee_pos.x + 0.5f,
            RightKneePosition.rightKnee_pos.y + 0.5f,
            RightKneePosition.rightKnee_pos.z);

        modelRightHand_pos = new Vector3(RightHandPosition.rightHand_pos.x,
            RightHandPosition.rightHand_pos.y,
            RightHandPosition.rightHand_pos.z);

        leftSphere.transform.position = modelRightKnee_pos;
        rightSphere.transform.position = modelRightHand_pos;

        // UpdateCylinderPosition(cylinder, leftSphere.transform.position, rightSphere.transform.position);
        UpdateCylinderPosition(cylinder, modelRightKnee_pos, modelRightHand_pos);

    }

    private void InstantiateCylinder(Transform cylinderPrefab, Vector3 beginPoint, Vector3 endPoint)
    {
        cylinder_rot = new Vector3(0, 0, 0);
        // cylinder = Instantiate<GameObject>(cylinderPrefab.gameObject, Vector3.zero, Quaternion.identity);
        cylinder = Instantiate<GameObject>(cylinderPrefab.gameObject, cylinder_rot, Quaternion.identity);
        UpdateCylinderPosition(cylinder, beginPoint, endPoint);
    }

    private void UpdateCylinderPosition(GameObject cylinder, Vector3 beginPoint, Vector3 endPoint)
    {
        Vector3 offset = endPoint - beginPoint;
        Vector3 position = beginPoint + (offset / 2.0f);

        cylinder.transform.position = position;
        cylinder.transform.LookAt(beginPoint);
        Vector3 localScale = cylinder.transform.localScale;
        localScale.z = (endPoint - beginPoint).magnitude;
        cylinder.transform.localScale = localScale;
    }
}