using UnityEngine;

public class Burst : FireMode
{
    [Space]
    [Tooltip("How many shots will be fired in a row per burst")]
    public int burstCount = 3;
    [Tooltip("Extra time added in between firing bursts")]
    public float burstTimer = 0.1f;

    private int burstCounter = 0;
    private float timer = 0;
    public override bool TryToFire(Gun gun, float fireRate)
    {
        // if starting to shoot wait for fire rate, otherwise your in a burst and wait for burst timer
        if (timer < (burstCounter == 0 ? fireRate + burstTimer : fireRate))
            timer += Time.deltaTime;
        else // ready to shoot
        {
            // if during a burst, update burst count
            if (burstCounter > 0)
            {
                if (burstCounter >= burstCount - 1)
                    burstCounter = 0;
                else
                    burstCounter++;

                timer = 0;
                return true;
            }
            // if first shot, start burst
            else if (gun.controls.isFiring)
            {
                burstCounter++;
                timer = 0;
                return true;
            }
        }
        return false;
    }

    public override void ResetGun(float fireRate)
    {
        burstCounter = 0;
        timer = fireRate + burstTimer;
    }

    public override string GetName() => "Burst";
}