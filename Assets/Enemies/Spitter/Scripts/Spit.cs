using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    Animator animator;

    int direction;
    float moveSpeed = 6;
    Vector2 currentTarget;

    bool reversed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void setDirection(int direction)
    {
        this.direction = direction;
        currentTarget = new Vector2(99999 * direction, transform.position.y);
    }

    private void FixedUpdate()
    {
        move();
    }

    private void move()
    {
        if (direction == 0) return;
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            hitNorm(collision);
            explode();
        }
        else if(collision.tag == "Ground" || collision.tag == "Ground")
        {
            explode();
        }
        else if(collision.tag == "Rake")
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
        else if(collision.tag == "Enemy" && reversed)
        {
            explode();
        }
        else if(collision.tag == "NormBullet")
        {
            explode();
        }
        
    }

    private void hitNorm(Collider2D collision)
    {
        string direction = "Left";
        if (collision.gameObject.transform.position.x > transform.position.x)
        {
            direction = "Right";
        }

        collision.gameObject.GetComponent<NormMovement>().knockback(direction);
       
    }

    public void explode()
    {
        setDirection(0);
        animator.SetBool("explode", true);
    }

    public void reverse()
    {
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
        setDirection(direction * -1);
        moveSpeed = 8;
        reversed = true;
        gameObject.tag = "NormBullet";
        gameObject.layer = LayerMask.NameToLayer("OnlyEnemies");
    }

    public void speedUp()
    {
        moveSpeed = 10;
        reversed = true;
        gameObject.tag = "NormBullet";
        gameObject.layer = LayerMask.NameToLayer("OnlyEnemies");
    }

    public void kill()
    {
        Destroy(gameObject);
    }
}
