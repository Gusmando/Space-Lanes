using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    public bool auto;
    public float bulletSpeed;
    public float delayTime;
    public Transform barrelLocation;
    public GameObject bullet;
    public Sprite gunSprite;
    public bool canShoot;
    public int clipSize;
    public int fullClip;
    public int clipCount;
    public float shakeIntensity;
    public float shakeDelay;
    public float damage;
    public abstract void shoot();

    protected IEnumerator shootingDelay(float delayLength)
    {
        canShoot = false;

        yield return new WaitForSeconds(delayLength);

        canShoot = true;

        yield return null;
    }
}
