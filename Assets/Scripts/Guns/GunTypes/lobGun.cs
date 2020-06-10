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
        Vector3 directionEdit = new Vector3 (barrelLocation.position.x, target.transform.position.y,target.transform.position.z); 
        Vector3 direction = directionEdit - barrelLocation.position;
        direction = Vector3.Scale(direction, new Vector3(-1,-1,1));
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = lookRotation.eulerAngles;
        barrelLocation.eulerAngles = rotation;
        GameObject arrow = Instantiate(bullet,barrelLocation.position , barrelLocation.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        arrow.GetComponent<Bullet>().damage = this.damage;
        Vector3 temp = barrelLocation.eulerAngles;
        barrelLocation.eulerAngles = new Vector3(-10,-temp.y,-temp.z);
        rb.AddForce(barrelLocation.forward * (-1*bulletSpeed),ForceMode.Impulse);
        StartCoroutine(shootingDelay(delayTime));
        clipSize--;
    }

}
