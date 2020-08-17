using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("Assignments")]
    public bool player;
    public Transform barrel;
    public GameObject gunObj;
    public CameraController camFunc;
    public GameObject[] guns;
    public float animDelayTime;

    [Header("Current Gun")]
    public Gun currentGun;
    public int currentGunIndex;
    public float reloadTime;

    [Header("State Variables")]
    public bool reloading;
    public bool shooting;
    public bool shotAnim;
    public bool reload;
    public bool gunChange;
    public bool input;

    void Start()
    {
        //Whether or not gun controller is used for player
        //object is specified within inspector
        if(player)
        {
            //Instantiating current gun from full gun array
            //and making apt assignments
            gunObj = Instantiate(guns[currentGunIndex]);
            currentGun = gunObj.GetComponent<Gun>();
            currentGun.barrelLocation = this.barrel;
        }
        
    }
    void Update()
    {
        //Gun change boolean trigger will cause instantiation
        //of the newly assigned gun; apt assignments follow
        if(gunChange)
        {
            gunObj = Instantiate(guns[currentGunIndex]);
            currentGun = gunObj.GetComponent<Gun>();
            currentGun.barrelLocation = this.barrel;
            gunChange = false; 
        }

        //Input is handled by mouse button when gun controller
        //assigned to player object ; automatic weapons handled
        //differently as well
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
        
        //If the weapon is being shot start animation timers and vars set
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

        //Player will reload automatically when the current
        //clip count reaches 0
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

        //Reloading timers start and clip is filled
        if(reload && !reloading)
        {
            StartCoroutine(reloadDelay(reloadTime));
            currentGun.clipSize = currentGun.fullClip;
            currentGun.clipCount--;
        }

        //When gun runs out of ammo, default gun is 
        //set
        if(currentGun.clipSize== 0 && currentGun.clipCount==0)
        {
            setGun(0);
        }

        //Setting screen shake settings
        if(player)
        {
            camFunc.setShakeIntensity(currentGun.shakeIntensity);
            camFunc.setShakeLength(currentGun.shakeDelay);
        }

        
    }

    void FixedUpdate() 
    {
        //When weapon is shooting, fire weapon 
        //and shake camera function implementation
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

    //Method used to set a new weapon
    public void setGun(int gunNum) 
    {
        if(currentGunIndex != gunNum)
        {
            currentGunIndex = gunNum; 
            gunChange = true;
        }
    }

    //Delays used to create animation state
    //pauses and reload timer for weapon
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
