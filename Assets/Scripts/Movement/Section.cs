using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public Lane[] lanes;
    public EnemySpawner[] spawners;
    [System.Serializable]
    public class MultiDimensionalInt
    {
    public GameObject[] platforms;
    }
    public MultiDimensionalInt[] lanePlats;
    public bool sectionActive;
    public bool first;
    public Material offColor;
    public Material onColor;
    public GameObject endSpawn;
    public GameManager gameManager;

    void Start() 
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();    
        lightUpdate();
    }

    // Update is called once per frame
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
    public void lightUpdate()
    {
        if(sectionActive)
        {
            for (int i = 0; i < lanePlats.Length; i++)
            {
                if(i == gameManager.lowActiveLane)
                {
                    foreach(GameObject x in lanePlats[i].platforms)
                    {
                        x.GetComponent<Renderer>().material = onColor;
                    }
                }
                else
                {
                    foreach(GameObject x in lanePlats[i].platforms)
                    {
                        x.GetComponent<Renderer>().material = offColor;
                    }
                }
            }
        }
        else
        {
             for (int i = 0; i < lanePlats.Length; i++)
             {
                 foreach(GameObject x in lanePlats[i].platforms)
                {
                        x.GetComponent<Renderer>().material = offColor;
                }
             }
        } 
    }
    public void deactivateSpawner()
    {
        if(!sectionActive)
        {
            foreach (EnemySpawner x in spawners)
            {
                x.isActive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(!first)
        {
            if(other.tag == "Player")
            {
                tag = "activeSection"; 
                gameManager.activeSectionChanged = true; 
            }
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.tag == "Player")
        {
            tag = "Untagged";  
        }
    }
}
