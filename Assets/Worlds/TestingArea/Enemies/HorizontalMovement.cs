using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    //Components
    [SerializeField] Transform rightLimit;
    [SerializeField] Transform leftLimit;

    //Stats
    [SerializeField] float moveSpeed;

    //Patrol Variables
    Transform currentTarget;
    string currentTargetName;

    void Start()
    {
        currentTargetName = "Left";
        currentTarget = leftLimit;
    }

    private void Update()
    {
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
        flipDirectionAdditional();
    }

    protected virtual void flipDirectionAdditional()
    {

    }
}
