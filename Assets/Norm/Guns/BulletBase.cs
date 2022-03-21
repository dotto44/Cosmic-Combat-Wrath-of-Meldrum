using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletBase : MonoBehaviour
{
    [SerializeField] Rigidbody2D rigidBody2D;

    public abstract int damage { get; }
    public abstract float speed { get; }
    public abstract float duration { get; }

    public virtual void shoot(Vector2 dir)
    {
        float value = 0.07f;
        float xR = Random.Range(-value, value);
        float yR = Random.Range(-value, value);
        rigidBody2D.AddForce(new Vector2(dir.x, dir.y + yR) * speed);
        Destroy(gameObject, duration);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //collision.gameObject.GetComponent<Enemy>().GotHit(collision.contacts[0].point);
        }
        
        Destroy(gameObject);
    }
}
