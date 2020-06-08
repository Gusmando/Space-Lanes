using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public Lane[] lanes;
    public int fadingArch;
    public int lastFade;
    public EnemySpawner[] spawners;
    public GameObject[] arches;
    [System.Serializable]
    public class MultiDimensionalInt
    {
        public GameObject[] platforms;
    }
    public MultiDimensionalInt[] lanePlats;
    public MultiDimensionalInt[] lights;
    public bool sectionActive;
    public bool first;
    public bool fading;
    public bool alt;
    public float intensityLev;
    public Color onLight;
    public Color offLight;
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
    void Update()
    {
        if(sectionActive)
        {
            if(!fading)
            {
                if(alt)
                {
                    StartCoroutine(FadeTo(1,.25f));
                }
                else
                {
                    StartCoroutine(FadeTo(0,.25f));
                } 
            }
        }

        else
        {
            
        }
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
                    foreach(GameObject x in lights[i].platforms)
                    {
                        x.GetComponent<Light>().color = onLight;
                    }
                }
                else
                {
                    foreach(GameObject x in lanePlats[i].platforms)
                    {
                        x.GetComponent<Renderer>().material = offColor;
                    }
                    foreach(GameObject x in lights[i].platforms)
                    {
                        x.GetComponent<Light>().color = offLight;
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
                
                foreach(GameObject x in lights[i].platforms)
                {
                    x.GetComponent<Light>().color = offLight;
                }
            }
            for( int i = 0; i < arches.Length; i++)
            {
                arches[i].GetComponent<Renderer>().material.SetColor("_EmissionColor",new Color(1,1,1,1) * 0);
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

    IEnumerator FadeTo(float aValue, float aTime)
    {
        fading = true;
        intensityLev = arches[fadingArch].GetComponent<Renderer>().material.GetColor("_EmissionColor").a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, 1);
            arches[fadingArch].GetComponent<Renderer>().material.SetColor("_EmissionColor",newColor * Mathf.Lerp(intensityLev,aValue,t));
            yield return null;
        }
        fadingArch ++;

        if(fadingArch == arches.Length)
        {
            fadingArch = 0;

            if(!alt)
            {
                alt = true;
            }
            else
            {
                alt = false;
            }
            
        }
        fading = false;
        
    }
}
