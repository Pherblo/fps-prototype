using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternedRecoil : RecoilHandler
{
    [Header("Recoil")]
    [Tooltip("How much the gun can rotate vertically during recoil")]
    public float verticalRecoil = 7f;
    [Tooltip("How much the gun can rotate horizontally during recoil")]
    public float horizontalRecoil = 2f;
    [Tooltip("How much the gun rotates vertically over time during recoil")]
    public AnimationCurve verticalOverTime = AnimationCurve.Linear(0, 0, 1, 1);
    [Tooltip("How much the gun rotates horizontally over time during recoil")]
    public AnimationCurve horizontalRecoilPattern = AnimationCurve.Linear(0, 0, 1, 1);

    [Header("Spread")]
    public float minSpread = 0f;
    public float maxSpread = 0.02f;
    [Space]
    public float additionalSpreadWhenHeated = 0.03f;
    public AnimationCurve spreadOverTime = AnimationCurve.Linear(0, 0, 1, 1);

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

        // calculate horizontal recoil based off pattern, but then become random when at max heat
        float horizontal = heat >= maxHeat ? horizontalRecoil * Random.Range(-1f, 1f) 
            : horizontalRecoilPattern.Evaluate(heat / maxHeat) * horizontalRecoil;

        // calculate vertical recoil then find the final target rotation
        targetRot = new Vector3((float)verticalOverTime.Evaluate(heat / maxHeat) * -verticalRecoil, horizontal, 0);
    }

    public override Vector3 GetProjectileDirectionOffset()
    {
        // return a random point inside a donut shape, using the min and max spread vales
        return PointOnCircle(Random.Range(minSpread, GetMaxOffsetSpread()), Random.Range(1, 360));
    }

    public override float GetMaximumSpread()
    {
        // calculate both the max horizontal and vertical spread at this heat level and return the highest
        return Mathf.Max((float)verticalOverTime.Evaluate(heat / maxHeat) * verticalRecoil,
            horizontalRecoil * horizontalRecoilPattern.Evaluate(heat / maxHeat))
            + GetMaxOffsetSpread();
    }

    // used for getting max potention directional offset spread for this heat level. ignores camera recoil
    private float GetMaxOffsetSpread()
    {
        return maxSpread + spreadOverTime.Evaluate(heat / maxHeat) * additionalSpreadWhenHeated;
    }
}
