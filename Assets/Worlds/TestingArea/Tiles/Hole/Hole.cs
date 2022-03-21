using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Enetered hole");
        animator.CrossFade("Invisible", 0.25f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        animator.CrossFade("Visible", 0.25f);
    }
}
