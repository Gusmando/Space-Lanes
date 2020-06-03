using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject playerCam;
    public MovementController player;
    private Vector3 initCamPos;
    [Header("Shake Function")]
    public float shakeIntensity;
    public float shakeDuration;
    public Color redLight;
    public Color whiteLight;
    public Light spotLight;
    public bool shaking;

    void Start()
    {
        initCamPos = playerCam.transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(shaking)
        {
            StartCoroutine(shakeDelay(shakeDuration));
            float x = Random.Range(-1f, 1f) * shakeIntensity;
            float y = Random.Range(-1f, 1f) * shakeIntensity;
            playerCam.transform.localPosition = initCamPos + new Vector3(x,y,0);
        }

        else
        {
            playerCam.transform.localPosition = initCamPos;
        }
        if(player.hurt)
        {
            spotLight.color = redLight;
        }
        else
        {
            spotLight.color = whiteLight;
        }
    }
    
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

    protected IEnumerator shakeDelay(float delayLength)
    {
        shaking = true;

        yield return new WaitForSeconds(delayLength);

        shaking = false;

        yield return null;
    }
}
