using UnityEngine;

public class SemiAutomatic : FireMode
{
    private float timer = 0;
    private bool letGoOfFire = false;

    public override bool TryToFire(Gun gun, float fireRate)
    {
        // track if you let go of Fire
        if(!gun.controls.isFiring)
            letGoOfFire = true;

        // wait for fire rate
        if (timer < fireRate)
            timer += Time.deltaTime;
        // then fire if youre trying to, as long as youre not holding it
        else if (gun.controls.isFiring && letGoOfFire)
        {
            timer = 0;
            letGoOfFire = false;
            return true;
        }
        return false;
    }

    public override void ResetGun(float fireRate) => timer = fireRate;

    public override string GetName() => "Semi-Automatic";
}
