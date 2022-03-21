using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    Spitter spitter;
    bool isInExit = false;

    private void Awake()
    {
        spitter = GetComponentInParent<Spitter>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        spitter.SetIsInExit(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        spitter.SetIsInExit(false);
    }

}
