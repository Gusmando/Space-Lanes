using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Gun currentGun;
    private bool reloading;
    public int reloadTime; 
    bool shooting;

    // Update is called once per frame
    void Update()
    {
        if(currentGun.canShoot && currentGun.clipSize!= 0 && currentGun.clipCount!= 0 && Input.GetMouseButtonDown(0))
        {
            shooting = true;
        }

        if(currentGun.clipSize == 0 && currentGun.clipCount!=0 && !reloading)
        {
            StartCoroutine(reloadDelay(reloadTime));
            currentGun.clipCount--;
        }   
    }

    void FixedUpdate() 
    {
        if(shooting)
        {
            currentGun.shoot();
            shooting = false;
        }
    }

    private IEnumerator reloadDelay(float delayLength)
    {
        reloading = true;
        yield return new WaitForSeconds(delayLength);
        currentGun.clipSize = currentGun.fullClip;
        reloading = false;
        yield return null;

    }

}
