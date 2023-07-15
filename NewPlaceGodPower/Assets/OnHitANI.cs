using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHitANI : MonoBehaviour
{
    public Animator animator; // Reference to the animator component

    private bool isAnimationPlaying = false;

    private void Update()
    {
        // Check if the animation is currently playing
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            isAnimationPlaying = true;
        }
        else
        {
            isAnimationPlaying = false;
        }

        // Check if the animation has finished playing and destroy the object
        if (!isAnimationPlaying)
        {
            Destroy(gameObject);
        }
    }
}
