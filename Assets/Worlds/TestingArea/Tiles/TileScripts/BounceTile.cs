using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceTile : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            checkNormBounce(collision);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyBounce(collision);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyBounce(collision);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            checkNormBounce(collision);
        }

       
    }

    protected void enemyBounce(Collider2D collision)
    {
        collision.gameObject.GetComponent<EnemyPhysicsObject>().bounce();
    }

    protected virtual void checkNormBounce(Collision2D collision)
    {
        //Checks if Norm hits the top of the block, if so bounce him.
        if (Mathf.Approximately(collision.GetContact(0).normal.y, -1))
        {
            normBounce(collision);
        }
    }

    protected virtual void normBounce(Collision2D collision)
    {
        collision.gameObject.GetComponent<NormMovement>().bounce();
    }
}
