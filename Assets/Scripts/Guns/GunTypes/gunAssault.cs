using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAssault : Gun
{
    
    private void Start() 
    {
        //Setting initial conditions
        canShoot = true;
        fullClip = clipSize;
    }
    
    public override void shoot()
    {
        //The assault rifle will only spawn one bullet at a time based
        //on an inspector-set barrel location transform
        GameObject arrow = Instantiate(bullet,barrelLocation.position , barrelLocation.rotation);

        //Using force on the rigid body to cause the bullet prefab to
        //projectile forward
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        arrow.GetComponent<Bullet>().damage = this.damage;
        rb.AddForce(barrelLocation.forward * bulletSpeed,ForceMode.Impulse);

        //Timer begins for the next shot, allowing for a change in fire rate
        StartCoroutine(shootingDelay(delayTime));
        clipSize--;
    }

}