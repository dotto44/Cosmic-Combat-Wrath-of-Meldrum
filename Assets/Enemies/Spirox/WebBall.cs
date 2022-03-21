using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebBall : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected int direction;
    protected float moveSpeed = 8;
    protected Vector2 currentTarget;
    protected Vector2 origin;

    [SerializeField] GameObject[] webs;
    Quaternion[] webAngles;
    protected bool reversed;

    protected virtual void Awake()
    {
        webAngles = new Quaternion[4];
        webAngles[0] = Quaternion.Euler(0, 0, 0);
        webAngles[0] = Quaternion.Euler(0, 0, 90);
        webAngles[0] = Quaternion.Euler(0, 0, 180);
        webAngles[0] = Quaternion.Euler(0, 0, 270);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void setDirection(Vector2 target, Vector2 origin)
    {
        if (target.x > transform.position.x) spriteRenderer.flipX = true;
        currentTarget = target;
        this.origin = origin;
    }

    protected void FixedUpdate()
    {
        move();
    }

    protected void move()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hitNorm(collision);
            return;
        }
        else if (collision.tag == "Rake")
        {
            if (collision.gameObject.transform.parent.gameObject.transform.position.x > transform.position.x && currentTarget.x > origin.x || collision.gameObject.transform.parent.gameObject.transform.position.x < transform.position.x && currentTarget.x < origin.x)
            {
                reverse();
            }
            else
            {
                speedUp();
            }
            return;
        }

        explode(collision);
    }

    public void speedUp()
    {
        Vector2 newTarget = new Vector2(transform.position.x + 100, transform.position.y - 20);
        if (currentTarget.x < origin.x) newTarget = new Vector2(transform.position.x - 100, transform.position.y - 20);
        setDirection(newTarget, transform.position);
        moveSpeed = 12;
        reversed = true;
        gameObject.tag = "NormBullet";
        gameObject.layer = LayerMask.NameToLayer("OnlyEnemies");
    }

    public void reverse()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
        Vector2 newTarget = new Vector2(transform.position.x + 100, transform.position.y - 20);
        if(currentTarget.x > origin.x) newTarget = new Vector2(transform.position.x - 100, transform.position.y - 20);
        setDirection(newTarget, transform.position);
        moveSpeed = 10;
        reversed = true;
        gameObject.tag = "NormBullet";
        gameObject.layer = LayerMask.NameToLayer("OnlyEnemies");
    }

    protected void hitNorm(Collider2D collision)
    {
        string direction = "Left";
        if (collision.gameObject.transform.position.x > transform.position.x)
        {
            direction = "Right";
        }

        collision.gameObject.GetComponent<NormMovement>().knockback(direction);

    }

    protected void explode(Collider2D collision)
    {
        setDirection(transform.position, transform.position);
        if(transform.position.y < 6.7)
        {
            Instantiate(webs[Random.Range(0, 3)], new Vector2(transform.position.x, transform.position.y - 0.8f), webAngles[Random.Range(0, 4)]);
            
        }
        else if(transform.position.x < 93)
        {
            Instantiate(webs[Random.Range(0, 3)], new Vector2(transform.position.x - 0.9f, transform.position.y), webAngles[Random.Range(0, 4)]);
        }
        else if (transform.position.x > 114)
        {
            Instantiate(webs[Random.Range(0, 3)], new Vector2(transform.position.x + 0.9f, transform.position.y), webAngles[Random.Range(0, 4)]);
        }

            kill();
    }

    protected void kill()
    {
        Destroy(gameObject);
    }

}
