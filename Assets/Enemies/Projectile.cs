using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Animator animator;

    protected int direction;
    protected float moveSpeed = 6;
    protected Vector2 currentTarget;

    protected bool reversed;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void setDirection(int direction)
    {
        this.direction = direction;
        currentTarget = new Vector2(99999 * direction, transform.position.y);
    }

    protected void Update()
    {
        move();
    }

    protected void move()
    {
        if (direction == 0) return;
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hitNorm(collision);
            explode();
        }
        else if (collision.tag == "Ground" || collision.tag == "Ground")
        {
            explode();
        }
        else if (collision.tag == "Rake")
        {
            hitWithWeapon(collision);
        }
        else if (collision.tag == "Enemy" && reversed)
        {
            explode();
        }
        else if (collision.tag == "NormBullet")
        {
            explode();
        }

    }

    protected void hitWithWeapon(Collider2D collision)
    {
        if (collision.gameObject.transform.parent.gameObject.transform.position.x > transform.position.x && direction > 0 || collision.gameObject.transform.parent.gameObject.transform.position.x < transform.position.x && direction < 0)
        {
            reverse();
        }
        else
        {
            speedUp();
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

    protected void explode()
    {
        setDirection(0);
        animator.SetBool("explode", true);
    }

    protected void reverse()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
        setDirection(direction * -1);
        moveSpeed = 8;
        reversed = true;
        gameObject.tag = "NormBullet";
        gameObject.layer = LayerMask.NameToLayer("OnlyEnemies");
    }

    protected void speedUp()
    {
        moveSpeed = 10;
        reversed = true;
        gameObject.tag = "NormBullet";
        gameObject.layer = LayerMask.NameToLayer("OnlyEnemies");
    }

    protected void kill()
    {
        Destroy(gameObject);
    }
}
