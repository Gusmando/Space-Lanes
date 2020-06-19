using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public bool player;
    public Transform barrel;
    public Gun currentGun;
    public GameObject gunObj;
    public int currentGunIndex;
    public GameObject[] guns;
    public bool reloading;
    public float reloadTime; 
    public bool shooting;
    public bool shotAnim;
    public bool reload;
    public bool gunChange;
    public float animDelayTime;
    public CameraController camFunc;
    public bool input;
    void Start()
    {
        if(player)
        {
            gunObj = Instantiate(guns[currentGunIndex]);
            currentGun = gunObj.GetComponent<Gun>();
            currentGun.barrelLocation = this.barrel;
        }
        
    }
    void Update()
    {
        if(gunChange)
        {
            gunObj = Instantiate(guns[currentGunIndex]);
            currentGun = gunObj.GetComponent<Gun>();
            currentGun.barrelLocation = this.barrel;
            gunChange = false; 
        }

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
        
        if(currentGun.canShoot && currentGun.clipSize!= 0 && input && !reloading)
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
            if(currentGun.clipSize == 0 && currentGun.clipCount > 0 && !reload)
            {
                reload = true;
            }
            else
            {
                reload = false;
            }
        }

        if(reload && !reloading)
        {
            StartCoroutine(reloadDelay(reloadTime));
            currentGun.clipSize = currentGun.fullClip;
            currentGun.clipCount--;
        }

        if(currentGun.clipSize== 0 && currentGun.clipCount==0)
        {
            setGun(0);
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
    public void setGun(int gunNum) 
    {
        if(currentGunIndex != gunNum)
        {
            currentGunIndex = gunNum; 
            gunChange = true;
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
        reloading = false;
        yield return null;
    }
}
