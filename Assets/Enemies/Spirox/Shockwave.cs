using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;

    protected int direction;
    protected float moveSpeed = 7;
    protected Vector2 currentTarget;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("kill");
    }

    public virtual void setDirection(int direction)
    {
        Vector2 target = new Vector2(transform.position.x + 100 * direction, transform.position.y);
        currentTarget = target;

        if (direction < 0) spriteRenderer.flipX = true;
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

    protected IEnumerator kill()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
