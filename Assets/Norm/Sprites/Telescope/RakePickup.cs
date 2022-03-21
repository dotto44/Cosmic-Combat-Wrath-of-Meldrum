using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public class RakePickup : MonoBehaviour
{
    [SerializeField] GameObject shine;
    [SerializeField] GameObject textBox;
    UIHolder uiHolder;

    Animator animator;
    private Player gameInputManager;
    NormMovement norm;

    bool shining;
    bool canCloseObtain;

    private void Awake()
    {
        gameInputManager = ReInput.players.GetPlayer(0);
        animator = GetComponent<Animator>();
        norm = GameObject.FindGameObjectWithTag("Player").GetComponent<NormMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (shining) return;

        animator.SetBool("inRange", true);
        norm.setCanPickupStick(true, gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (shining) return;

        animator.SetBool("inRange", false);
        norm.setCanPickupStick(false, gameObject);
    }

    private void Update()
    {
        if (canCloseObtain && (gameInputManager.GetButtonDown("Attack") || gameInputManager.GetButtonDown("Jump")))
        {
            //if we didn't find the UI earlier to hide it, it doesn't exist or is not hidden  
            if(uiHolder != null) uiHolder.unhideUISlide();

            textBox.GetComponent<Animator>().CrossFade("ObtainOut", 0);
            animator.CrossFade("FadeOut", 0);
            StartCoroutine("FreeNorm");
        }
    }

    public void startShine()
    {
        shining = true;
        textBox.SetActive(true);
        shine.SetActive(true);
        animator.SetBool("inRange", false);

        try
        {
            if (uiHolder == null) uiHolder = GameObject.FindGameObjectWithTag("UIHolder").GetComponent<UIHolder>();
            uiHolder.hideUISlide();
        }
        catch (NullReferenceException ex)
        {
            //could not find UI holder
        }

        StartCoroutine("CloseObtain");
    }

    IEnumerator CloseObtain()
    {
        yield return new WaitForSeconds(3.1f);
        canCloseObtain = true;
    }

    IEnumerator FreeNorm()
    {
        yield return new WaitForSeconds(1f);
        norm.obtainItemEnd();
        Destroy(gameObject);
    }
}
