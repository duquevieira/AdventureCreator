using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAnimationScript : MonoBehaviour
{
    [SerializeField]
    private int _animation;
    private static string ANIMATION_EMOTION = "targetAnimation";
    private Animator NPCAnimator;

    void Start()
    {
        NPCAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        NPCAnimator.SetInteger("targetAnimation", _animation);
        StartCoroutine(ExecuteAfter(2f));
    }

    IEnumerator ExecuteAfter(float time)
    {
        yield return new WaitForSeconds(time);
        NPCAnimator.SetInteger(ANIMATION_EMOTION, 0);
    }
}
