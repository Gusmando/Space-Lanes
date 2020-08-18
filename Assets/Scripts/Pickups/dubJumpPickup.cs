using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class dubJumpPickup : Pickup
{
    public float time;

    //The dubjump Pickup will set the players 
    //souble jump booleans to true and set the timer
    public override void action(GameObject player)
    {          
        player.GetComponent<MovementController>().dubJumpTime = time;
        player.GetComponent<MovementController>().dubJump = true;
    }
}
