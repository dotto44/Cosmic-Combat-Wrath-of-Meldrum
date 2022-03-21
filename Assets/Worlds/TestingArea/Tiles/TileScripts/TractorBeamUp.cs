using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeamUp : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<NormMovement>().modifyGravity(-2);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<NormMovement>().modifyGravity(4);
    }
}
