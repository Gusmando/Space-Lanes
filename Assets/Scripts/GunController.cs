using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        fullClip = clipSize;
        
    }

    // Update is called once per frame
    void Update()
    {

        if(clipSize != 0)
        {
            if(canShoot)
            {
                GameObject arrow = Instantiate(bullet,barrelLocation.position , barrelLocation.rotation);

                Rigidbody rb = arrow.GetComponent<Rigidbody>();

                rb.AddForce(-1*barrelLocation.forward * bulletSpeed,ForceMode.Impulse);

                StartCoroutine(shootingDelay(delayTime));
                clipSize--;
            } 
        }

        if(clipSize == 0 && clipCount!=0 && !reloading)
        {
            StartCoroutine(reloadDelay(reloadTime));
            clipCount--;
        }   
    }


    private IEnumerator shootingDelay(float delayLength)
    {
        canShoot = false;

        yield return new WaitForSeconds(delayLength);

        canShoot = true;

        yield return null;
    }

    private IEnumerator reloadDelay(float delayLength)
    {
        reloading = true;
        yield return new WaitForSeconds(delayLength);
        clipSize = fullClip;
        reloading = false;

        yield return null;

    }

}
