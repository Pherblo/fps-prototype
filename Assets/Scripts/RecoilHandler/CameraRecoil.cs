using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bumps the camera around randomly the more you shoot consecutively
public class CameraRecoil : RecoilHandler
{
    [Header("Recoil")]
    [Tooltip("How much the gun can rotate vertically during recoil")]
    public float verticalRecoil = 7f;
    [Tooltip("How much the gun can rotate horizontally during recoil")]
    public float horizontalRecoil = 2f;
    [Tooltip("How much the gun rotates vertically over time during recoil")]
    public AnimationCurve verticalOverTime = AnimationCurve.Linear(0, 0, 1, 1);
    [Tooltip("How much the gun rotates horizontally over time during recoil")]
    public AnimationCurve horizontalOverTime = AnimationCurve.Linear(0, .3f, 1, 1);

    [Header("Heat")]
    [Tooltip("How much heat can be stored")]
    public int maxHeat = 10;
    [Tooltip("How long you have to wait after firing before the heat will decrease")]
    public float cooldownDelay = 0.2f;
    [Tooltip("How much heat will get reduced during a cooldown")]
    public float cooldownRate = 25f;

    private float cooldown = 0f;
    private float heat = 0f;

    [Header("Camera Movement")]
    [Tooltip("Transform to rotate for recoil effects")]
    public Transform camRoot;
    [Tooltip("How fast the camera jolts during recoil")]
    public float recoilSpeed = 1f;
    [Tooltip("How fast the camera will steady to its ready position after recoil")]
    public float steadySpeed = 0.05f;

    private Vector3 targetRot;

    private void Update()
    {
        // wait for cooldown perdiod
        if (cooldown < cooldownDelay)
            cooldown += Time.deltaTime;
        else if (heat > 0)
        {
            // reset camera rotation to 0
            targetRot = Vector3.zero;

            // decrease heat
            heat -= Time.deltaTime * cooldownRate;
            heat = Mathf.Max(heat, 0);
        }

        // smooth lerp cameras rotation to target
        camRoot.localRotation = Quaternion.Slerp(camRoot.localRotation, Quaternion.Euler(targetRot), cooldown < cooldownDelay ? recoilSpeed : steadySpeed);
    }

    public override void AddRecoil()
    {
        // add heat and reset cooldown
        if (heat < maxHeat)
            heat++;
        cooldown = 0;

        // rotate the camera to random values increasing with higher heat values
        targetRot = new Vector3((float)verticalOverTime.Evaluate(heat/maxHeat) * -verticalRecoil, 
            horizontalRecoil * (Random.Range(-1f, 1f)) * horizontalOverTime.Evaluate(heat / maxHeat), 0);
    }

    public override Vector3 GetProjectileDirectionOffset()
    {
        // return nothing because recoil is handled thorugh camera rotation
        return Vector3.zero;
    }

    public override float GetMaximumSpread()
    {
        // calculate both the max horizontal and vertical spread at this heat level and return the highest
        return Mathf.Max((float)verticalOverTime.Evaluate(heat / maxHeat) * verticalRecoil,
            horizontalRecoil * horizontalOverTime.Evaluate(heat / maxHeat));
    }
}
