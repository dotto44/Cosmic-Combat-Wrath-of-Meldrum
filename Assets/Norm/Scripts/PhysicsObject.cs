using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [SerializeField] protected GameObject fireSpinHolder;
    [SerializeField] protected GameObject rake;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected UIController uiController;

    protected bool ducking;

    private bool _flipSprite;
    public bool flipSprite {
        get
        {
            return _flipSprite;
        }
        set
        {
            if (flipSpriteLocked) return;
            _flipSprite = value;
        }
    }

    public float minGroundNormalY = .65f;
    public const float gravity = 4f;
    public float gravityModifier = gravity;

    private int _direction;
    private int _previousDirection;
    protected int direction
    {
        get
        {
            return _direction;
        }
        set
        {
            _direction = value;
            if (_previousDirection != value)
            {
                setAnimatorLayer();
            }
            _previousDirection = value;
        }
    }
    protected int previousDirection;
    protected int timeInDirection;

    protected Vector2 targetVelocity;
    protected bool movementLock;
    protected bool flipSpriteLocked;
    protected float conveyorModifier;

    private bool _collapsed;
    protected bool collapsed
    {
        get
        {
            return _collapsed;
        }
        set
        {
            _collapsed = value;
            animator.SetBool("collapsed", value);
        }
    }

  
    private bool _grounded;
    protected bool previouslyGrounded;
    
    protected bool grounded {
        get
        {
            return _grounded;
        }
        set
        {
            previouslyGrounded = _grounded;
            _grounded = value;
        }
    }

    bool noFall;
    
    /*######KNOCKBACK######*/
    protected int _knockbackTime;
    protected const int maxKnockbackTime = 20;
    protected const int knockBackSpeed = 16;
    protected const int knockbackUpSpeed = 17;
    protected const int stunEndTime = 18;
    protected float knockbackSide;
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
                if (uiController == null) uiController = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
                uiController.tookDamage(1);
                if (uiController.health <= 0)
                {
                    dead = true;
                    return;
                }
                movementLock = true;
                attack = false;
                noFall = true;
                velocity.y = 0;
                velocity.x = 0;
                knockbackSide = flipSprite ? -knockBackSpeed : knockBackSpeed;
                animator.SetBool("knockback", true);
                Time.timeScale = 0.15f;
                invincibility = true;
                
                
            }
            else if (value == stunEndTime)
            {
                setOpacity(0.5f);
                noFall = false;
                Time.timeScale = 1;
                velocity.y = knockbackUpSpeed;
                modifyGravity(8);
            }
            else if (value == 0)
            {
                Time.timeScale = 1;
                movementLock = false;
                animator.SetBool("knockback", false);
                modifyGravity(4);
                StartCoroutine("endInvincibility");
            }

            _knockbackTime = value;
        }
    }

    public bool invincibility;

    /*######DEAD######*/
    protected bool _dead;
    protected bool dead
    {
        get
        {
            return _dead;
        }
        set
        {
            if (value)
            {
                movementLock = true;
                animator.SetBool("dead", true);
            }
            _dead = value;
        }
    }

    /*######ATTACK########*/
    protected bool _attack;
    protected bool attack
    {
        get
        {
            return _attack;
        }
        set
        {
            _attack = value;
            animator.SetBool("attack", value);
            if (value == false) flipSpriteLocked = false;
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
    protected const int fallHeightLimit = 50;
    protected const int maxFallSpeed = -22;

    float preGravityVelocity;
    int activeLayer;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
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
    void setOpacity(float opacity)
    {
        spriteRenderer.color = new Color(255, 255, 255, opacity);
    }
    public virtual void modifyGravity(int gravity)
    {

    }
    public virtual void resetGravity()
    {

    }
    public virtual void knockback(string direction)
    {

    }
    public void die()
    {

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
            if (knockbackTime < stunEndTime)
            {
                targetVelocity.x = -knockbackSide;
            }
        }
    }

    void FixedUpdate()
    {
        updateKnockbackTime();
        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        if (conveyorModifier == 0)
        {
            velocity.x = targetVelocity.x;
        }
        else
        {
            velocity.x = targetVelocity.x + conveyorModifier;
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

        if (grounded && timeAtMaxSpeed > fallHeightLimit)
        {
            //NO HIGH FALL ATM
            //highFall();
        }

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
                hitBufferList.Add(hitBuffer[i]);
            }

            for (int i = 0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if (currentNormal.y > minGroundNormalY)
                {
                    grounded = true;
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

    IEnumerator endInvincibility()
    {
        yield return new WaitForSeconds(1.25f);
        setOpacity(1f);
        invincibility = false;
    }
    
    protected void setFlipX(bool value)
    {
        spriteRenderer.flipX = value;
        fireSpinHolder.transform.rotation = value ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }

    public void setAnimatorLayer()
    {
        activeLayer = 0;

        for (int i = 1; i < 4; i++)
        {
            if (i == activeLayer)
            {
                animator.SetLayerWeight(i, 1);
            }
            else
            {
                animator.SetLayerWeight(i, 0);
            }
        }

    }
}