using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shows a dynamic corsshair that scales to show exactly where youll shoot
public class DynamixCrosshair : MonoBehaviour
{
    public RecoilHandler gun; // used to find max amount of recoil spread
    public GunSights sights; // used to check if were aiming the gun
    public Transform origin; // set to camera root to have the crosshair always be in the center, or the muzzle to have the cross hair always point exactly where the gun points
    public Camera cam; // main camera for calculating world to screen points
    public RectTransform crosshair; // parent crosshair to scale
    public float crosshairSize = 20; // size of 2 crosshair sprites, used to make sure you shoot inside the crosshair

    private Vector3 worldCenter;
    private Vector3 worldOffset;
    private Vector3 screenDistance;

    void Update()
    {
        // disable crosshair when aiming down the sights
        crosshair.gameObject.SetActive(sights && !sights.IsAiming() || !sights);

        // calculate points representing where the gun is pointing, and the worst it could point from recoil
        worldCenter = origin.transform.position + Quaternion.AngleAxis(0, origin.transform.up) * origin.transform.forward;
        worldOffset = origin.transform.position + Quaternion.AngleAxis(gun.GetMaximumSpread(), origin.transform.up) * origin.transform.forward;

        // get the distance between the points in screensize so we know exactly where to put the crosshair
        screenDistance = cam.WorldToScreenPoint(worldOffset) - cam.WorldToScreenPoint(worldCenter);
        // then scale crosshair to fit the gun spread
        float size = screenDistance.x + crosshairSize;
        crosshair.sizeDelta = new Vector2(size, size);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldCenter, 0.1f);
        Gizmos.DrawSphere(worldOffset, 0.1f);
    }
}