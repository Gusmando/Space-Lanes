using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public Lane[] lanes;
    public EnemySpawner[] spawners;
    public bool sectionActive;
    public GameObject endSpawn;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateSpawner()
    {
        if(sectionActive)
        {
            foreach (EnemySpawner x in spawners)
            {
                x.isActive = true;
            }
        }
    }
}
