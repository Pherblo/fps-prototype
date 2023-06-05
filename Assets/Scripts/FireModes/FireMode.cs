using UnityEngine;

public abstract class FireMode : MonoBehaviour
{
    public abstract string GetName(); // returns name of fire mode
    public abstract bool TryToFire(Gun gun, float fireRate); // should be called every frame to see if the gun should shoot
    public abstract void ResetGun(float fireRate); // resets the internal gun timers
}