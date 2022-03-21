using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhysicsObject : MonoBehaviour
{
    bool normHadInvincibility;
    protected bool movesNormallyInAir;

    protected float enemyMultiplier = 1;
    [SerializeField] protected int health = 3;
    SpriteRenderer spriteRenderer;

    public float minGroundNormalY = .65f;
    protected float gravity = 4f;
    public float gravityModifier;
    protected bool noFall;

    private int _direction;
    protected int direction
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value;
        }
    }



    protected Vector2 targetVelocity;
    protected float conveyorModifier;

    private bool _grounded;

    protected bool grounded
    {
        get
        {
            return _grounded;
        }
        set
        {
            _grounded = value;
        }
    }


    /*######KNOCKBACK######*/
    protected int _knockbackTime;
    protected const int maxKnockbackTime = 7;
    protected const float knockbackSpeedMax = 7;
    protected const float knockbackUpSpeedMax = 12f;
    protected float knockbackSpeed;
    protected float knockbackUpSpeed;
    bool groundedSinceKnockback;
    protected int knockbackTime
    {
        get
        {
            return _knockbackTime;
        }
        set
        {
            if (value == maxKnockbackTime)
            {
                velocity.y = knockbackUpSpeed;
            }
            else if (value == maxKnockbackTime - 1)
            {
                groundedSinceKnockback = false;
            }
            else if (value == 0)
            {
                endKnockback();
            }

            _knockbackTime = value;
        }
    }
   
    protected int timeAtMaxSpeed;
    protected Vector2 groundNormal;
    protected Rigidbody2D rb2d;
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);


    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    protected const int wallJumpTime = 5;
    protected const int fallHeightLimit = 50;
    protected const int maxFallSpeed = -22;


    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gravityModifier = gravity;
    }

    public virtual void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }
    public virtual void modifyGravity(int gravity)
    {
        gravityModifier = gravity;
    }
    public virtual void resetGravity()
    {
        gravityModifier = gravity;
    }
    public virtual void gonnaFall()
    {

    }
    public virtual void blocked()
    {

    }
    public virtual void bounce()
    {

    }
    public virtual void detectedNorm()
    {

    }
    public virtual void normEscapedDetection()
    {

    }
    public virtual void endKnockback()
    {

    }
    public virtual void die(Vector2 pos)
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public virtual void hitNorm(Collider2D collision)
    {
        string direction = "Left";
        if (collision.gameObject.transform.position.x > transform.position.x)
        {
            direction = "Right";
        }
        if (!collision.gameObject.GetComponent<NormMovement>().invincibility)
        {
            collision.gameObject.GetComponent<NormMovement>().knockback(direction);
        }
    }

    public void setConveyorModifier(float conveyorModifier)
    {
        this.conveyorModifier = conveyorModifier;
    }

    protected virtual void highFall()
    {

    }

    public void updateKnockbackTime()
    {
        if (knockbackTime > 0)
        {
            knockbackTime--;
            targetVelocity.x = knockbackSpeed;

        }
    }

    void FixedUpdate()
    {

        updateKnockbackTime();

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        
        if (grounded || !grounded && movesNormallyInAir)
        {
            velocity.x = targetVelocity.x;
        }
        else if(!grounded && !groundedSinceKnockback)
        {
            velocity.x = velocity.x * 0.95f; ;
        }
        
        if (velocity.y <= maxFallSpeed)
        {
            velocity.y = maxFallSpeed;
            timeAtMaxSpeed++;
        }
        else
        {
            timeAtMaxSpeed = 0;
        }


        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;

        Movement(move, true);


    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for (int i = 0; i < count; i++)
            {
                if (hitBuffer[i].transform.tag == "Player") continue;
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
                    groundedSinceKnockback = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if (projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }


                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;

            }


        }

        if (noFall && yMovement)
        {
            velocity.y = 0;
            return;
        }

        rb2d.position = rb2d.position + move.normalized * distance;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hitNorm(collision);
        }
        else if (collision.tag == "Rake" || collision.tag == "NormBullet")
        {
            tookDamage(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;

        hitNorm(collision);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag != "Player") return;
        normHadInvincibility = false;
    }

    public virtual void tookDamage(Collider2D collision)
    {
        float multiplier = 1;

        float dx = 1;
        //float dy = collision.transform.position.y;

        string tag = collision.gameObject.tag;

        if (tag == "Rake")
        {
            dx = Mathf.Approximately(collision.gameObject.transform.parent.rotation.y, 0) ? dx * -1 : dx;
        }
        else if(tag == "NormBullet")
        {
            dx = collision.gameObject.transform.position.x > transform.position.x ? dx * -1 : dx;
        }

        GotHit(new Vector2(dx, 0).normalized, multiplier, true);
    }

    public void GotHit(Vector2 pos, float multiplier, bool doesDamage)
    {
        if (!doesDamage) return;

        health -= 1;
        if (health > 0)
        {
            knockback(pos, multiplier);
        }
        else
        {
            die(pos);
        }
    }

    public virtual void knockback(Vector2 pos, float multiplier)
    {
        flash();

        if (pos.x <= 0.001 && pos.y <= 0.001) return;

        knockbackUpSpeed = pos.y * multiplier * knockbackUpSpeedMax * enemyMultiplier;
        knockbackSpeed = pos.x * multiplier * knockbackSpeedMax * enemyMultiplier;

        knockbackTime = maxKnockbackTime;
        
    }

    public void flash()
    {
        StopAllCoroutines();
        spriteRenderer.material.SetFloat("_FlashAmount", 0.75f);
        StartCoroutine(endFlash());
    }


    IEnumerator endFlash()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        //StartCoroutine(endKnockback());
    }
}