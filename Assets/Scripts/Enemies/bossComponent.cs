using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossComponent : MonoBehaviour
{
    public bossEnemy main;
    public float health;
    public bool hurt;
    public bool vulnerable;
    public float hurtTime;
    public Light compLight;
    public Color hurtColor;
    public Color normColor;
    // Update is called onsce per frame
    void Update()
    {
        if(hurt)
        {
            compLight.color = hurtColor;
        }
        else
        {
            compLight.color = normColor;
        }

        if(health <= 0)
        {
            Destroy(this.gameObject);
            main.fleeing = true;
        }

    }

    public void hurtDelayStart()
    {
        StartCoroutine(hurtDelay(hurtTime));
    }

    private IEnumerator hurtDelay(float delayLength)
    {
        hurt = true;

        yield return new WaitForSeconds(delayLength);

        hurt = false;

        yield return null;
    }
}
