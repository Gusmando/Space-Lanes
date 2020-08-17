using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAssault3Lane : Gun
{
    private void Start() 
    {
        //Initial Conditions
        canShoot = true;
        fullClip = clipSize;
    }
    public override void shoot()
    { 
        //Using a for loop to spawn bullets as the process is identical
        //only changing the angle based on the amount of bullets 
        for(int i = 0; i < 3; i++)
        {
            GameObject arrow = Instantiate(bullet, barrelLocation.position, barrelLocation.rotation);

            //Angle of transform changes to create fan shaped projectile path
            if(i == 1)
            {
                arrow.transform.eulerAngles = new Vector3(0,17,0);
            }
            else if(i == 2)
            {
                arrow.transform.eulerAngles = new Vector3(0,-17,0);
            }

            //Adding force to the bullet each loop
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            arrow.GetComponent<Bullet>().damage = this.damage;
            rb.AddForce(arrow.transform.forward * bulletSpeed,ForceMode.Impulse);
        }

        StartCoroutine(shootingDelay(delayTime));
        clipSize--;
    }

}
