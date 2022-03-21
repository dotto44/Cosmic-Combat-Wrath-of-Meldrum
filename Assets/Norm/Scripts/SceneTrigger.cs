using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] GameObject area1;
    [SerializeField] GameObject area2;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.position.x > transform.position.x)
        {
            if (area2 != null) area2.SetActive(true);
            if (area1 != null) area1.SetActive(false);
        }
        else
        {
            if (area2 != null) area2.SetActive(false);
            if (area1 != null) area1.SetActive(true);
        }
    }
}
