using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    //Visible 2d Array in the inspector
    [System.Serializable]
    public class MultiDimensionalInt
    {
        public GameObject[] platforms;
    }

    [Header("Section Assignments")]
    public GameObject endSpawn;
    public GameManager gameManager;
    public Lane[] lanes;
    public bool first;
    public bool notFirst;

    [Header("Section State")]
    public bool sectionActive;

    [Header("Platforms")]
    public MultiDimensionalInt[] lanePlats;
    public Material offColor;
    public Material onColor;
    public Material speedColor;

    [Header("Arches")]
    public GameObject[] arches;
    public int fadingArch;
    public int lastFade;
    public float lightFadeSpeed;
    public bool fading;
    public bool fadingLast;
    public bool alt;
    public bool altLast;
    public bool startOff;
    public int startOffInt;

    [Header("Spawners")]
    public EnemySpawner[] spawners;

    [Header("Lighting")]
    public MultiDimensionalInt[] lights;
    public Color onLight;
    public Color offLight;

    void Start() 
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();    
        lightUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        //Spawners not active unless section is
        if(!sectionActive)
        {
            deactivateSpawner();
        }

        //If arches exist and the section is active
        if(sectionActive && arches.Length != 0)
        {
            //Start Coroutine which fades the lights in
            if(!fading)
            {
                StartCoroutine(FadeTo(2,lightFadeSpeed));
            }
            
            //Start Coroutine which fades lights out when
            //start off arch is reached in the fading process
            if(!fadingLast && startOff)
            {
                StartCoroutine(FadeToLast(0,lightFadeSpeed));
            }
        }
    }

    //Activate all spawners within the section
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

    //When called, this function will update all lights
    //and platform colors 
    public void lightUpdate()
    {
        if(sectionActive)
        {
            //For all Lanes
            for (int i = 0; i < lanePlats.Length; i++)
            {
                //Low active lane color
                if(i == gameManager.lowActiveLane)
                {
                    //For all platforms in each lane
                    foreach(GameObject x in lanePlats[i].platforms)
                    {
                        //Update Material 
                        x.GetComponent<Renderer>().material = onColor;
                        if(x.tag.Contains("Fast"))
                        {
                            x.GetComponent<Renderer>().material = speedColor;
                        }
                    }

                    //For all lights in each lane
                    foreach(GameObject x in lights[i].platforms)
                    {
                        x.GetComponent<Light>().color = onLight;
                    }
                }

                //Everything else
                else
                {
                    //For all platforms in each lane
                    foreach(GameObject x in lanePlats[i].platforms)
                    {
                        x.GetComponent<Renderer>().material = offColor;
                        if(x.tag.Contains("Fast"))
                        {
                            x.GetComponent<Renderer>().material = speedColor;
                        }
                    }
                    //For all lights in each lane
                    foreach(GameObject x in lights[i].platforms)
                    {
                        x.GetComponent<Light>().color = offLight;
                    }
                }
            }
        }

        else
        {
            //When the section is not active 
            //all lights and platforms set to off color
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

            //Arch lights are off
            for( int i = 0; i < arches.Length; i++)
            {
                arches[i].GetComponent<Renderer>().material.SetColor("_EmissionColor",new Color(1,1,1,1) * 0);
            }
        
        } 
    }

    //This method used to turn of all spawners in the section
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

    //Sets the current active section to the
    //one which has collided
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

    //When leaving this assignment is changed
    private void OnTriggerExit(Collider other) 
    {
        if(other.tag == "Player")
        {
            tag = "Untagged";  
        }
    }

    //This function will be used fade the light to a value
    //in a set amount of time
    IEnumerator FadeTo(float aValue, float aTime)
    {
        fading = true;
        float intensityLev = arches[fadingArch].GetComponent<Renderer>().material.GetColor("_EmissionColor").a;
        
        //As time increases and is not at the goal time
        for (float t = 0.0f; t < 1.0f;t += (Time.deltaTime / aTime))
        {
            //Color is taken from the current weapon amd applied to the arches
            Color newColor = GameObject.FindWithTag("Player").GetComponent<MovementController>().weapon.currentGun.gunColor;
            arches[fadingArch].GetComponent<Renderer>().material.SetColor("_EmissionColor",newColor * Mathf.Lerp(intensityLev,aValue,t));
            yield return null;
        }

        //fadingArch iterator increases
        fadingArch ++;

        //When the fadingArch reaches the start of the fade out
        if(fadingArch == startOffInt && !startOff)
        {
            startOff = true;            
        }
        
        //When finished with arches, reset iterators
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

    //Separate coroutine which will be used to fade the arches
    //to a value of 0
    IEnumerator FadeToLast(float aValue, float aTime)
    {
        //
        fadingLast = true;
        float intensityLev = arches[lastFade].GetComponent<Renderer>().material.GetColor("_EmissionColor").a;

        //As time increases and is not at the goal time
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            //Color will be set to a neutral off color
            Color newColor = new Color(1, 1, 1, 1);
            arches[lastFade].GetComponent<Renderer>().material.SetColor("_EmissionColor",newColor * Mathf.Lerp(intensityLev,aValue,t));
            yield return null;
        }
        
        //Iterator increases
        lastFade ++;

        //Values reset
        if(lastFade == arches.Length)
        {
            lastFade = 0;

            if(!altLast)
            {
                altLast = true;
            }
            else
            {
                altLast = false;
            }   
        }
        fadingLast = false;
    }
}
