using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickup : Pickup
{
    public override void action(GameObject player)
    {
        if(player.GetComponent<MovementController>().health < 5)
        {
            player.GetComponent<MovementController>().health ++;
        }
    }
}
