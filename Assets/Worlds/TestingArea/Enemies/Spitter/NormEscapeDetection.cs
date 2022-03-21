using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormEscapeDetection : MonoBehaviour
{
    EnemyPhysicsObject enemyPhysicsObject;

    private void Awake()
    {
        enemyPhysicsObject = gameObject.transform.parent.gameObject.GetComponent<EnemyPhysicsObject>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemyPhysicsObject.normEscapedDetection();
    }
}
