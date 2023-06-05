using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalProjectile : MonoBehaviour
{
    public Rigidbody rb;
    [HideInInspector] public UnityEvent<RaycastHit> OnHit;

    private Vector3 lastPosition;

    private void Awake()
    {
        // set last position to current one to prevent errors
        lastPosition = transform.position;
    }

    private void Update()
    {
        // if hit something between now and last frame, destroy projectile and invoke onhit event
        RaycastHit hit;
        if (Physics.Linecast(lastPosition, transform.position, out hit))
        {
            OnHit.Invoke(hit);
            Destroy(gameObject);
        }

        // store current pos for next frame
        lastPosition = transform.position;
    }

    // allows the ProjectileShooter to add velocity to this
    public void AddVelocity(Vector3 dir)
    {
        rb.velocity = dir;
    }
}
