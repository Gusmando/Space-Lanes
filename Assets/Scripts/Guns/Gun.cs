using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [Header("Assignments")]
    public Transform barrelLocation;
    public GameObject bullet;
    public Sprite gunSprite;
    public Color gunColor;

    [Header("Gun Specs")]
    public bool auto;
    public float bulletSpeed;
    public float delayTime;
    public int fullClip;
    public float shakeIntensity;
    public float shakeDelay;
    public float damage;

    [Header("State Vars")]
    public bool canShoot;
    public int clipSize;
    public int clipCount;

    //This abstract function will work to define
    //a unique shooting pattern for each gun type.
    public abstract void shoot();

    //Delay for time between each gun shot
    protected IEnumerator shootingDelay(float delayLength)
    {
        canShoot = false;

        yield return new WaitForSeconds(delayLength);

        canShoot = true;

        yield return null;
    }
}
