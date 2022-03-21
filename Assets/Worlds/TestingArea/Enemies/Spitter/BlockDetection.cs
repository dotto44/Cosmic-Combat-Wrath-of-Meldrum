using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetection : MonoBehaviour
{
    EnemyPhysicsObject enemyPhysicsObject;
    private void Awake()
    {
        enemyPhysicsObject = gameObject.transform.parent.gameObject.GetComponent<EnemyPhysicsObject>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Detection" || collision.tag == "EnemyBullet") return;
        enemyPhysicsObject.blocked();
    }
}
