using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Assignments")]
    public GameObject bullet;
    public bool player;
    public bool distanceCap;

    [Header("Bullet Settings")]
    public int durability;
    public float damage;
    public float damageMod;
    public float knockback;
    public float speed;
    public float range;
    public float invHitTime;

    [Header("State Variables")]
    public bool canHit;
    private Vector3 initPosition;

    void Start()
    {
        //Initial Settings
        initPosition = bullet.transform.position;
        canHit = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Distance is tracked for checking range
        float distance = Vector3.Distance(initPosition,bullet.transform.position);
        
        //Bullets destroyed with out of range
        if((distance >= range && distanceCap) || durability <= 0)
        {
            Destroy(bullet);
        }
    }

    void FixedUpdate() 
    {
        //Bullets have a delay to hit multiple times which is why
        //the boolean must be checked
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
        //Another bullet causes both bullets to 
        //lose durability
        if(other.gameObject.CompareTag("Bullet"))
        {
            durability--;
            other.gameObject.GetComponent<Bullet>().durability --;
        }

        //If the bullet us a player bullet
        if(player)
        {
            //Bullet hitting an enemy 
            if(other.gameObject.CompareTag("Enemy"))
            {
                //Subtracting damage from the enemies total health
                other.gameObject.GetComponent<EnemyMovement>().health -= (damage * damageMod);

                //If the Enemy still exists
                if(other.gameObject != null && !other.gameObject.GetComponent<EnemyMovement>().hurt)
                {
                    //Rigid Body Assignment
                    Rigidbody enemy = other.gameObject.GetComponent<Rigidbody>();

                    //Starting the hit time on enemy/Knocking Back
                    if(!other.gameObject.GetComponent<EnemyMovement>().hurt)
                    {
                        other.gameObject.GetComponent<EnemyMovement>().hurtDelayStart();
                    }
                    enemy.AddForce(0,0,knockback, ForceMode.Impulse);
                    durability--;

                    //Disabling collider and applying speed to bullet
                    StartCoroutine(colliderDelay(invHitTime));
                    bullet.GetComponent<Rigidbody>().AddForce(0,0,speed,ForceMode.Impulse);
                }
            }
            //If collision with boss enemy
            if(other.gameObject.CompareTag("Boss") && other.gameObject.GetComponent<bossComponent>().vulnerable && !other.gameObject.GetComponent<bossComponent>().hurt)
            {
                //Subtract Health
                other.gameObject.GetComponent<bossComponent>().health -= (damage * damageMod);
                
                //Collision and Hurt
                if(other.gameObject != null && !other.gameObject.GetComponent<bossComponent>().hurt)
                {
                    if(!other.gameObject.GetComponent<bossComponent>().hurt)
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
            //Collision with player object
            if(other.gameObject.CompareTag("Player"))
            {
                //Set player state variables
                if(!other.gameObject.GetComponent<MovementController>().hurt)
                {
                    other.gameObject.GetComponent<MovementController>().health --;
                    other.gameObject.GetComponent<MovementController>().hurt = true;
                    other.gameObject.GetComponent<MovementController>().hurtTime = false;
                }
                //If enemy not destroyed, bullet advances, kniockback, durabality subtraction
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

    //Delay for collider
    IEnumerator colliderDelay(float delayLength)
    {
        canHit = false;

        yield return new WaitForSeconds(delayLength);

        canHit = true;

        yield return null;
    }
}
