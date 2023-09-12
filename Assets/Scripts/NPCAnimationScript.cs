using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationScript : MonoBehaviour
{
    private int _animation;
    private static string ANIMATION_EMOTION = "targetAnimation";

    void Start()
    {
        _animation = 0;
    }

    void setAnimation(int animationIndex)
    {
        _animation = animationIndex;
    }

    int getAnimation()
    {
        return _animation;
    }

    void playAnimation()
    {
        Animator NPCAnimator = GetComponent<Animator>();
        NPCAnimator.SetInteger("targetAnimation", _animation);
        StartCoroutine(ExecuteAfter(2f, NPCAnimator));
    }

    IEnumerator ExecuteAfter(float time, Animator animator)
    {
        yield return new WaitForSeconds(time);
        animator.SetInteger(ANIMATION_EMOTION, 0);
    }
}
