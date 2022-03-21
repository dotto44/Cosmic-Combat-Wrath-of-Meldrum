using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.SceneManagement;

public class NormMovement : MonoBehaviour
{
    [SerializeField] protected GameObject fireSpinHolder;
    [SerializeField] protected GameObject rake;
    [SerializeField] protected GameObject voidHolder;

    Pigeon pigeon;

    #region Components
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected UIController uiController;
    protected Rigidbody2D rb2d;
    #endregion

    #region Fields
    /*######REWIRED######*/
    int playerId = 0;
    private Player gameInputManager;

    /*######PHYSICS######*/
    public bool grounded { get; protected set; }
    protected Vector2 velocity;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);
    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected bool noFall;
    protected bool movementLock;
    protected float gravity = defaultGravity;
    protected int timeAtMaxSpeed;
    protected bool isGravity = true;

    /*######ANIMATION#######*/

    /// <summary>
    /// The current active animator layer
    /// </summary>
    protected int activeLayer;
    enum AnimationLayers
    {
        Base,
        Stick,
        Obtain,
        Pigeon
    }


    /*######DIRECTION######*/
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
            _previousDirection = value;
        }
    }

    /*######FLIP SPRITE######*/
    protected bool flipSpriteLocked;
    private bool _flipSprite;
    public bool flipSprite
    {
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
                voidHolder.SetActive(true);
            }
            _dead = value;
        }
    }

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
                resetGravity();
                StartCoroutine("endInvincibility");
            }

            _knockbackTime = value;
        }
    }

    protected const int spiroxCutsceneKnockbackTime = 260;
    protected int _cutsceneKnockbackTime;
    protected int cutsceneKnockbackTime
    {
        get
        {
            return _cutsceneKnockbackTime;
        }
        set
        {
            {
                if (value == spiroxCutsceneKnockbackTime)
                {
                    movementLock = true;
                    attack = false;
                    velocity.y = knockbackUpSpeed;
                    velocity.x = 0;
                    knockbackSide = flipSprite ? -knockBackSpeed : knockBackSpeed;
                    animator.SetBool("knockback", true);
                    modifyGravity(8);
                }
                else if (value == 0)
                {
                    movementLock = false;
                    animator.SetBool("knockback", false);
                    resetGravity();
                    StartCoroutine("endInvincibility");
                }

                _cutsceneKnockbackTime = value;
            }
        }
    }

    public bool invincibility { get; set; }
    protected bool canCancelJump;
    protected float maxSpeed = 7;
    protected float jumpTakeOffSpeed = 21;
    protected bool ducking;

    #endregion

    #region Constants
    public const float minGroundNormalY = .65f;
    public const float defaultGravity = 4f;
    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;
    protected const int fallHeightLimit = 50;
    protected const int maxFallSpeed = -22;
    #endregion

    int bufferJumpTime = 0;

    bool hasStick = true;
    bool canPickupStick;
    GameObject stickObject;

    float conveyorModifier = 0;
    bool collapsed;
    /*######ATTACK########*/
    int bufferAttackTime = 0;
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

    void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    void OnEnable()
    {
        gameInputManager = ReInput.players.GetPlayer(0);
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "PlutoCave")
        {
            hasStick = true;
        }
    }

    void Update()
    {
        checkForDuck();
        computeVelocity();
        checkForJump();

        if ((gameInputManager.GetButtonDown("Attack") || bufferAttackTime > 0) && !movementLock && hasStick && !attack /*&& knockbackTime <= 0*/ && !ducking)
        {
            rakeAttack();
        }
        else if (gameInputManager.GetButtonDown("Attack"))
        {
            bufferAttackTime = 10;
        }

        if(gameInputManager.GetButtonDown("PickUp") && canPickupStick)
        {
            obtainItem();
            hasStick = true;
            canPickupStick = false;
            
        }

        setFlipX(flipSprite);

        if (attack && grounded && animator.GetCurrentAnimatorStateInfo(0).IsName("RakeAirborne") && !animator.GetBool("jump"))
        {
            animator.CrossFade("RakeStandingAttack", 0, 0, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            movementLock = true;
        }

        if (attack && !grounded && animator.GetCurrentAnimatorStateInfo(0).IsName("RakeStandingAttack"))
        {
            animator.CrossFade("RakeAirborne", 0, 0, animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }

        animator.SetBool("grounded", grounded);
        animator.SetFloat("velocityX", Mathf.Abs((velocity.x - conveyorModifier) / maxSpeed));
        animator.SetFloat("velocityY", velocity.y / maxSpeed);
    }

    private void obtainItem()
    {
        movementLock = true;
        setAnimatorLayer((int)AnimationLayers.Obtain);
        stickObject.transform.position = new Vector3(transform.position.x, transform.position.y + 1.75f, transform.position.z);
        stickObject.GetComponent<RakePickup>().startShine();
    }
    int startTime = 0;
    public void EnterPigeon(Pigeon pigeon)
    {
        movementLock = true;
        isGravity = false;
        this.pigeon = pigeon;
        pigeon.SetPath();
        setAnimatorLayer((int)AnimationLayers.Pigeon);

    }

    protected void MoveIntoCockpit()
    {
        if (activeLayer != (int)AnimationLayers.Pigeon) return;
        startTime++;
        rb2d.position = new Vector3(rb2d.position.x, pigeon.CalculateYPoint(rb2d.position.x));
        if (transform.position.x < pigeon.cockpitPoint.x)
        {
            velocity.x = pigeon.xVelocity;
        }
        else
        {
            Debug.Log(startTime);
            velocity.x = -1f * pigeon.xVelocity;
        }
    }

    protected void MoveIntoCockPitCosmetic()
    {

    }

    public void obtainItemEnd()
    {
        movementLock = false;
        //TODO: Return to previous layer, not base
        setAnimatorLayer((int)AnimationLayers.Base);
    }


    private void updateKnockbackTime()
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

    private void updateCutsceneKnockbackTime()
    {
        if (cutsceneKnockbackTime > 0)
        {
            cutsceneKnockbackTime--;
            if(cutsceneKnockbackTime > spiroxCutsceneKnockbackTime - 22)
            {
                targetVelocity.x = -knockbackSide;
            } 
        }
    }

    private void updateJumpBuffer()
    {
        if (bufferJumpTime > 0) bufferJumpTime--;
    }

    private void updateAttackBuffer()
    {
        if (bufferAttackTime > 0) bufferAttackTime--;
    }

    void FixedUpdate()
    {
        updateKnockbackTime();
        updateCutsceneKnockbackTime();
        updateAttackBuffer();
        updateJumpBuffer();

        velocity += gravity * Physics2D.gravity * Time.deltaTime;

        velocity.x = targetVelocity.x + conveyorModifier;

        MoveIntoCockpit();

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

        if (isGravity) Movement(move, true);

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

    /// <summary>
    /// Sets duck based on user input
    /// </summary>
    protected void checkForDuck()
    {
        if (movementLock) return;

        if (grounded && gameInputManager.GetAxis("Vertical") < -0.4 && Mathf.Abs(gameInputManager.GetAxis("Vertical")) >= Mathf.Abs(gameInputManager.GetAxis("Horizontal")) - 0.01)
        {
            animator.SetInteger("direction", 5);
            ducking = true;
        }
        else
        {
            animator.SetInteger("direction", 2);
            ducking = false;
        }
    }

    /// <summary>
    /// Jumps based on user input
    /// </summary>
    protected void checkForJump()
    {
        if (movementLock) return;

        if (gameInputManager.GetButtonDown("Jump") && grounded && !collapsed || grounded && bufferJumpTime > 0)
        {
            canCancelJump = true;
            jump(jumpTakeOffSpeed);
        }
        else if (gameInputManager.GetButtonDown("Jump") && !grounded)
        {
            bufferJumpTime = 4;
        }
        else if (gameInputManager.GetButtonUp("Jump") && canCancelJump)
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0f;
            }
        }
    }

    /// <summary>
    /// Sets Norm's speed and flip sprite based on the user's left/right input
    /// </summary>
    protected void computeVelocity()
    {
        //sets x velocity to 0 in case movement is lccked
        targetVelocity = Vector2.zero;

        //don't set x velocity if movement is locked or norm is ducking
        if (movementLock || ducking) return;

        Vector2 move = Vector2.zero;
        
        move.x = gameInputManager.GetAxis("Horizontal");

        if (move.x > 0.3)
        {
            flipSprite = false;
            move.x = 1;
        }
        else if (move.x < -0.3)
        {
            flipSprite = true;
            move.x = -1;
        }
        else
        {
            move.x = 0;
        }

        targetVelocity = move * maxSpeed;
    }

    protected void endDeathFall()
    {
        voidHolder.SetActive(true);
    }

    protected void highFall()
    {
        collapsed = true;
        movementLock = true;
    }

    IEnumerator endJump()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("jump", false);
    }

    public void getUp()
    {
        collapsed = false;
        movementLock = false;
    }

    public void jump(float speed)
    {
        velocity.y = speed;
        animator.SetBool("jump", true);
        StartCoroutine("endJump");
    }
    public void rakeAttack()
    {
        attack = true;
        if(grounded && !gameInputManager.GetButtonDown("Jump")) movementLock = true;
        flipSpriteLocked = true;
    }

    public void endRakeAttack()
    {
        attack = false;
        movementLock = false;
    }

    public void bounce()
    {
        canCancelJump = false;
        jump(jumpTakeOffSpeed * 1.1f);
    }

    public void setCanPickupStick(bool canPickupStick, GameObject stickObject)
    {
        this.canPickupStick = canPickupStick;
        this.stickObject = stickObject;
    }

    /// <summary>
    /// Sets gravity to the input value 
    /// </summary>
    /// <param name="gravity"></param>
    public virtual void modifyGravity(int gravity)
    {
        this.gravity = gravity;
    }

    /// <summary>
    /// Sets gravity to the default
    /// </summary>
    public virtual void resetGravity()
    {
        this.gravity = defaultGravity;
    }

    /// <summary>
    /// Sets the opacity value of norm to the given value
    /// </summary>
    /// <param name="opacity"></param>
    private void setOpacity(float opacity)
    {
        spriteRenderer.color = new Color(255, 255, 255, opacity);
    }

    /// <summary>
    /// Updates the active animation layer
    /// </summary>
    protected void setAnimatorLayer(int layer)
    {
        activeLayer = layer;

        for (int i = 1; i < animator.layerCount; i++)
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

    public void knockback(string direction)
    {
        if (invincibility || dead) return;

        flipSprite = (direction == "Right") ? true : false;
        knockbackTime = maxKnockbackTime;
    }

    public void cutsceneKnockback()
    {
        flipSprite = false;
        cutsceneKnockbackTime = spiroxCutsceneKnockbackTime;
    }

    public void setConveyorModifier(float conveyorModifier)
    {
        this.conveyorModifier = conveyorModifier;
    }
}
