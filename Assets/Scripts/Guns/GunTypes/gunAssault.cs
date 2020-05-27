using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAssault : Gun
{
    
    private void Start() 
    {
        canShoot = true;
        fullClip = clipSize;
    }
    public override void shoot()
    { 
            GameObject arrow = Instantiate(bullet,barrelLocation.position , barrelLocation.rotation);

            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            
            arrow.GetComponent<Bullet>().damage = this.damage;

            rb.AddForce(barrelLocation.forward * bulletSpeed,ForceMode.Impulse);

            StartCoroutine(shootingDelay(delayTime));
            clipSize--;
    }

}