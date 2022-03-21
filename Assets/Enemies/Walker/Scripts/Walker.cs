using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Walker : EnemyPhysicsObject
{
    GameObject norm;

    Animator animator;

    int maxSpeed = 5;
    string state = "";
    string previousState = "PatrolLeft";
    float targetVelocityX = -1;

    bool normDetected;

    int layer_mask;
    int stunTime;
    int panicTime;

    bool nervous;
    public bool overLedge { get; set; }

    private void Awake()
    {
        norm = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        state = "PatrolLeft";
        layer_mask = LayerMask.GetMask("Default", "Norm");

        if (SceneManager.GetActiveScene().name == "PlutoCave") animator.SetLayerWeight(1, 1);

    }


    private void updateState()
    {
        if (stunTime > 0)
        {
            stunTime--;
            if (stunTime == 0)
            {
                faceAwayFromNorm();
                if (norm.transform.position.x > transform.position.x)
                {
                    state = "PanicLeft";
                }
                else
                {
                    state = "PanicRight";
                }
                panicTime = 100;
            }
        }

        if (panicTime > 0)
        {
            panicTime--;
            if(panicTime == 0)
            {
                if (!grounded || overLedge)
                {
                    panicTime = 5;
                }
                else
                {
                    state = "Hide";
                }
                
            }
        }
            //always called
            switch (state)
        {
            case "PatrolLeft":
                targetVelocityX = -0.4f;
                break;
            case "PatrolRight":
                targetVelocityX = 0.4f;
                break;
            case "PanicRight":
                targetVelocityX = 1f;
                break;
            case "PanicLeft":
                targetVelocityX = -1;
                break;
            case "Hurt":
                targetVelocityX = 0;
                break;
            case "Hide":
                targetVelocityX = 0;
                break;
            case "Exit":
                targetVelocityX = 0;
                break;
            case "Dead":
                targetVelocityX = 0;
                break;
            default:
                break;
        }

        if (previousState.Substring(0, 4) == state.Substring(0, 4))
        {
            return;
        }

        animator.CrossFade(state.Substring(0, 4), 0.0f);
        previousState = state;

    }
    private void faceAwayFromNorm()
    {
        if (norm.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (norm.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public override void gonnaFall()
    {
        if (knockbackTime > 0 || stunTime > 0 || !grounded || state.Substring(0, 5) == "Panic") return;
        turnAround();
    }

    public override void knockback(Vector2 pos, float multiplier)
    {
        base.knockback(pos, multiplier);
        state = "Hurt";
        stunTime = 15;
    }


    public override void blocked()
    {
        if (knockbackTime > 0 || stunTime > 0 || !grounded) return;

        if (state.Length > 4 && state.Substring(0, 5) == "Panic")
        {
            panicTime = 1;
            return;
        }
        turnAround();
    }

    public override void die(Vector2 pos)
    {
        StopAllCoroutines();
        state = "Dead";
        stunTime = 500;
        knockbackTime = 0;
        gravityModifier = 0;
        velocity.x = 0;
        velocity.y = 0;
        GameObject.FindGameObjectWithTag("JellybeanSpawner").GetComponent<JellybeanSpawner>().spawnBeans(transform.position, GetComponent<BoxCollider2D>().bounds, 3);
    }

    bool canSeeNorm()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, (norm.transform.position - transform.position).normalized, 8, layer_mask);

        Debug.DrawLine(transform.position, hit.point, Color.white, 2.5f);
        return hit.transform != null && hit.transform.gameObject.tag != null && hit.transform.gameObject.tag == "Player";
    }
    public void lookAround()
    {
        if (canSeeNorm()) return;
        state = "Exit";
    }

    private void turnAround()
    {
        if (state == "PatrolLeft")
        {
            state = "PatrolRight";
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (state == "PatrolRight")
        {
            state = "PatrolLeft";
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }


    protected override void ComputeVelocity()
    {
        updateState();

        Vector2 move = Vector2.zero;

        if (knockbackTime <= 0 && grounded)
        {
            move.x = targetVelocityX;
        }

        targetVelocity = move * maxSpeed;
    }


    void kill()
    {
        Destroy(gameObject);
    }

    void startPatrolling()
    {
        if (Mathf.Approximately(transform.rotation.eulerAngles.y, 0))
        {
            state = "PatrolRight";
        }
        else if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180))
        {
            state = "PatrolLeft";
        }

    }

}