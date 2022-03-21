using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D;
    Animator animator;

    [SerializeField] GameObject corpsePrefab;

    [SerializeField] int health;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void GotHit(Vector2 pos)
    {
        health -= 1;
        if(health > 0)
        {
            animator.SetBool("hurt", true);
            rigidbody2D.AddForce(pos * 1000);
            flash();
        }
        else
        {
           die(pos);
        }
        
        
    }

    public void flash()
    {
        StopAllCoroutines();
        spriteRenderer.material.SetFloat("_FlashAmount", 0.75f);
        StartCoroutine(endFlash());
    }

    public void die(Vector2 pos)
    {
        StopAllCoroutines();
        GameObject corpse = Instantiate(corpsePrefab, new Vector2(transform.position.x, transform.position.y + 0.5f), transform.rotation);
        /*if(pos.x > transform.position.x)
        {
            corpse.GetComponent<Rigidbody2D>().AddForce(new Vector2(600, 400));
        }
        else
        {
            corpse.GetComponent<Rigidbody2D>().AddForce(new Vector2(-600, 400));
        }*/
        corpse.GetComponent<Rigidbody2D>().AddForce(pos * 3000);
        if (spriteRenderer.GetComponent<SpriteRenderer>().flipX)
        {
            corpse.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        Destroy(gameObject);
    }

    IEnumerator endFlash()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
    }
}
