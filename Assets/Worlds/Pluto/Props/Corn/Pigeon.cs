using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public class Pigeon : MonoBehaviour
{
    public readonly float baseVelocity = 5f;

    public bool inRange { get; protected set; }
    public Vector3 cockpitPoint { get; protected set; }
    public Vector3 startPoint { get; protected set; }
    public float xVelocity { get; protected set; }

    float midX;
    float distance;
    float a;

    UIHolder uiHolder;
    Animator anim;
    private Player gameInputManager;
    NormMovement norm;

    [SerializeField] Transform cockpit;

    private void Awake()
    {
        cockpitPoint = cockpit.position;
        gameInputManager = ReInput.players.GetPlayer(0);
        anim = GetComponent<Animator>();
        norm = GameObject.FindGameObjectWithTag("Player").GetComponent<NormMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            EnterRange();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ExitRange();
        }
    }

    private void EnterRange()
    {
        anim.CrossFade("Open", 0.0f);
        inRange = true;
    }

    private void ExitRange()
    {
        anim.CrossFade("Close", 0.0f);
        inRange = false;
    }

    private void Update()
    {
        CheckForEnterPress();
    }


    private void CheckForEnterPress()
    {
        if (inRange && gameInputManager.GetButtonDown("PickUp"))
        {
            try
            {
                if (uiHolder == null) uiHolder = GameObject.FindGameObjectWithTag("UIHolder").GetComponent<UIHolder>();
                uiHolder.hideUISlide();
            }
            catch(NullReferenceException ex)
            {
                //could not find UI holder
            }

            norm.EnterPigeon(this);
        }
    }

    public void SetPath()
    {
        startPoint = norm.transform.position;
        midX = (startPoint.x + cockpitPoint.x) / 2;
        distance = Mathf.Abs(startPoint.x - cockpitPoint.x);

        xVelocity = baseVelocity * (distance / 2.5f);

        a = -3f/Mathf.Pow(startPoint.x - midX, 2);
        
    }
    
    public float CalculateYPoint(float x)
    {
        float y = a * Mathf.Pow(x - midX, 2) + 3 + startPoint.y;
        //float y = -1 * 0.4445f * Mathf.Pow(x - midX, 2) + Mathf.Pow(distance / 3, 2) + startPoint.y;
        return y;
    }
}
