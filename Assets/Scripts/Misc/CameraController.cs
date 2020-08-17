using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Assignments")]
    public GameObject playerCam;
    public MovementController player;
    private Vector3 initCamPos;

    [Header("Shake Function Settings")]
    public float shakeIntensity;
    public float shakeDuration;

    [Header("Light Settings")]
    public Color redLight;
    public Color whiteLight;
    public Light spotLight;
    
    [Header("State Vars")]
    public bool shaking;

    void Start()
    {
        //Saving initial Position
        initCamPos = playerCam.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If this variable is active
        if(shaking)
        {
            //Shake boolean delay
            StartCoroutine(shakeDelay(shakeDuration));

            //Setting to random location within small range
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;

            //Setting position to random coordinates
            playerCam.transform.localPosition = initCamPos + new Vector3(x,y,0);
        }
        else
        {
            playerCam.transform.localPosition = initCamPos;
        }

        //Changing the Light based on hurt status
        if(player.hurt)
        {
            spotLight.color = redLight;
        }
        else
        {
            spotLight.color = whiteLight;
        }
    }
    
    //Methods below used for starting shake
    //and changing intensity values
    public void screenShake()
    {
        shaking = true; 
    }
    public void setShakeIntensity(float intensityMod)
    {
        shakeIntensity = intensityMod;
    }
    public void setShakeLength(float shakeTime)
    {
        shakeDuration = shakeTime;
    }

    //Coroutine used for delaying the shaking variable
    protected IEnumerator shakeDelay(float delayLength)
    {
        shaking = true;

        yield return new WaitForSeconds(delayLength);

        shaking = false;

        yield return null;
    }
}
