using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorTile : MonoBehaviour
{
    [SerializeField] float conveyorModifier;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Mathf.Approximately(collision.GetContact(0).normal.y, -1))
        {
            collision.gameObject.GetComponent<NormMovement>().setConveyorModifier(conveyorModifier);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<NormMovement>().setConveyorModifier(0);
    }
}
