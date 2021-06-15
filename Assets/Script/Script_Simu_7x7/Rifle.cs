using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Rifle : Weapon
{
    public AudioSource audioSoource;
    public AudioClip audioClip;

    [SerializeField] private Transform effectPosition;
    [SerializeField] private ParticleSystem fpsRifle;
    [SerializeField] private float fireRate;
    private Projectile projectile;

    private WaitForSeconds wait;

    protected override void Awake()
    {
        base.Awake();
        projectile = GetComponentInChildren<Projectile>();
    }

    private void Start()
    {
        wait = new WaitForSeconds(1 / fireRate);
        projectile.Init(this);
    }

    protected override void StartShooting(XRBaseInteractor interactor)
    {
        base.StartShooting(interactor);
        StartCoroutine(ShootingCO());
    }

    private IEnumerator ShootingCO()
    {
        while (true)
        {
            Shoot();
            yield return wait;
        }
    }

    protected override void Shoot()
    {
        base.Shoot();
        projectile.Launch();
        //ParticleSystem effect = Instantiate(fpsRifle);
        //effect.Stop();
        fpsRifle.Play();
        audioSoource.PlayOneShot(audioClip);
        

    }

    protected override void StopShooting(XRBaseInteractor interactor)
    {
        base.StopShooting(interactor);
        StopAllCoroutines();
    }
}
