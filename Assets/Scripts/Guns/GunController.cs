﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Gun currentGun;
    private bool reloading;
    public int reloadTime; 
    public bool shooting;
    public bool shotAnim;
    public float animDelayTime;
    bool input;
    // Update is called once per frame
    void Update()
    {

        if(currentGun.auto)
        {
            input = Input.GetMouseButton(0);

            if(!input)
            {
                shotAnim = false;
            }
        }
        else
        {
            input = Input.GetMouseButtonDown(0);
        }
        
        if(currentGun.canShoot && currentGun.clipSize!= 0 && currentGun.clipCount!= 0 && input)
        {
            if(!currentGun.auto)
            {
                StartCoroutine(shotAnimDelay(animDelayTime));
            }
            else
            {
                shotAnim = true;
            }
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

    private IEnumerator shotAnimDelay(float delayLength)
    {
        shotAnim = true;

        yield return new WaitForSeconds(delayLength);

        shotAnim = false;

        yield return null;
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