using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class assaultMult : Pickup
{
    public override void action(GameObject player)
    {
        player.GetComponent<GunController>().setGun(1);
    }
}
