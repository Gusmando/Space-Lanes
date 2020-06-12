using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lobGun : Gun
{
    public GameObject target;
    private void Start() 
    {
        canShoot = true;
        fullClip = clipSize;
        auto = true;
        target = GameObject.FindWithTag("Player");
    }
    public override void shoot()
    { 
        GameObject arrow = Instantiate(bullet, barrelLocation.position, barrelLocation.rotation);
        arrow.transform.eulerAngles = new Vector3(4*clipSize,0,0);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        arrow.GetComponent<Bullet>().damage = this.damage;
        rb.AddForce(arrow.transform.forward * bulletSpeed,ForceMode.Impulse);
        StartCoroutine(shootingDelay(delayTime));
        clipSize--;
    }

}
