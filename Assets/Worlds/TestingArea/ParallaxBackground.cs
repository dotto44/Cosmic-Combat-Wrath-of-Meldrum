using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] double amt = 1.0;
    Vector3 lastPos;
    Vector3 tempPos;

    void Start()
    {
        lastPos = cameraTransform.position;
    }


    void Update()
    {
        if (lastPos != cameraTransform.position)
        {
            tempPos = gameObject.transform.position;
            tempPos.x += (float)((cameraTransform.position.x - lastPos.x) / amt);
            tempPos.y += (float)((cameraTransform.position.y - lastPos.y) / amt);
            gameObject.transform.position = tempPos;

            lastPos = cameraTransform.position;
        }
    }

}
