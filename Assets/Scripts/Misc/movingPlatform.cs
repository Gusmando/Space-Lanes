using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class movingPlatform : MonoBehaviour
{
    //Initialiting variables, which can be changed
    //at any time for different objects
    public float xSpeed;
    public float ySpeed;
    public float zSpeed;
    //Start Position and positionchanged
    //will be used for bounds checking
    private Vector3 startPosition;
    public Vector3 positionChange;

    //All bounds can be changed and each 
    //axis movement can be turned on and off
    public float upperBoundx;
    public float upperBoundy;
    public float upperBoundz;
    public bool xMovement;
    public bool yMovement;
    public bool zMovement;
    public bool returning;

    // Start is called before the first frame update
    void Start()
    {
        //Saving for future reference
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Calculates position change
        positionChange.x = Math.Abs(startPosition.x - transform.position.x);
        positionChange.y = Math.Abs(startPosition.y - transform.position.y);
        positionChange.z = Math.Abs(startPosition.z - transform.position.z);

        //Checking the bounds of each axis
        if((positionChange.x >= upperBoundx && xMovement)
        || (positionChange.y >= upperBoundy && yMovement)
        || (positionChange.z >= upperBoundz && zMovement))
        {
            returning = true;
        }
        
        //If any of the movement are turned on
        if(xMovement || yMovement || zMovement)
        {
            //Nudging towards the boundary
            if(!returning && positionChange.x < upperBoundx && positionChange.y < upperBoundy && positionChange.z < upperBoundz)
            {
                transform.position += new Vector3(xSpeed * Time.deltaTime * System.Convert.ToSingle(xMovement), 
                ySpeed * Time.deltaTime * System.Convert.ToSingle(yMovement),
                zSpeed * Time.deltaTime * System.Convert.ToSingle(zMovement));
            }

            //Coming back from boundary
            else if(returning)
            {
                transform.position += new Vector3(-xSpeed * Time.deltaTime * System.Convert.ToSingle(xMovement), 
                -ySpeed * Time.deltaTime * System.Convert.ToSingle(yMovement),
                -zSpeed * Time.deltaTime * System.Convert.ToSingle(zMovement));
                
                //End of returning when hitting a small buffer
                if((positionChange.x < 0.5 && xMovement) || (positionChange.y < 0.5 && yMovement) || (positionChange.z < 0.5 && zMovement))
                {
                    returning = false;
                }
            }
        }
    }
}
