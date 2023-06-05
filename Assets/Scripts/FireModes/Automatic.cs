using UnityEngine;

public class Automatic : FireMode
{
    private float timer = 0;
    public override bool TryToFire(Gun gun, float fireRate)
    {
        // wait for fire rate timer
        if (timer < fireRate)
            timer += Time.deltaTime;
        // shoot gun if pressing button
        else if (gun.controls.isFiring)
        {
            timer = 0;
            return true;
        }
        // otherise you dont shoot
        return false;
    }

    public override void ResetGun(float fireRate) => timer = fireRate;

    public override string GetName() => "Automatic";
}
