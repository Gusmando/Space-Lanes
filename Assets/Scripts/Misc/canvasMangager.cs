using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class canvasMangager : MonoBehaviour
{
    public GameObject[] coloredUI;
    public Color currentColor;
    public Image[] healthBar;
    public Text ammoCount;
    public Image unlimitedSign;
    public int ammoCountInt;
    public int currentHealth;
    public bool limited;
    void Update() 
    {
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

        ammoCount.text = ammoCountInt.ToString();
    }
    public void updateUIColors()
    {
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
        for(int i = 0;i < 5; i++)
        {
            if(i< currentHealth - 1)
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
