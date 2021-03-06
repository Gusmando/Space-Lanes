﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAnimationController : MonoBehaviour
{
    [Header("Player Animation Controller")]
    public Animator playerAnim;
    public SpriteRenderer spriteDisp;
    public int animStateDisplay;

    [Header("Gun Object")]
    public GunController gunController;
    public Transform gunTrans;
    public Vector3 initLocation;
    [Header("Parent Rotation Object")]
    public Transform currentTrans;

    [Header("Asjustments")]
    public int angleLeft;
    public int angleRight;
    public float shootAngle;
    public float time;
    public float leftDashOffset;
    public float rightDashOffset;
    public float jumpOffset;

    // Update is called once per frame
    void Start() 
    {
        //spriteDisp.sprite = gunController.currentGun.gunSprite;
        initLocation = gunTrans.localPosition;
    }
    
    void FixedUpdate()
    {
        int animStateDisp = playerAnim.GetInteger("animState");
        if(playerAnim.GetBool("hurt"))
        {
            animStateDisp = 111;
        }
        switch(animStateDisp)
        {
            case 0:
                gunTrans.localPosition = initLocation;
                AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
                time = stateInfo.normalizedTime % 1; 

                if(time >= 0 && time < .25)
                {

                    gunTrans.eulerAngles = currentTrans.eulerAngles + new Vector3(0,0,angleLeft);
                    break;
                }

                else if(time >= .50 && time < .75)
                {
                    gunTrans.eulerAngles = currentTrans.eulerAngles + new Vector3(0,0,angleRight);
                    break;
                }

                else
                {
                    gunTrans.eulerAngles = currentTrans.eulerAngles;
                    break;
                }  
            case 01:
                gunTrans.localPosition = initLocation + new Vector3(rightDashOffset,0,0);
            break;
            case 10:
                gunTrans.localPosition = initLocation + new Vector3(leftDashOffset,0,0);
            break;
            case 100:
                gunTrans.localPosition = initLocation;
            break;
            case 111:
                gunTrans.localPosition = initLocation + new Vector3(0,jumpOffset,0);
            break;
        }

        if(gunController.shotAnim)
        {
            Vector3 temp  = gunTrans.localEulerAngles;
            temp.y = shootAngle;
            gunTrans.localEulerAngles = temp;
        }
    }
}
