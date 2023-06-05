
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Increases the guns spread over time. Doesnt directly effect camera, but you can use a cinemachine affect to achieve something basic
public class RndSpreadRecoil : RecoilHandler
{
    [Header("Spread")]
    [Tooltip("Minimum amount of spread on all shots")]
    public float minSpread = 0f;
    [Tooltip("Maximum amount of spread without additional recoil from heat")]
    public float maxSpread = 0.03f;

    [Space]
    [Tooltip("How much spread will be added when fully heated")]
    public float additionalSpreadWhenHeated = 0.1f;
    [Tooltip("A graph of how much spread increases over increased heat")]
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

    private void Update()
    {
        // wait for cooldown period
        if (cooldown < cooldownDelay)
            cooldown += Time.deltaTime;
        // then start reducing heat over time
        else if(heat > 0)
            heat -= Time.deltaTime * cooldownRate;
    }

    public override void AddRecoil()
    {
        // add heat and reset cooldown
        if (heat < maxHeat) 
            heat++;
        cooldown = 0;
    }

    public override Vector3 GetProjectileDirectionOffset()
    {
        // return a random point inside a donut shape, using the min and max spread vales
        return PointOnCircle(Random.Range(minSpread, GetMaximumSpread()), Random.Range(1, 360));
    }

    public override float GetMaximumSpread()
    {
        // calculate the max spread potential for this heat level
        return maxSpread + spreadOverTime.Evaluate(heat / maxHeat) * additionalSpreadWhenHeated;
    }
}
