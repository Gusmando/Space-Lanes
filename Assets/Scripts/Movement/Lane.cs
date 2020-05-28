using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will be used to define lanes
//giving them a rotation and position
//hazards can be added easily as well as terain options as well
public class Lane : MonoBehaviour
{
    public Transform LaneReferenceObject;
    public Vector3 position;
    public Vector3 rotation;
    public int minorEnemyCount;
    public int shootingEnemyCount;
    public int enemyLimit;

    // Start is called before the first frame update
    void Start()
    {
        position = LaneReferenceObject.position;
        rotation = LaneReferenceObject.eulerAngles;   
    }
}
