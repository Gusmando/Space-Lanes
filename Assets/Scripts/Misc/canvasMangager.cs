using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasMangager : MonoBehaviour
{
    [Header("Assignments")]
    public GameObject[] coloredUI;
    public Image[] healthBar;
    public Image unlimitedSign;
    public Image arrow;
    public Image timesTwo;
    public Text ammoCount;
    public float dubJumpTimeFull;

    [Header("Current State Vars")]
    public Color currentColor;
    public float dubJumpTime;
    public bool dubJumpOn;
    public int ammoCountInt;
    public int currentHealth;
    public bool limited;

    void Update() 
    {
        //Weapons with limited ammo counts will need to display an
        //ammo count and hide the infinity sign
        if(limited)
        {
            ammoCount.enabled = true;
            unlimitedSign.enabled = false;
        }
        else
        {
            ammoCount.enabled = false;
            unlimitedSign.enabled = true;
        }

        //When double jump is enabled, associated UI elements will be 
        //enabled and updated, otherwise disable arrow UI entirely
        if(dubJumpOn)
        {
            arrow.transform.Find("UpArrowShadow").GetComponent<Image>().enabled = true;
            arrow.enabled = true;
            timesTwo.enabled = true;
            arrow.fillAmount = dubJumpTime/dubJumpTimeFull;
            timesTwo.fillAmount = arrow.fillAmount;
        }
        else
        {
            arrow.transform.Find("UpArrowShadow").GetComponent<Image>().enabled = false;
            arrow.enabled = false;
            timesTwo.enabled = false;
            arrow.fillAmount = 1;
            timesTwo.fillAmount = 1;
        }

        ammoCount.text = ammoCountInt.ToString();
    }

    public void updateUIColors()
    {
        //Changing the color of UI based on tag, colors will be 
        //set based upon the current weapon choice
        foreach(GameObject x in coloredUI)
        {
            if(x.tag.Contains("Bar"))
            {
                currentColor = new Color(currentColor.r,currentColor.g,currentColor.b,.071f);
                x.GetComponent<Shadow>().effectColor = currentColor;       
            }
            else if(x.tag.Equals("shadowUI"))
            {
                currentColor = new Color(currentColor.r,currentColor.g,currentColor.b,1);  
                x.GetComponent<Shadow>().effectColor = currentColor;
            }
            else if(x.tag.Equals("boxUI"))
            {
                currentColor = new Color(currentColor.r,currentColor.g,currentColor.b,1);  
                if(x.gameObject.GetComponent<Image>() != null)
                {
                    x.GetComponent<Image>().color = currentColor;
                }
                x.GetComponent<Outline>().effectColor = currentColor;
            }
            else if(x.tag.Equals("frame"))
            {
                currentColor = new Color(currentColor.r,currentColor.g,currentColor.b,1);  
                x.GetComponent<Outline>().effectColor = currentColor;
            }
        }
    }

    public void updateUIHealth()
    {
        //Enabling and disabling health bars
        //based on current health count
        for(int i = 0;i < 5; i++)
        {
            if(i< currentHealth)
            {
                healthBar[i].enabled = true;
            }

            else 
            {
                healthBar[i].enabled = false;
            }
        }
    }
}
