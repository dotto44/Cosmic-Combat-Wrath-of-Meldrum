using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerDitchDetection : MonoBehaviour
{
    Walker walker; 

    private void Awake()
    {
        walker = gameObject.transform.parent.gameObject.GetComponent<Walker>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        walker.overLedge = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        walker.overLedge = true;
    }
}
