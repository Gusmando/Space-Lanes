using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAssault : Gun
{

    private void Start() 
    {
        auto = false;
        canShoot = true;
        fullClip = clipSize;
    }
    public override void shoot()
    { 
            GameObject arrow = Instantiate(bullet,barrelLocation.position , barrelLocation.rotation);

            Rigidbody rb = arrow.GetComponent<Rigidbody>();

            rb.AddForce(barrelLocation.forward * bulletSpeed,ForceMode.Impulse);

            StartCoroutine(shootingDelay(delayTime));
            clipSize--;
    }

}