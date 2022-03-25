using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public class Pigeon : MonoBehaviour
{
    public readonly float baseVelocity = 5f;
    private Vector3 groundPoint;
    public enum States
    {
        Idle,
        Exit,
        Enter
    }

    public States state { get; set; }

    public bool inRange { get; protected set; }
    public Vector3 cockpitPoint { get; protected set; }
    public Vector3 startPoint { get; protected set; }
    public float xVelocity { get; protected set; }
    public bool inCockpit { get; protected set; }

    

    float midX;
    float distance;
    float a;
    public bool travelingRight { get; protected set; }

    UIHolder uiHolder;
    Animator anim;
    private Player gameInputManager;
    NormMovement norm;

    [SerializeField] SpriteRenderer glass;
    [SerializeField] Transform cockpit;

    private void Awake()
    {
        groundPoint = new Vector3(transform.position.x + 5, transform.position.y, 10);
        cockpitPoint = cockpit.position;
        gameInputManager = ReInput.players.GetPlayer(0);
        anim = GetComponent<Animator>();
        norm = GameObject.FindGameObjectWithTag("Player").GetComponent<NormMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (inCockpit) return;

        if(collision.gameObject.tag == "Player")
        {
            EnterRange();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (inCockpit) return;

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
        if (inRange && gameInputManager.GetButtonDown("PickUp") && norm.grounded)
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

        travelingRight = startPoint.x < cockpitPoint.x;

        state = States.Enter;
    }

    public void SetReversePath()
    {
        startPoint = cockpitPoint;
        midX = (startPoint.x + groundPoint.x) / 2;
        distance = Mathf.Abs(startPoint.x - groundPoint.x);

        xVelocity = baseVelocity * (distance / 2.5f);

        a = -3f / Mathf.Pow(startPoint.x - midX, 2);

        travelingRight = startPoint.x < groundPoint.x;
        
        state = States.Exit;
    }

    
    public float CalculateYPoint(float x)
    {
        float y = a * Mathf.Pow(x - midX, 2) + 3 + startPoint.y;
        if (y > startPoint.y + 2) InvertGlass(); 
        return y;
    }

    private void InvertGlass()
    {
        if(state == States.Exit)
        {
            glass.color = new Color(1, 1, 1, 0);
        }
        else
        {
            glass.color = new Color(1, 1, 1, 1);
        }
    }

    public void CloseCockpit()
    {
        anim.CrossFade("Close", 0.0f);
        inCockpit = true;
       
    }

    public void BlastOff()
    {
        anim.CrossFade("BlastOff", 0.0f);
    }

    public void ClosedCockpit()
    {
        if (!inCockpit) return;
        state = States.Idle;
        BlastOff();
    }

    public void FinishedExit()
    {
        state = States.Idle;
        inCockpit = false;
        inRange = true;
    }

    public void OpenedCockput()
    {
        if (!inCockpit) return;
        norm.ExitPigeon(this);
    }

    public bool NormReachedCockpit()
    {
        return Vector2.Distance(norm.transform.position, cockpitPoint) < 0.1f || norm.transform.position.x > cockpitPoint.x && travelingRight || norm.transform.position.x < cockpitPoint.x && !travelingRight;
    }

    public bool NormReachedGround()
    {
        return Vector2.Distance(norm.transform.position, groundPoint) < 0.1f || norm.transform.position.x > groundPoint.x && travelingRight || norm.transform.position.x < groundPoint.x && !travelingRight;
    }

    public void Land()
    {
        anim.CrossFade("Land", 0.0f);
        inCockpit = true;
        glass.color = new Color(1, 1, 1, 1);
    }

    public void Landed()
    {
        anim.CrossFade("Open", 0.0f);
    }
}
