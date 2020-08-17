using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobGun : Gun
{
    public GameObject target;
    private void Start() 
    {
        //Initial Conditions
        canShoot = true;
        fullClip = clipSize;
        auto = true;
        target = GameObject.FindWithTag("Player");
    }
    public override void shoot()
    { 
        //The bullet is instantiated 
        GameObject arrow = Instantiate(bullet, barrelLocation.position, barrelLocation.rotation);

        //As the clip size moves along a small adjustment to the angle
        //allows for a lobbed projectile to be possible
        arrow.transform.eulerAngles = new Vector3(4*clipSize, 0, 0);

        //Applying forces to the bullet
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        arrow.GetComponent<Bullet>().damage = this.damage;
        rb.AddForce(arrow.transform.forward * bulletSpeed,ForceMode.Impulse);
        
        StartCoroutine(shootingDelay(delayTime));
        clipSize--;
    }

}
