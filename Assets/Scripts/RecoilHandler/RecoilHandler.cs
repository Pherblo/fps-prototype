using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecoilHandler : MonoBehaviour
{
    public abstract void AddRecoil(); // add simulated recoil to gun
    public abstract Vector3 GetProjectileDirectionOffset(); // return a rotational offset for the projectile to create spread effects 
    public abstract float GetMaximumSpread(); // return an estimated average size of the guns current group shot

    // can be used be inheriting scripts as an easy way of getting a point in a circle
    public static Vector2 PointOnCircle(float radius, float angleInDegrees)
    {
        // convert degrees to radians
        float angle = angleInDegrees * Mathf.PI / 180F;
        // get x pos from cosine of angle
        float x = radius * Mathf.Cos(angle);
        // get y pos from sine of angle
        float y = radius * Mathf.Sin(angle);

        return new Vector2(x, y);
    }
}
