using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] GameObject camera1;
    [SerializeField] GameObject camera2;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.transform.position.x > transform.position.x)
        {
            camera2.SetActive(true);
            camera1.SetActive(false);
        }
        else
        {
            camera2.SetActive(false);
            camera1.SetActive(true);
        }
    }
}
