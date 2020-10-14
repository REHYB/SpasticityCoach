using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBetweenTwoPoints : MonoBehaviour
{
    [SerializeField]
    private Transform cubePrefab;

    private GameObject leftSphere;
    private GameObject rightSphere;
    private GameObject cube;

    private Vector3 modelRightKnee_pos;
    private Vector3 modelRightHand_pos;
    private Vector3 cube_rot;


    private void Start()
    {
        leftSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rightSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        leftSphere.transform.position = new Vector3(-1, 0, 0);
        rightSphere.transform.position = new Vector3(1, 0, 0);

        // InstantiateCube(cubePrefab, leftSphere.transform.position, rightSphere.transform.position);
        InstantiateCube(cubePrefab, RightKneePosition.rightKnee_pos, RightHandPosition.rightHand_pos);
    }

    private void Update()
    {
        // Retrieve global position of right hand and knee
        modelRightKnee_pos = new Vector3(RightKneePosition.rightKnee_pos.x - 0.5f,
            RightKneePosition.rightKnee_pos.y + 0.45f,
            RightKneePosition.rightKnee_pos.z - 0.5f);

        modelRightHand_pos = new Vector3(RightHandPosition.rightHand_pos.x,
            RightHandPosition.rightHand_pos.y + 0.15f,
            RightHandPosition.rightHand_pos.z - 0.4f);

        Vector3 minScale = new Vector3(0.01f, 0.01f, 0.01f);
        leftSphere.transform.localScale = minScale;
        rightSphere.transform.localScale = minScale;

        leftSphere.transform.position = modelRightKnee_pos;
        rightSphere.transform.position = modelRightHand_pos;

        // UpdateCubePosition(cube, leftSphere.transform.position, rightSphere.transform.position);
        UpdateCubePosition(cube, modelRightKnee_pos, modelRightHand_pos);

    }

    private void InstantiateCube(Transform cubePrefab, Vector3 beginPoint, Vector3 endPoint)
    {
        cube_rot = new Vector3(0, 0, 0);
        // cube = Instantiate<GameObject>(cubePrefab.gameObject, Vector3.zero, Quaternion.identity);
        cube = Instantiate<GameObject>(cubePrefab.gameObject, cube_rot, Quaternion.identity);
        UpdateCubePosition(cube, beginPoint, endPoint);
    }

    private void UpdateCubePosition(GameObject cube, Vector3 beginPoint, Vector3 endPoint)
    {
        Vector3 offset = endPoint - beginPoint;
        Vector3 position = beginPoint + (offset / 2.0f);

        cube.transform.position = position;
        cube.transform.LookAt(beginPoint);
        
        // Vector3 localScale = cube.transform.localScale;
        Vector3 bandScale = new Vector3(0.1f, 0.3f, 0.1f);
        Vector3 localScale = bandScale;
        localScale.z = (endPoint - beginPoint).magnitude;
        cube.transform.localScale = localScale;
    }
}