﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    private bool chasing;
    public float distanceToChase=10f,distanceToLose=15f,distanceToStop=2f;

    private Vector3 targetPoint;
    public NavMeshAgent agent;

    private Vector3 startPoint;

    public GameObject bullet;
    public Transform firePoint;

    public float fireRate,waitBetweenShots, timeToShoot;
    private float fireCount, shotWaitCounter, shootTimeCounter;

    public float keepChasingTime = 5f;
    private float chaseCounter;

    public Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;
        if (!chasing)
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
                fireCount = 1f;
            }
            if (chaseCounter > 0) {
                chaseCounter -= Time.deltaTime;
                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }
            anim.SetBool("isMoving",true);
        }
        else
        {
            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop) { 
                agent.destination = targetPoint;
            }
            else
            {
                agent.destination = transform.position;
            }

            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;
                chaseCounter = keepChasingTime;
            }
            fireCount -= Time.deltaTime;
            if (fireCount <= 0)
            {
                fireCount = fireRate;
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                anim.SetTrigger("fireShot");
            }
            anim.SetBool("isMoving", false);
        }
        
    }
}
