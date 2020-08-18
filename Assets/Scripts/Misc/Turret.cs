using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject target;
    public GameObject bullet;
    public float bulletSpeed;
    public Transform barrelLocation;
    public int delayTime;
    private bool canShoot;
    private bool reloading;
    private int fullClip;
    public int clipSize;
    public int clipCount;
    public int reloadTime; 
    void Start()
    {
        //Initial Conditions
        canShoot = true;
        fullClip = clipSize;
    }

    void Update()
    {
        
        //Changing rotation of turret object based on the location
        //of the target object
        Vector3 direction = target.transform.position - transform.position;
        direction = Vector3.Scale(direction, new Vector3(-1,-1,1));
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = lookRotation.eulerAngles;
        transform.rotation = lookRotation;

        //If current clip is not empty
        if(clipSize != 0)
        {
            if(canShoot)
            {
                //Spawning a bullet prefab and applying force in the opposite direction
                //of this turret object
                GameObject arrow = Instantiate(bullet,barrelLocation.position , barrelLocation.rotation);
                Rigidbody rb = arrow.GetComponent<Rigidbody>();
                rb.AddForce(-1*barrelLocation.forward * bulletSpeed,ForceMode.Impulse);
                StartCoroutine(shootingDelay(delayTime));
                clipSize--;
            } 
        }

        //Automatically reload if more ammo is available
        //and current clip is empty
        if(clipSize == 0 && clipCount!=0 && !reloading)
        {
            reloading = true;
            StartCoroutine(reloadDelay(reloadTime));
            clipCount--;
        }   
    }

    //Delays for time between bullet and reload time
    private IEnumerator shootingDelay(float delayLength)
    {
        canShoot = false;
        yield return new WaitForSeconds(delayLength);
        canShoot = true;
        yield return null;
    }
    private IEnumerator reloadDelay(float delayLength)
    {
        yield return new WaitForSeconds(delayLength);
        clipSize = fullClip;
        reloading = false;
    }

}
