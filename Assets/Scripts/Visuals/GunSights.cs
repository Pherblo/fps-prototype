using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// allows you to aim down your sights
public class GunSights : MonoBehaviour
{
    [Tooltip("Speed that you position the gun to aim down sights")]
    public float aimSpeed;
    [Tooltip("Speed that you ease your weapon to the hip")]
    public float easeSpeed;
    [Tooltip("The position you use when aiming down sights")]
    public Vector3 targetPos;

    private Vector3 initialPos;
    private bool aiming = false;

    private void Start()
    {
        // store initial position for moving gun back to resting/hip position
        initialPos = transform.localPosition;
    }

    public void UpdatePosition(InputManager controls, bool isReloading)
    {
        // set aiming to if aim button is pressed, unless reloading in which case you cant aim
        aiming = isReloading ? false : controls.aiming;

        // if youre aiming down sights move to correct position
        if(aiming)
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, aimSpeed);
        // otherwise move to resting/hip position
        else
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPos, easeSpeed);
    }

    // public method for checking if were aiming
    public bool IsAiming() => aiming;
}
