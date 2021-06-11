using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    private float gunAccuracy;

    [SerializeField]
    private GameObject go_CrosshairHUD;
    [SerializeField]
    private GunController theGunController;

    public void WalkingAnimation(bool _flag)
    { if (!GameManager.isWater)
        {
            animator.SetBool("Walking", _flag);

        }
    }

    public void RunningAnimation(bool _flag)
    {
        if (!GameManager.isWater)
        {
            animator.SetBool("Running", _flag);
        }
    }
    public void FireAnimation()
    {
        if (!GameManager.isWater)
        {
            if (animator.GetBool("Walking"))
                animator.SetTrigger("Walk_Fire");
            else
                animator.SetTrigger("Idle_Fire");
        }
    }

    public float GetAccuracy()
    {
        {
            if (animator.GetBool("Walking"))
                gunAccuracy = 0.06f;
            else
                gunAccuracy = 0.035f;

            return gunAccuracy;
        }
    }
}
