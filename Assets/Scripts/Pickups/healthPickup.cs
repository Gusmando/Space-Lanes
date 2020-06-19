using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickup : Pickup
{
    public override void action(GameObject player)
    {
        player.GetComponent<MovementController>().health ++;
    }
}
