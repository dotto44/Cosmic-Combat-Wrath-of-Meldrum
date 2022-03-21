using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Camera cam;

    Vector3 startPosition;
    Vector2 camStartPosition;

    Vector2 travel => (Vector2)cam.transform.position - camStartPosition;

   
    [SerializeField] float parallaxFactor;

    void Start()
    {
        startPosition = transform.position;
        camStartPosition = cam.transform.position;
    }

    void Update()
    {
        transform.position = startPosition + new Vector3((int)(travel.x / 0.0625f * parallaxFactor) * 0.0625f, (int)(travel.y / 0.0625f * parallaxFactor) * 0.0625f, 0);
    }
}
