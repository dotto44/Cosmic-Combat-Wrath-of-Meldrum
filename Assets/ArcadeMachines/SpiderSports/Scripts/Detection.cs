using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    Spitter spitter;

    private void Awake()
    {
        spitter = GetComponentInParent<Spitter>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        spitter.InField();
    }
}
