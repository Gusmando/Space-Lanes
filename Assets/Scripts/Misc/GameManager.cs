using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    void Awake()
    {
        Physics.IgnoreLayerCollision(9,9,true);
    }

    
    void Update()
    {
        
    }
}
