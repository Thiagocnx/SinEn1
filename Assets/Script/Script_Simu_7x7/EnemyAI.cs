﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour, ITakeDamage
{
    const string DIED_TRIGGER = "Died";
    const string IDLE_TRIGGER = "Idle";
    const string SHOOT_TRIGGER = "Shoot";

    [SerializeField] private float startingHealth;
    [SerializeField] private float timeToShoot;
    //[SerializeField] private float maxTimeUnderCover;
    //[SerializeField] private int minShotsToTake;
    //[SerializeField] private int maxShotsToTake;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float damage;
    [Range(0, 100)]
    [SerializeField] private float shootingAccuracy;

    [SerializeField] private Transform shootingPosition;
    [SerializeField] private ParticleSystem bloodSplatterFX;
    [SerializeField] private ParticleSystem fpsPistol;

    private bool isShooting;
    //private int currentShotsTaken;
    //private int currentMaxShotsToTake;
    //private NavMeshAgent agent;
    //private Transform occupiedCoverSpot;
    private Animator animator;
    private bool rotation = true;

    public float maxDistance;
    public Player player;
    public ActionController actionController;
    //public ActionControllerRoon1 controllerRoon1;
    //public ActionControllerRoon2 controllerRoon2;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject pistol;
    public AudioSource audioSourcePistol;
    public AudioClip audioClipPistol;

    private float _health;
    public float health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = Mathf.Clamp(value, 0, startingHealth);
        }
    }

    private void Awake()
    {
        
        animator = GetComponent<Animator>();
        //agent = GetComponent<NavMeshAgent>();
        animator.SetTrigger(IDLE_TRIGGER);
        _health = startingHealth;
        
    }

    private void Start()
    {
         //pistol = GetComponentInParent<GameObject>();
    }

    //public void Init(Player player, Transform coverSpot)
    //{
    //    occupiedCoverSpot = coverSpot;
    //    this.player = player;
    //    GetToCover();
    //}

    //private void GetToCover()
    //{
    //    agent.isStopped = false;
    //    agent.SetDestination(occupiedCoverSpot.position);
    //}

    private void Update()
    {
        if (rotation)
        {
            RotateTowardsPlayer();
        }
        Vector3 orientation = player.GetHeadPosition() - transform.position;

        bool action = actionController.action;
        if (orientation.sqrMagnitude <= 16.0f && action) // Physics.Raycast(transform.position,orientation,maxDistance)
        {  
            StartShooting();   
            //StartCoroutine(InitializeShootingCO());
        }

        //bool action1 = controllerRoon1.action;
        //if (orientation.sqrMagnitude <= 16.0f && action1) // Physics.Raycast(transform.position,orientation,maxDistance)
        //{
        //    StartShooting();
        //StartCoroutine(InitializeShootingCO());
        //}

        //bool action2 = controllerRoon2.action;
        //if (orientation.sqrMagnitude <= 36.0f && action2) // Physics.Raycast(transform.position,orientation,maxDistance)
        //{
        //    StartShooting();
            //StartCoroutine(InitializeShootingCO());
        //}
    }
    private IEnumerator InitializeShootingCO()
    {
        //HideBehindCover();
        yield return new WaitForSeconds(timeToShoot);
        StartShooting();
    }


    private void Died()
    {
        audioSource.enabled = false;
        rotation = false;
        animator.SetTrigger(DIED_TRIGGER);
    }
    private void StartShooting()
    {
        //isShooting = true;
        //currentMaxShotsToTake = UnityEngine.Random.Range(minShotsToTake, maxShotsToTake);
        //currentShotsTaken = 0;
        animator.SetTrigger(SHOOT_TRIGGER);
    }

    public void Shoot()
    {
        bool hitPlayer = UnityEngine.Random.Range(0, 100) < shootingAccuracy;

        if (hitPlayer)
        {
            RaycastHit hit;
            Vector3 direction = player.GetHeadPosition() - shootingPosition.position;
            if(Physics.Raycast(shootingPosition.position, direction, out hit))
            {
                Player player = hit.collider.GetComponentInParent<Player>();
                if (player)
                {
                    fpsPistol.Play();
                    player.TakeDamage(damage);
                    audioSourcePistol.PlayOneShot(audioClipPistol);
                    
                }
            }
        }
        //currentShotsTaken++;
        //if(currentShotsTaken >= currentMaxShotsToTake)
        //{
        //    StartCoroutine(InitializeShootingCO());
        //}
    }

    public void RotateTowardsPlayer()
    {
        Vector3 direction = player.GetHeadPosition() - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.rotation = rotation;
    } 

    public void TakeDamage(Weapon weapon, Projectile projectile, Vector3 contactPoint)
    {
        health -= weapon.GetDamage();
        audioSource.PlayOneShot(audioClip);
        if (health <= 0)
            Died();
        ParticleSystem effect = Instantiate(bloodSplatterFX, contactPoint, Quaternion.LookRotation(weapon.transform.position - contactPoint));
        effect.Stop();
        effect.Play();
    }

    public void EnablePistol()
    {
        pistol.gameObject.SetActive(true);
    }


}
