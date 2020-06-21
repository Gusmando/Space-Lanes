using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    public GameObject sprite;
    public float angleSpeed;
    public abstract void action(GameObject Player);
    private void Update() 
    {
        sprite.transform.RotateAround(sprite.transform.position,sprite.transform.up,angleSpeed);    
    }
    private void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player")
        {
            action(other.gameObject);
            Destroy(gameObject);
        }
    }
}
