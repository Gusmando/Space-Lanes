using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will be used to define lanes
//giving them a rotation and position.
public class Lane : MonoBehaviour
{
    [Header("Assignments")]
    public Transform LaneReferenceObject;

    [Header("State Variables")]
    public Vector3 position;
    public Quaternion rotation;
    public int minorEnemyCount;
    public int shootingEnemyCount;
    public int lobbingEnemyCount;

    // Start is called before the first frame update
    void Start()
    {   
        //Assigning vartiables to be referenced in other scripts
        LaneReferenceObject = this.transform.GetChild(0);
        position = LaneReferenceObject.position;
        rotation = LaneReferenceObject.rotation;   
    }
}
