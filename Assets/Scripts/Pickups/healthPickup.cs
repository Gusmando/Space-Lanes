using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickup : Pickup
{
    //The health pickup will simply add to the 1 to
    //the player's health
    public override void action(GameObject player)
    {
        if(player.GetComponent<MovementController>().health < 5)
        {
            player.GetComponent<MovementController>().health ++;
        }
    }
}
