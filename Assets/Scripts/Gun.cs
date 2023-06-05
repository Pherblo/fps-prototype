using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    public ProjectileShooter projectileShooter;

    public GunSights sights;

    public Animator animator;
    public InputManager controls;

    [Header("Stats")]
    public float fireRate = 0.3f;

    [Space]
    public FireMode[] fireModes = new FireMode[1];
    private int currentFireMode = 0;

    [Header("Reloading")]
    public int magazineCapacity;
    public int magazineContents;

    public bool autoReload = true;
    private bool isReloading = false;

    public UnityEvent onStartReload;
    public UnityEvent onFinishReload;

    void Awake()
    {
        magazineContents = magazineCapacity;

        controls.onReload += StartReload;
        controls.onSwitchFiremode += ToggleFireMode;
    }

    public void ToggleFireMode()
    {
        currentFireMode = currentFireMode == fireModes.Length - 1 ? 0 : currentFireMode + 1;
    }

    public FireMode GetCurrentFireMode() => fireModes[currentFireMode];

    private void Fire()
    {
        if (magazineContents == 0)
        {
            if(autoReload) 
                StartReload();
            return;
        }

        projectileShooter?.Fire();
        magazineContents--;
    }

    public void StartReload()
    {
        if (isReloading || magazineContents >= magazineCapacity) return;

        onStartReload.Invoke();
        animator.SetTrigger("Reload");
        isReloading = true;
    }

    public void FinishReload()
    {
        onFinishReload.Invoke();
        fireModes[currentFireMode].ResetGun(fireRate);
        magazineContents = magazineCapacity;
        isReloading = false;
    }

    void Update()
    {
        if (sights)
            sights.UpdatePosition(controls, isReloading);

        if (isReloading) return;

        if (fireModes[currentFireMode].TryToFire(this, fireRate))
            Fire();
    }
}
