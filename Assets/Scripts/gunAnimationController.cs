using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAnimationController : MonoBehaviour
{
    [Header("Player Animation Controller")]
    public Animator playerAnim;

    [Header("Gun Object")]
    public Transform gunRotation;

    [Header("Asjustments")]
    public int angleLeft;
    public int angleRight;
    public float time;


    // Update is called once per frame
    void Update()
    {
        switch(playerAnim.GetInteger("animState"))
        {
            case 0:

                AnimatorClipInfo[] clipInfo = playerAnim.GetCurrentAnimatorClipInfo(0);
                AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);

                time = stateInfo.normalizedTime % 1; 
                if(time >= 0 && time < .33)
                {
                    gunRotation.eulerAngles = new Vector3(0,0,angleLeft);
                    break;
                }

                else if(time >= .66 && time <= 1)
                {
                    gunRotation.eulerAngles = new Vector3(0,0,angleRight);
                    break;
                }

                else
                {
                    gunRotation.eulerAngles = new Vector3(0,0,0);
                    break;
                }  
            break;

            case 01:

            break;
            case 10:

            break;
            case 100:

            break;
            case 111:

            break;
        }
    }
}
