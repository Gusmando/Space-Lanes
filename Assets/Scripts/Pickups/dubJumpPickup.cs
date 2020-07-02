using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class dubJumpPickup : Pickup
{
    public float time;
    public override void action(GameObject player)
    {          
        player.GetComponent<MovementController>().dubJumpTime = time;
        player.GetComponent<MovementController>().dubJump = true;
    }
}
