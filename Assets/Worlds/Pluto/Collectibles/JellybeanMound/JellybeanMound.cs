using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JellybeanMound : MonoBehaviour
{
    Animator animator;

    int health = 4;
    int[] spawnCounts = { 4, 4, 3, 2, 2 };


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        setMoundState();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Rake")
        {
            GameObject.FindGameObjectWithTag("JellybeanSpawner").GetComponent<JellybeanSpawner>().spawnBeansFromMound(transform.position, GetComponent<BoxCollider2D>().bounds, spawnCounts[health]);
            health--;
            setStatusOfMound(health);
            if (health < 1)
            {
                Destroy(gameObject);
            }
            else
            {
                animator.CrossFade("MoundShake", 0.0f);
            }
        }
    }

    void setMoundState()
    {
        health = getStatusOfMound();

        if (health == 0) Destroy(gameObject);
    }

    abstract protected int getStatusOfMound();

    abstract protected void setStatusOfMound(int health);

}
