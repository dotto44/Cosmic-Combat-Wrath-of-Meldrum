using UnityEngine;

public class DitchDetection : MonoBehaviour
{
    EnemyPhysicsObject enemyPhysicsObject;
    private void Awake()
    {
        enemyPhysicsObject = gameObject.transform.parent.gameObject.GetComponent<EnemyPhysicsObject>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Detection") return;
        enemyPhysicsObject.gonnaFall();
    }
}
