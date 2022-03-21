using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrappyMovement : MonoBehaviour
{
    [SerializeField] Transform rightLimit;
    [SerializeField] Transform leftLimit;
    SpriteRenderer spriteRenderer;

    bool canMove = true;
    const float moveSpeed = 2;

    //Patrol Variables
    Transform currentTarget;
    string currentTargetName;


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
}
