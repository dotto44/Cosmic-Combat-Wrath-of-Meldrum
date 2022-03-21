using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ducks : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Vector2 currentTarget;

    private void Start()
    {
        currentTarget = new Vector2(transform.position.x, 6);
    }
    private void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, step);

        if (Mathf.Abs(transform.position.y - currentTarget.y) < 0.1f)
        {
            transform.position = new Vector2(transform.position.x, -6);
        }
    }


}
