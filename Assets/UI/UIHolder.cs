using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHolder : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        DontDestroyOnLoad(gameObject);
    }

    public void hideUISlide()
    {
        animator.CrossFade("UIHide", 0);
    }

    public void unhideUISlide()
    {
        animator.CrossFade("UIShow", 0);
    }
}
