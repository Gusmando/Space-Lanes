using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assaultMult : Pickup
{
    //The multiple assault class action will simply set the
    //players weapon to the multiple assault variant
    public override void action(GameObject player)
    {
        player.GetComponent<MovementController>().weapon.setGun(1);
    }
}
