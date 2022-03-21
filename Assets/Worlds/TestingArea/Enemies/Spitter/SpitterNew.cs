using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitterNew : EnemyPhysicsObject
{
    [SerializeField] GameObject spit;
    [SerializeField] Transform spitPosition;

    GameObject norm;

    Animator animator;

    int maxSpeed = 2;
    string state = "";
    string previousState = "PatrolLeft";
    float targetVelocityX = -1;

    bool normDetected;

    int layer_mask;
    int stunTime;

    private void Awake()
    {
        norm = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        state = "PatrolLeft";
        layer_mask = LayerMask.GetMask("Default", "Norm");
    }



     private void updateState()
     {
        if(stunTime > 0)
        {
            stunTime--;
            if (stunTime == 0)
            {
                state = "AlertIdle";
                if (normDetected)
                {
                    detectedNormResponse();
                }
            }
        }
        //always called
        switch (state)
        {
            case "PatrolLeft":
                targetVelocityX = -1;
                break;
            case "PatrolRight":
                targetVelocityX = 1;
                break;
            case "Detection":
                targetVelocityX = 0;
                break;
            case "Spit":
                targetVelocityX = 0;
                break;
            case "AlertIdle":
                targetVelocityX = 0;
                break;
            case "Hurt":
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
    private void faceNorm()
    {
        if (norm.transform.position.x < transform.position.x && Mathf.Approximately(transform.rotation.eulerAngles.y, 0))
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (norm.transform.position.x > transform.position.x && Mathf.Approximately(transform.rotation.eulerAngles.y, 180))
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void gonnaFall()
    {
        if (knockbackTime > 0 || stunTime > 0 || !grounded) return;
        turnAround();
    }

    public override void knockback(Vector2 pos, float multiplier)
    {
        base.knockback(pos, multiplier);
        state = "Hurt";
        stunTime = 25;
    }

    public override void endKnockback()
    {
        //state = "AlertIdle";
    }


    public override void blocked()
    {
        if (knockbackTime > 0 || stunTime > 0  || !grounded) return;
        turnAround();
    }

    public override void detectedNorm()
    {
        normDetected = true;

        detectedNormResponse();
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

    void detectedNormResponse()
    {
        if (!normDetected || knockbackTime > 0 || stunTime > 0) return;
        if (!canSeeNorm())
        {
            StartCoroutine(checkLineOfSight());
            return;
        }

        if (state == "Spit")
        {
            return;
        }
        else if (state == "AlertIdle")
        {
            state = "Spit";
            faceNorm();
        }
        else
        {
            state = "Detection";
            faceNorm();
        }
    }

    public override void normEscapedDetection()
    {
        if (!normDetected) return;
        normDetected = false;
    }

    private void finishedSpit()
    {
        if (knockbackTime > 0 || stunTime > 0) return;
        if (normDetected && canSeeNorm())
        {
            faceNorm();
        }
        else
        {
            state = "AlertIdle";
        }
    }

    bool canSeeNorm()
    {
        Vector3 normLoweredPosition = new Vector3(norm.transform.position.x, norm.transform.position.y - 0.4f, norm.transform.position.z);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, (normLoweredPosition - transform.position).normalized, 20, layer_mask);

        return hit.transform != null && hit.transform.gameObject.tag != null && hit.transform.gameObject.tag == "Player";
    }

    private void fireSpit()
    {
        GameObject spitBullet = Instantiate(spit, spitPosition.position, transform.rotation);
        if (Mathf.Approximately(transform.rotation.eulerAngles.y, 0))
        {
            spitBullet.GetComponent<Spit>().setDirection(1);
        }
        else if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180))
        {
            spitBullet.GetComponent<Spit>().setDirection(-1);
        }
    }
    public override void bounce()
    {
        velocity.y = 21;
    }

    private void turnAround()
    {
        if(state == "PatrolLeft")
        {
            state = "PatrolRight";
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(state == "PatrolRight")
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


    void startSpitting()
    {
        state = "Spit";
    }

    void kill()
    {
        Destroy(gameObject);
    }

    void startPatrolling()
    {
        if ( Mathf.Approximately(transform.rotation.eulerAngles.y, 0))
        {
            state = "PatrolRight";
        }
        else if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180))
        {
            state = "PatrolLeft";
        }
        
    }
    
    

    bool isStationary()
    {
        //return Mathf.Abs(rigidbody2d.velocity.x) < 1 && Mathf.Abs(rigidbody2d.velocity.y) < 1;
        return false;
    }

    IEnumerator checkLineOfSight()
    {
        yield return new WaitForSeconds(.3f);
        detectedNormResponse();
    }

    }