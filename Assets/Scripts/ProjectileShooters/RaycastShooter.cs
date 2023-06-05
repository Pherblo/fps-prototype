using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RaycastShooter : ProjectileShooter
{
    [Header("Camera")]
    [Tooltip("Camera used to fire raycast from")]
    public Camera cam;
    [Tooltip("Type of recoil to be used on each shot")]
    public RecoilHandler recoilHandler;

    [Header("Raycast")]
    [Tooltip("Number of raycasts to be fired")]
    public int shotCount = 1;
    [Tooltip("Offset of where the crosshair is on the Canvas (default is centered)")]
    public Vector3 crosshairCenter = new Vector3(0.5f, 0.5f, 0f);
    [Tooltip("How long the projectile raycast will be")]
    public float range = 50f;
    [Tooltip("Velocity transfered to the hit object")]
    public float hitForce = 100f;
    [Tooltip("Impact prefab for when an object is shot")]
    public GameObject impactEffect;

    [Space]
    public UnityEvent OnFire;

    public override void Fire()
    {
        // invoke onfire event for triggering sounds, vfx, etc.
        OnFire.Invoke();

        // for every shot to be fires...
        for (int i = 0; i < shotCount; i++)
        {
            // if theres recoil, add that to the projectiles direction
            Vector3 dir = recoilHandler ? cam.transform.forward + cam.transform.TransformDirection(recoilHandler.GetProjectileDirectionOffset()) : cam.transform.forward;

            // shoot a raycast to where youre pointing, and if something is hit...
            RaycastHit hit;
            if (Physics.Raycast(cam.ViewportToWorldPoint(crosshairCenter), dir, out hit, range))
            {
                // spawn a impact effect
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal))
                    .transform.SetParent(hit.transform);

                // and add force to the rigidbody if there is one
                if (hit.rigidbody != null && !hit.rigidbody.isKinematic)
                    hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }

        // if theres a visual recoil handler, trigger it
        recoilHandler?.AddRecoil();
    }
}
