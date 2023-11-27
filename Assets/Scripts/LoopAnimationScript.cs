using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoopAnimationScript : MonoBehaviour
{
    public int _animation;
    private static string ANIMATION_EMOTION = "targetAnimation";
    private Animator NPCAnimator;
    private int currentAnimation;

    void Start()
    {
        NPCAnimator = GetComponent<Animator>();
        currentAnimation = 0;
    }

    void Update()
    {
        if(currentAnimation == 0 && SceneManager.GetActiveScene().name.Equals("CreateScene"))
        {
            NPCAnimator.SetInteger(ANIMATION_EMOTION, _animation);
            currentAnimation = _animation;
            StartCoroutine(ExecuteAfter(2f));
        }
    }

    IEnumerator ExecuteAfter(float time)
    {
        yield return new WaitForSeconds(time);
        NPCAnimator.SetInteger(ANIMATION_EMOTION, 0);
        currentAnimation = 0;
    }
}
