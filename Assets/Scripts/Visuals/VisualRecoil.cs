using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// kicks gun back when you shoot
public class VisualRecoil : MonoBehaviour
{
    [Tooltip("How much movement is added to the gun when shooting")]
    public Vector3 movement = new Vector3(0,0,1);
    [Tooltip("How smoothly the gun kicks back")]
    public float moveRecoilSmoothness;
    [Tooltip("How smoothly the gun returns to resting position")]
    public float moveRegainSmoothness;
    [Space]
    [Tooltip("The rotation set to the gun when shooting")]
    public Vector3 rotation = new Vector3(1, 0, 0);
    [Tooltip("How smoothly the gun rotates back")]
    public float rotRecoilSmoothness;
    [Tooltip("How smoothly the gun returns to resting rotation")]
    public float rotRegainSmoothness;

    private Vector3 initialPos;
    private Quaternion initialRot;

    private Vector3 targetPos;
    private Quaternion targetRot;

    public void AddRecoil()
    {
        // increase target position and rotation values
        targetPos += movement;
        targetRot = Quaternion.Euler(rotation);
    }

    void Start()
    {
        // store initial position and rotation so we can return to it
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
    }

    void Update()
    {
        // move gun to target pos and rot, with a speed depending on whether your receiving or recovering recoil
        transform.localPosition = Vector3.Slerp(transform.localPosition, targetPos + initialPos, Time.deltaTime * (targetPos == initialPos ? moveRegainSmoothness : moveRecoilSmoothness));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * (targetPos == initialPos ? rotRegainSmoothness : rotRecoilSmoothness));

        // when the gun is finished with recoil, go back to its original position
        if (transform.localPosition == targetPos + initialPos) 
            targetPos = initialPos;
        if (transform.localRotation == targetRot) 
            targetRot = initialRot;
    }
}
