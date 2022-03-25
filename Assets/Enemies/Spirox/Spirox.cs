using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Spirox : EnemyPhysicsObject
{

    [SerializeField] GameObject separateBody;
    [SerializeField] GameObject spiroxName;
    [SerializeField] GameObject shootLeftPos;
    [SerializeField] GameObject shootRightPos;
    [SerializeField] GameObject shockwaveRightPos;
    [SerializeField] GameObject shockwaveLeftPos;

    [SerializeField] GameObject webBall;
    [SerializeField] GameObject shockwave;
    [SerializeField] Animator[] caveChunks;
    [SerializeField] FloorPieces floorPieces;

    [SerializeField] CinemachineImpulseSource yellCameraImpulse;
    [SerializeField] CinemachineImpulseSource landCameraImpulse;
    [SerializeField] CinemachineImpulseSource dieCameraImpulse;

    GameObject norm;
    Animator animator;

    

    bool firstHit = true;
    string state = "";
    string previousState = "";
    float targetVelocityX = 0;

    const float maxSpeedConst = 10f;
    float maxSpeed = maxSpeedConst;
    int idleLoops = 0;
    int hangIdleLoops = 0;
    int hangChargeLoops = 0;
    int stunLoops = 0;
    int numberOfShots = 0;
    int numberOfCharges = 0;

    int numberOfLoops = 0;
    bool finalSmash;

    int chargeDirection = -1;

    Vector2 lockedShotPosition;

    private void Awake()
    {
        norm = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        movesNormallyInAir = true;
    }

    public override void Start()
    {
        base.Start();

        if (DataService.Instance.saveData.getBeatSpirox())
        {
            floorPieces.destroyAll();
            Destroy(caveChunks[0].gameObject);
            caveChunks[1].enabled = true;
            caveChunks[1].CrossFade("WallChunkFall", 0, 0, 1);
            caveChunks[2].enabled = true;
            caveChunks[2].CrossFade("WallChunkFall2", 0, 0, 1);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void updateState()
    {
        //always called
        switch (state)
        {
            case "WakeUp":
                targetVelocityX = 0;
                break;
            case "HangIdle":
                targetVelocityX = 0;
                break;
            case "HangChargeRight":
                targetVelocityX = 0;
                break;
            case "HangChargeLeft":
                targetVelocityX = 0;
                break;
            case "HangingGrowl":
                targetVelocityX = 0;
                break;
            case "Stun":
                targetVelocityX = 0;
                break;
            case "ScrapeLeg":
                targetVelocityX = 0;
                break;
            case "Idle":
                targetVelocityX = 0;
                break;
            case "WebJump":
                targetVelocityX = 0;
                break;
            case "Growl":
                targetVelocityX = 0;
                break;
            case "Scream":
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
            case "UhOh":
                break;
            case "Charge":
                targetVelocityX = chargeDirection;
                maxSpeed = maxSpeedConst;
                if (transform.localPosition.x < -7 && chargeDirection < 0 || transform.localPosition.x > 4.5 && chargeDirection > 0)
                {
                    state = "Slide";
                }
                
                if (numberOfCharges <= 0 && (transform.localPosition.x < 3.5 && chargeDirection < 0 || transform.localPosition.x > -6 && chargeDirection > 0))
                {
                    state = "Slide";
                }

                break;
            case "Slide":
                targetVelocityX = chargeDirection;
                if(maxSpeed > 1) maxSpeed *= 0.97f;
                if (numberOfCharges <= 0 && transform.localPosition.x > -0.6 && transform.localPosition.x < -0.4)
                {
                    state = "WebJump";
                }
                else if (numberOfCharges > 0 && (transform.localPosition.x > -7 && transform.localPosition.x < 4.5 || transform.localPosition.x < -7 && chargeDirection > 0 || transform.localPosition.x > 4.5 && chargeDirection < 0))
                {
                    state = "Charge";
                }
                else if(numberOfCharges > 0 &&  (transform.localPosition.x < -10 && chargeDirection < 0|| transform.localPosition.x > 7.7 && chargeDirection > 0))
                {
                    switchDirection();
                    numberOfCharges--;
                }
               
                break;
            default:
                break;
        }
        
        if (previousState == state)
        {
            return;
        }

        switch (state)
        {
            case "WakeUp":
                modifyGravity(0);
                separateBody.SetActive(false);
                break;
            case "Scream":
                spiroxName.SetActive(true);
                yellCameraImpulse.GenerateImpulseAt(transform.position, new Vector3(0, 1, 0));
                for (int i = 0; i < caveChunks.Length; i++)
                    caveChunks[i].enabled = true;
                break;
            case "HangChargeRight":
                lockedShotPosition = norm.transform.position;
                transform.localPosition = new Vector2(transform.localPosition.x, 7.2f);
                break;
            case "HangChargeLeft":
                lockedShotPosition = norm.transform.position;
                transform.localPosition = new Vector2(transform.localPosition.x, 7.2f);
                break;
            case "HangShootRight":
                shoot(shootRightPos.transform.position);
                transform.localPosition = new Vector2(transform.localPosition.x, 7.2f);
                break;
            case "HangShootLeft":
                shoot(shootLeftPos.transform.position);
                transform.localPosition = new Vector2(transform.localPosition.x, 7.2f);
                break;
            case "HangingGrowl":
                transform.localPosition = new Vector2(transform.localPosition.x, 7.2f);
                break;
            case "HangIdle":
                transform.localPosition = new Vector2(transform.localPosition.x, 7.2f);
                break;
            case "HangIdle2":
                transform.localPosition = new Vector2(transform.localPosition.x, 7.2f);
                break;
            case "Fall":
                transform.localPosition = new Vector2(transform.localPosition.x, 0.0f);
                break;
            default:
                break;
        }

        animator.CrossFade(state, 0.0f);
        previousState = state;

    }

    private void shoot(Vector2 pos)
    {
        GameObject spitBullet = Instantiate(webBall, pos, transform.rotation);
        spitBullet.GetComponent<WebBall>().setDirection(new Vector2(lockedShotPosition.x + (lockedShotPosition.x - pos.x) * 20, lockedShotPosition.y + (lockedShotPosition.y - pos.y) * 20), pos);
    }

    protected override void ComputeVelocity()
    {
        updateState();

        Vector2 move = Vector2.zero;

        move.x = targetVelocityX;
        
        targetVelocity = move * maxSpeed;
    }

    public override void tookDamage(Collider2D collision)
    {
        if(firstHit)
        {
            norm.GetComponent<NormMovement>().cutsceneKnockback();
            state = "WakeUp";
            firstHit = false;
            GotHit(new Vector2(0, 0), 0, false);
            return;
        }

        GotHit(new Vector2(0, 0), 0, true);
    }

    public override void die(Vector2 pos)
    {
        knockback(pos, 0);
        finalSmash = true;
    }

    private void spawnShockwaves()
    {
        if (finalSmash)
        {
            floorPieces.staggerFloorPieces();
            modifyGravity(6);
            dieCameraImpulse.GenerateImpulseAt(transform.position, new Vector3(0, -1, 0));
            return;
        }

        landCameraImpulse.GenerateImpulseAt(transform.position, new Vector3(0, -1, 0));
        GameObject shockwaveR = Instantiate(shockwave, shockwaveRightPos.transform.position, transform.rotation);
        shockwaveR.GetComponent<Shockwave>().setDirection(-2 * chargeDirection);
        GameObject shockwaveL = Instantiate(shockwave, shockwaveLeftPos.transform.position, transform.rotation);
        shockwaveL.GetComponent<Shockwave>().setDirection(2 * chargeDirection);
    }

    private void faceNorm()
    {
        if(norm.transform.position.x < transform.position.x && chargeDirection > 0 || norm.transform.position.x > transform.position.x && chargeDirection < 0)
        {
            switchDirection();
        }
        else
        {
            state = "ScrapeLeg";
        }
    }

    private void switchDirection()
    {
        state = "Turn";
    }

    private void endGrowl()
    {
        state = "Scream";
    }

    private void endTurn()
    {
        StartCoroutine("nextFrame");
        state = "ScrapeLeg";
    }

    private void endHangingGrowl()
    {
        state = "Fall";
    }

    private void endFall()
    {
        stunLoops = 8;
        if (!finalSmash)
        {
            state = "Stun";
        }
        else
        {
            state = "UhOh";
        }
       
    }

    private void endStun()
    {
        stunLoops--;
        if (stunLoops <= 0)
        {
            state = "Shake";
        }
    }

    private void endShake()
    {
        idleLoops = 3;
        faceNorm();
        numberOfCharges = 3;
        if (health < 11 && numberOfLoops != 1)
        {
            numberOfCharges = 5;
            maxSpeed = maxSpeedConst + 1;
        }
    }

    private void endIdle()
    {
        idleLoops--;
        if (idleLoops <= 0)
        {
            state = "ScrapeLeg";
        }
    }

    private void endLegScrape()
    {
        state = "Charge";
    }

    private void endHangIdle()
    {
        hangIdleLoops--;
        if(hangIdleLoops <= 0)
        {
            numberOfShots = 5;
            hangChargeLoops = 3;
            if (health < 11)
            {
                numberOfShots = 6;
                hangChargeLoops = 2;
            }
            chooseHangIdle();
        }
    }
    
    private void endHangIdle2()
    {
        hangIdleLoops--;
        if (hangIdleLoops <= 0)
        {
            state = "HangingGrowl";
        }
    }

    private void endShoot()
    {
        numberOfShots--;
        if(numberOfShots <= 0)
        {
            hangIdleLoops = 2;
            if (health < 11) hangIdleLoops--;
            state = "HangIdle2";
            return;
        }

        hangChargeLoops = 3;
        chooseHangIdle();
    }

    private void chooseHangIdle()
    {
        if (norm.transform.position.x > transform.position.x && chargeDirection < 0 || norm.transform.position.x < transform.position.x && chargeDirection > 0)
        {
            state = "HangChargeRight";
        }
        else
        {
            state = "HangChargeLeft";
        }
    }

    private void endHangCharge()
    {
        hangChargeLoops--;
        if (hangChargeLoops <= 0)
        {
            if (state == "HangChargeRight")
            {
                state = "HangShootRight";
            }
            else
            {
                state = "HangShootLeft";
            }
        }
    }

    private void endScream()
    {
        state = "WebJump";
    }

    private void endWebJump()
    {
        numberOfLoops++;
        hangIdleLoops = 3;
        state = "HangIdle";
    }

    private void endWakeup()
    {
        state = "Growl";
    }

    IEnumerator nextFrame()
    {
        yield return new WaitForEndOfFrame();

        chargeDirection *= -1;
        if (Mathf.Approximately(transform.localRotation.y, 0))
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
