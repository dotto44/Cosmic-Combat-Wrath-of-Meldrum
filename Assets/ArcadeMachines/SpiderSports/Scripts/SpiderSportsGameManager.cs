using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderSportsGameManager : MonoBehaviour
{
    //Spider
    [SerializeField] Transform spiderSpawn;
    [SerializeField] GameObject spider;
    Rigidbody2D spiderRigidbody;

    //Thrower
    [SerializeField] GameObject thrower;
    Animator throwerAnim;

    //RedX
    [SerializeField] GameObject redX;
    Animator redXAnim;

    //Power Meter Cursor
    [SerializeField] GameObject cursor;
    Animator cursorAnim;

    //Stats
    [SerializeField] int speedMultiplier;
    bool spiderInZone;
    int goalCount;
    int timeInGoal;
    int stageOfShoot;
    bool controlsLocked;
    private void Awake()
    {
        spiderRigidbody = spider.GetComponent<Rigidbody2D>();
        throwerAnim = thrower.GetComponent<Animator>();
        redXAnim = redX.GetComponent<Animator>();
        cursorAnim = cursor.GetComponent<Animator>();
    }

    private void Start()
    {
        cursorAnim.speed = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !controlsLocked)
         {
             fireSpider();
         }
       /* if (Input.GetMouseButtonDown(0))
        {
            if(stageOfShoot == 0)
            {
                freezeTrajectory();
            }
            else if(stageOfShoot == 1)
            {
                freezeCursor();
            }
            else
            {
                fireSpiderAltControls();
            }
        }*/
        if (GameInputManager.GetKeyUp("Up"))
        {
            resetSpider();
        }

        if(spiderInZone && Mathf.Abs(spiderRigidbody.velocity.x) < 0.01 && Mathf.Abs(spiderRigidbody.velocity.y) < 0.01)
        {
            timeInGoal++;
        } else
        {
            timeInGoal = 0;
        }

        if(timeInGoal > 5)
        {
            nextGoal();
        }
        if(spider.transform.position.y < -6)
        {
            resetSpider();
        }
    }
   /* private void fireSpiderAltControls()
    {
        stageOfShoot = 0;
        
        float power = (cursor.transform.localPosition.x + 0.475f) * 1000 + 400;

        float dx = (redX.transform.position.x - spiderSpawn.position.x) / 2f  * power;
        float dy = (redX.transform.position.y - spiderSpawn.position.y) / 2f * power;

        Vector2 force = new Vector2(dx, dy);

        spiderRigidbody.gravityScale = 1.8f;
        spiderRigidbody.AddForce(force);

        throwerAnim.CrossFadeInFixedTime("Throw", 0.0f);
    }*/

    private void fireSpider()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        Debug.Log(lookPos.x + ", " + lookPos.y);
        float dx = (lookPos.x - spiderSpawn.position.x)/6 * speedMultiplier;
        float dy = (lookPos.y - spiderSpawn.position.y)/6 * speedMultiplier;

        dx = dx > 1000 ? 1000 : dx;
        dy = dy > 1000 ? 1000 : dy;

        Vector2 force = new Vector2(dx, dy);

        spiderRigidbody.gravityScale = 1.6f;
        spiderRigidbody.AddForce(force);

        throwerAnim.CrossFadeInFixedTime("Throw", 0.0f);

        controlsLocked = true;
    }

    private void resetSpider()
    {
        controlsLocked = false;
        spiderRigidbody.gravityScale = 0;
        spiderRigidbody.velocity = new Vector2(0, 0);
        spiderRigidbody.angularVelocity = 0;
        spider.transform.rotation = Quaternion.Euler(0, 0, 0);
        spider.transform.position = spiderSpawn.position;
    }

    /*private void freezeTrajectory()
    {
        redXAnim.speed = 0;
        cursorAnim.speed = 1;
        stageOfShoot++;
    }

    private void freezeCursor()
    {
        cursorAnim.speed = 0;
        stageOfShoot++;
    }*/

    private void nextGoal()
    {
        goalCount++;
        resetSpider();
        newGoal();
    }

    private void newGoal()
    {
        
    }

    public void setSpiderInZone(bool spiderInZone)
    {
        this.spiderInZone = spiderInZone;
    }
}
