﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool player;
    public int durability;
    public GameObject bullet;
    public float damage;
    public float damageMod;
    public float knockback;
    public float speed;
    public float range;
    public bool canHit;
    public float invHitTime;
    private Vector3 initPosition;
    void Start()
    {
        initPosition = bullet.transform.position;
        canHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(initPosition,bullet.transform.position);
        if(distance >= range || durability <= 0)
        {
            Destroy(bullet);
        }
    }

    void FixedUpdate() 
    {
        if(canHit)
        {
            bullet.GetComponent<SphereCollider>().enabled = true;
        }
        else
        {
            bullet.GetComponent<SphereCollider>().enabled = false;
        }
    }

     private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Bullet"))
        {
            durability--;
            other.gameObject.GetComponent<Bullet>().durability --;
        }
        if(player)
        {
            if(other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyMovement>().health -= (damage * damageMod);

                if(other.gameObject != null)
                {
                    Rigidbody enemy = other.gameObject.GetComponent<Rigidbody>();
                    enemy.AddForce(0,0,knockback, ForceMode.Impulse);
                    durability--;
                    StartCoroutine(colliderDelay(invHitTime));
                    bullet.GetComponent<Rigidbody>().AddForce(0,0,speed,ForceMode.Impulse);
                }
            }
        }
        else
        {
            if(other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<MovementController>().health -= (damage * damageMod);
                if(other.gameObject != null)
                {
                    Rigidbody playerRB = other.gameObject.GetComponent<Rigidbody>();
                    playerRB.AddForce(0,0,knockback, ForceMode.Impulse);
                    durability--;
                    StartCoroutine(colliderDelay(invHitTime));
                    bullet.GetComponent<Rigidbody>().AddForce(0,0,speed,ForceMode.Impulse);
                }
            }
        }
    }

    IEnumerator colliderDelay(float delayLength)
    {
        canHit = false;

        yield return new WaitForSeconds(delayLength);

        canHit = true;

        yield return null;
    }
}