using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : EnemyPhysicsTest
{
    //Player
    GameObject norm;

    //Components
    [SerializeField] Transform rightLimit;
    [SerializeField] Transform leftLimit;
    [SerializeField] GameObject detectionZone;
    [SerializeField] GameObject exitZone;

    SpriteRenderer spriteRenderer;
    Animator animator;

    //Stats
    [SerializeField] float moveSpeed;

    //General Variables
    string state = "";

    //Patrol Variables
    Transform currentTarget;
    string currentTargetName;

    //Idle Variables
    [SerializeField] float waitTime = 4.0f;

    //Detection Variables
    bool alerted;
    bool isInExit;
    private void Awake()
    {
       // norm = GameObject.FindGameObjectWithTag("Player");
       // animator = GetComponent<Animator>();
       // spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        //currentTargetName = "Left";
        //currentTarget = leftLimit;
        //ChangeState("Patrol");
    }
    protected override void ComputeVelocity()
    {

        Vector2 move = Vector2.zero;

        move.x = 0;

        targetVelocity = move * 4;
    }
    private void Update()
    {
        switch (state)
        {
            case "Patrol":
                Patrol();
                break;
            case "Idle":
                break;
            case "Detection":
                break;
            case "Spit":
                break;
            default:
                break;
        }
    }

    private void Patrol()
    {
       /* float step = moveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, step);

        if(Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.1f)
        {
            ChangeState("Idle");
        }*/
    }

    private void Idle()
    {

    }

    public void InField()
    {
        if (alerted) return;
        alerted = true;
        ChangeState("Detection");
    }
    public void FinishedShot()
    {
        if (!isInExit)
        {
            ChangeState("AlertIdle");
        }
    }
    public void FinishedAlertIdle()
    {
        alerted = false;
        ChangeState("Patrol");
    }

    public void EndDetection()
    {
        ChangeState("Spit");
    }

    private void ChangeState(string nextState)
    {
        state = nextState;

        animator.CrossFade(state, 0.0f);
        switch (state)
        {
            case "Patrol":
                break;
            case "Idle":
                StartCoroutine(JustIdleForABit());
                break;
            case "Detection":
                break;
            case "Spit":
                faceNorm();
                break;
            default:
                break;
        }
    }

    private void faceNorm()
    {
        if(norm.transform.position.x > transform.position.x && !spriteRenderer.flipX ||
            norm.transform.position.x < transform.position.x && spriteRenderer.flipX)
        {
            flipDirection();
        }
    }
    private void flipDirection()
    {
        if (currentTargetName == "Right")
        {
            currentTargetName = "Left";
            currentTarget = leftLimit;
        }
        else
        {
            currentTargetName = "Right";
            currentTarget = rightLimit;
        }
        spriteRenderer.flipX = !spriteRenderer.flipX;
        if (spriteRenderer.flipX)
        {
            detectionZone.transform.Rotate(0, 0, 180);
            exitZone.transform.Rotate(0, 0, 180);
        }
        else
        {
            detectionZone.transform.Rotate(0, 0, -180);
            exitZone.transform.Rotate(0, 0, -180);
        }
       
    }

    public void SetIsInExit(bool isInExit)
    {
        if(state == "AlertIdle" && isInExit)
        {
            ChangeState("Spit");
        }
        this.isInExit = isInExit;
    }

    IEnumerator JustIdleForABit()
    {
        yield return new WaitForSeconds(waitTime);
        flipDirection();
        ChangeState("Patrol");
        alerted = false;
    }
}
