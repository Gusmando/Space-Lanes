using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public bool player;
    public Gun currentGun;
    public bool reloading;
    public float reloadTime; 
    public bool shooting;
    public bool shotAnim;
    public bool reload;
    public float animDelayTime;
    public CameraController camFunc;
    public bool input;
    // Update is called once per frame
    void Update()
    {
        if(player)
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

        if(player)
        {
            reload = Input.GetMouseButtonDown(1);
        }

        if( reload && !reloading)
        {
            StartCoroutine(reloadDelay(reloadTime));
            currentGun.clipCount--;
        }   
        if(player)
        {
            camFunc.setShakeIntensity(currentGun.shakeIntensity);
            camFunc.setShakeLength(currentGun.shakeDelay);
        }
    }

    void FixedUpdate() 
    {
        if(shooting)
        {
            currentGun.shoot();
            if(player)
            {
                camFunc.screenShake();
            }
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
