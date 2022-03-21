using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrapFlapper : MonoBehaviour
{
    [SerializeField] GameObject corpsePrefab;
    [SerializeField] Transform rightLimit;
    [SerializeField] Transform leftLimit;

    //Components
    Enemy healthScript;
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rigidbody2d;

    //stats
    [SerializeField] int health = 3;
    const float moveSpeed = 2;
    bool canMove = true;

    //Patrol Variables
    Transform currentTarget;
    string currentTargetName;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthScript = GetComponent<Enemy>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }


    /* override protected void flipDirectionAdditional() {
         spriteRenderer.flipX = !spriteRenderer.flipX;
     }*/
    

    void Start()
    {
        currentTargetName = "Left";
        currentTarget = leftLimit;
    }

    private void Update()
    {
        move();
    }

    private void move()
    {
        if (!canMove) return;
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, step);

        if (Mathf.Abs(transform.position.x - currentTarget.position.x) < 0.1f)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hitNorm(collision);
        }
        else if (collision.tag.Substring(0, 5) == "Boots")
        {
            tookDamage(collision);
        }
    }
    private void hitNorm(Collider2D collision)
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
    private void tookDamage(Collider2D collision)
    {
        int multiplier = 1;

        float dx = collision.gameObject.transform.localPosition.x;
        
        float dy = collision.gameObject.transform.localPosition.y;
        string tag = collision.gameObject.tag;
        if (tag == "BootsJump")
        {
            //do nothing
        }
        else if (tag == "BootsDash")
        {
            //do nothing
        }
        else
        {
            dx = Mathf.Approximately(collision.gameObject.transform.parent.rotation.y, 0) ? dx : dx * -1;
        }
        if (tag == "BootsSlide")
        {
            multiplier = 2;
          //  collision.gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<NormMovement>().slideKnockback();

        }

        GotHit(new Vector2(dx, dy).normalized, multiplier);
    }

    public void GotHit(Vector2 pos, int multiplier)
    {
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

    private void knockback(Vector2 pos, int multiplier)
    {
        animator.SetBool("hurt", true);
        rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2d.AddForce(pos * 1000 * multiplier);
        canMove = false;
        flash();
    }

    private void die(Vector2 pos)
    {
        StopAllCoroutines();
        GameObject corpse = Instantiate(corpsePrefab, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation);

        corpse.GetComponent<Rigidbody2D>().AddForce(pos * 3000);
        if (spriteRenderer.GetComponent<SpriteRenderer>().flipX)
        {
            corpse.GetComponent<SpriteRenderer>().flipX = true;
        }

        Destroy(gameObject);
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
        StartCoroutine(endKnockback());
    }

    IEnumerator endKnockback()
    {
        yield return new WaitUntil(isStationary);
        rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
        rigidbody2d.velocity = new Vector2(0, 0);
        animator.SetBool("hurt", false);
        canMove = true;
    }

    bool isStationary()
    {
        return Mathf.Abs(rigidbody2d.velocity.x) < 1 && Mathf.Abs(rigidbody2d.velocity.y) < 1;
    }
}