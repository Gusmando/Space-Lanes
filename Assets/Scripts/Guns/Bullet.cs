using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool player;
    public bool distanceCap;
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
        if((distance >= range && distanceCap) || durability <= 0)
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

                if(other.gameObject != null && !other.gameObject.GetComponent<EnemyMovement>().hurt)
                {
                    Rigidbody enemy = other.gameObject.GetComponent<Rigidbody>();

                    if(!other.gameObject.GetComponent<EnemyMovement>().hurt)
                    {
                        other.gameObject.GetComponent<EnemyMovement>().hurtDelayStart();
                    }
                    enemy.AddForce(0,0,knockback, ForceMode.Impulse);
                    durability--;
                    StartCoroutine(colliderDelay(invHitTime));
                    bullet.GetComponent<Rigidbody>().AddForce(0,0,speed,ForceMode.Impulse);
                }
            }

            if(other.gameObject.CompareTag("Boss") && other.gameObject.GetComponent<bossComponent>().vulnerable)
            {
                other.gameObject.GetComponent<bossComponent>().health -= (damage * damageMod);

                if(other.gameObject != null && !other.gameObject.GetComponent<bossComponent>().hurt)
                {
                    if(!other.gameObject.GetComponent<EnemyMovement>().hurt)
                    {
                        other.gameObject.GetComponent<bossComponent>().hurtDelayStart();
                    }
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
                if(!other.gameObject.GetComponent<MovementController>().hurt)
                {
                    other.gameObject.GetComponent<MovementController>().health --;
                    other.gameObject.GetComponent<MovementController>().hurt = true;
                    other.gameObject.GetComponent<MovementController>().hurtTime = false;
                }
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
