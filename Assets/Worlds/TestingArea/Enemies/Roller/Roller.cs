using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : EnemyPhysicsObject
{
    //norm
    GameObject norm;

    //components
    Animator animator;

    //stats
    float maxSpeed = 1.2f;

    //state
    string state = "";
    string previousState = "PatrolLeft";
    float targetVelocityX = -1;
    bool normDetected;

    int timeToIdle;
    int timeToPatrol;
    int timeToGetUp;

    int bounceNum;

    protected string facingDirection
    {
        get
        {
            if (Mathf.Approximately(transform.rotation.eulerAngles.y, 180))
            {
                return "Right";
            }
            else
            {
                return "Left";
            }
        }
        set
        {
            if (value == "Right")
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (value == "Left")
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
          
        }
    }


    private void Awake()
    {
        animator = GetComponent<Animator>();
        state = "PatrolLeft";
        norm = GameObject.FindGameObjectWithTag("Player");
        setTimeToIdle();
        setTimeToPatrol();
        setTimeToGetUp();
        movesNormallyInAir = true;
    }

    protected override void ComputeVelocity()
    {

        updateState();

        Vector2 move = Vector2.zero;

        if (knockbackTime <= 0)
        {
            move.x = targetVelocityX;
        }

        targetVelocity = move * maxSpeed;
    }

    private void updateState()
    {
        //always called
        switch (state)
        {
            case "PatrolLeft":
                targetVelocityX = -1;
                countToIdle();
                maxSpeed = 1.2f;
                break;
            case "PatrolRight":
                targetVelocityX = 1;
                countToIdle();
                maxSpeed = 1.2f;
                break;
            case "RollLeft":
                targetVelocityX = -1;
                maxSpeed = 20;
                break;
            case "RollRight":
                targetVelocityX = 1;
                maxSpeed = 20;
                break;
            case "Detection":
                targetVelocityX = 0;
                break;
            case "Stun":
                targetVelocityX = 0;
                countToGetUp();
                break;
            case "Idle":
                targetVelocityX = 0;
                countToPatrol();
                break;
            case "Jump":
                targetVelocityX = 0;
                if (grounded)
                {
                    if (normDetected)
                    {
                        state = "Detection";
                        faceNorm();
                    }
                    else
                    {
                        state = "Idle";
                    }
                }
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

    public override void detectedNorm()
    {
        normDetected = true;
        if (state.Substring(0, 4) == "Roll" || state == "Stun" || state == "Jump")
        {
            return;
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

    void startRolling()
    {
        state = "Roll" + facingDirection;
    }

    private void faceNorm()
    {
        if (norm.transform.position.x < transform.position.x && facingDirection == "Right")
        {
            facingDirection = "Left";
        }
        else if (norm.transform.position.x > transform.position.x && facingDirection == "Left")
        {
            facingDirection = "Right";
        }
    }

    private void setTimeToIdle()
    {
        timeToIdle = Random.Range(200, 1000);
    }

    private void setTimeToPatrol()
    {
        timeToPatrol = Random.Range(50, 150);
    }

    private void setTimeToGetUp()
    {
        timeToGetUp = 300;
    }

    private void countToIdle()
    {
        timeToIdle--;
        if(timeToIdle < 0)
        {
            setTimeToIdle();
            state = "Idle";
        }
    }
    private void countToGetUp()
    {
        timeToGetUp--;
        if (timeToGetUp < 0)
        {
            setTimeToGetUp();
            state = "Jump";
            bounceNum = 0;
            velocity.y = 12;
        }
    }

    private void countToPatrol()
    {
        timeToPatrol--;
        if (timeToPatrol < 0)
        {
            randomFlipDirection();
            setTimeToPatrol();
            state = "Patrol" + facingDirection;
        }
    }

    private void randomFlipDirection()
    {
        int odds = Random.Range(0, 100);
        if(odds < 50)
        {
            flipDirection();
        }
    }
    public override void gonnaFall()
    {
        if (knockbackTime > 0 || !grounded || state.Substring(0,4) == "Roll") return;
        turnAround();
    }

    public override void blocked()
    {
        if (knockbackTime > 0 || state == "Stun") return;
        if (state.Substring(0,4) != "Roll")
        {
            if (!grounded) return;
            turnAround();
        }
        else
        {
            if(bounceNum < 2)
            {
                bounceNum++;
                flipDirection();
                state = "Roll" + facingDirection;
                return;
            }
            float multiplier = 1;

            float dx = facingDirection == "Right" ? -1 : 1;

            float dy = 1;

            GotHit(new Vector2(dx, dy).normalized, multiplier, false);

            state = "Stun";
        }
        
    }


    public override void bounce()
    {
        velocity.y = 21;
    }
    private void flipDirection()
    {
        if (facingDirection == "Right")
        {
            facingDirection = "Left";
        }
        else
        {
            facingDirection = "Right";
        }
    }

    private void turnAround()
    {
        flipDirection();
        state = "Patrol" + facingDirection;
    }

    public override void tookDamage(Collider2D collision)
    {
        if(state != "Stun")
        {
            string tag = collision.gameObject.tag;
            if (tag == "BootsSlide")
            {
               // collision.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<NormMovement>().slideKnockback();

            }
        }
        else
        {
            base.tookDamage(collision);
        }
        

        
    }

}