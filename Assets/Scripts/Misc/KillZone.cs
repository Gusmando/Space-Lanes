using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will be used to manage a collider which 
//destroys objects on collision
public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Any object but the Player
        if(other.gameObject.tag != "Player")
        {
            Destroy(other.gameObject);
        }
    }
}
