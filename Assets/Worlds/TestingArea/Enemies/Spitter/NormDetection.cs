using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormDetection : MonoBehaviour
{
    EnemyPhysicsObject enemyPhysicsObject;

    private void Awake()
    {
        enemyPhysicsObject = gameObject.transform.parent.gameObject.GetComponent<EnemyPhysicsObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
         enemyPhysicsObject.detectedNorm();
    }

}
