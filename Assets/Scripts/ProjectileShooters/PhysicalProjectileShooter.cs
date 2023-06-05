using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalProjectileShooter : ProjectileShooter
{
    [Header("Camera")]
    [Tooltip("Camera used to fire raycast from. Unused if alwaysAimForCenter = false")]
    public Transform cam;
    [Tooltip("Type of recoil to be used on each shot")]
    public RecoilHandler recoilHandler;

    [Header("Raycast")]
    [Tooltip("Sometimes the gun isnt pointing at the center of the screen, this will bend the projectiles to hit the center anyways")]
    public bool alwaysAimForCenter = true;
    [Tooltip("Offset of where the crosshair is on the Canvas (default is centered)")]
    public Vector3 crosshairCenter = new Vector3(0.5f, 0.5f, 0f);
    [Tooltip("How long the rayast will be that finds the center of the camera")]
    public float range = 50f;

    [Header("Projectile")]
    public int shotCount = 1;
    [Tooltip("Prefab to be shot")]
    public GameObject projectile;
    [Tooltip("Location of the muzzle, where the projectiles are shot from")]
    public Transform muzzle;
    [Tooltip("Starting Velocity of the projectile")]
    public float velocity = 100f;
    [Tooltip("Velocity transfered to the hit object")]
    public float hitForce = 100f;
    [Tooltip("Maximum lifetime of each projectile")]
    public float lifeTime = 3f;
    [Tooltip("Impact prefab for when an object is shot")]
    public GameObject impactEffect;

    [Space]
    public UnityEvent OnFire;

    public override void Fire()
    {
        // invoke onfire event for triggering sounds, vfx, etc.
        OnFire.Invoke();

        // if you want to always aim for center, claculate for that, otherwise just aim forward of the muzzle
        Vector3 forward = alwaysAimForCenter ? (GetCenteredProjectilePos() - muzzle.transform.position).normalized : muzzle.transform.forward;

        // for every shot to be fires...
        for (int i = 0; i < shotCount; i++)
        {
            // if theres recoil, add that to the projectiles direction
            Vector3 dir = recoilHandler ? forward + muzzle.TransformDirection(recoilHandler.GetProjectileDirectionOffset()) : forward;

            // spawn the projectile, and store its PhysicalProjectile component
            PhysicalProjectile obj = Instantiate(projectile, muzzle.transform.position, Quaternion.LookRotation(dir)).GetComponent<PhysicalProjectile>();

            // listen to onhit event and add velocity to projectile
            obj.OnHit.AddListener(OnHit);
            obj.AddVelocity(obj.transform.forward * velocity);
        
            // set a timer to destroy the projectile after a certain time
            Destroy(obj.gameObject, lifeTime);
        }

        // if theres a visual recoil handler, trigger it
        recoilHandler?.AddRecoil();
    }

    public void OnHit(RaycastHit hit)
    {
        // spawn a impact effect
        Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal))
            .transform.SetParent(hit.transform);

        // and add force to the rigidbody if there is one
        if (hit.rigidbody != null && !hit.rigidbody.isKinematic)
            hit.rigidbody.AddForce(-hit.normal * hitForce);
    }

    private Vector3 GetCenteredProjectilePos()
    {
        // send a raycast from the camera to see where the gun should aim
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, range))
            return hit.point;
        // if not hit, just return a point in front of the camera for an estimate
        else
            return cam.position + cam.forward * range;
    }
}
