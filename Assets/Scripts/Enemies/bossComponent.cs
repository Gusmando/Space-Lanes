using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class will define each component for the boss enemy,
handling the hurt state and destruction of each component
*/ 
public class bossComponent : MonoBehaviour
{
    [Header("Main Object")]
    public bossEnemy main;

    [Header("Health Settings")]
    public float health;

    [Header("State Variables")]
    public bool hurt;
    public bool vulnerable;

    [Header("Light Settings")]
    public Light compLight;
    public Color hurtColor;
    public Color normColor;
    public float hurtTime;

    // Update is called once per frame
    void Update()
    {
        //Setting the light one the component based on 
        //whether or not the component has been hurt
        if(hurt)
        {
            compLight.color = hurtColor;
        }
        else
        {
            compLight.color = normColor;
        }

        //Destroying the object on Death
        if(health <= 0)
        {
            Destroy(this.gameObject);

            //Sets Main Object Script variables so that
            //flee state may be triggered
            main.fleeing = true;
            main.compCount --;
        }

    }
    
    /*This function can be called by any bullet which may
    collide with the component, starting a coroutine and          
    granting invincibility frames*/
    public void hurtDelayStart()
    {
        StartCoroutine(hurtDelay(hurtTime));
    }

    //Alows a delay to be placed on the hurt state variable
    //depending on the passed in <delayLength>
    private IEnumerator hurtDelay(float delayLength)
    {
        hurt = true;

        yield return new WaitForSeconds(delayLength);

        hurt = false;

        yield return null;
    }
}
