using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sways the gun when you look around
public class GunSway : MonoBehaviour
{
    public InputManager input; // used to get look input
    [Space]
    [Tooltip("How much the gun can move in each direction")]
    public Vector2 movement;
    [Tooltip("How smoothly the gun moves when you look around")]
    public float movementSmoothness;
    [Space]
    [Tooltip("How much the gun can rotate in each direction")]
    public Vector3 rotation;
    [Tooltip("How smoothly the gun rotates when you look around")]
    public float rotationSmoothness;

    private Vector3 initialPos;
    private Quaternion initialRot;

    void Start()
    {
        // store initial position and rotation so we can return to it
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
    }

    void Update()
    {
        // calculate how much the gun should sway
        float moveX = input.look.x * movement.x;
        float moveY = input.look.y * movement.y;

        // calculate how much the gun should rotate
        float rotX = input.look.x * rotation.x;
        float rotY = input.look.y * rotation.y;
        float rotZ = input.look.x * rotation.z;

        // store final calculations in Vector3s for use
        Vector3 finalPos = new Vector3(moveX, moveY, 0);
        Vector3 finalRot = new Vector3(rotY, rotX, rotZ);

        // lerp position and rotation to final values
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos + initialPos, Time.deltaTime * movementSmoothness);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(finalRot) * initialRot, Time.deltaTime * rotationSmoothness);
    }
}
