using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionArea : MonoBehaviour
{
    [SerializeField] SpiderSportsGameManager manager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        manager.setSpiderInZone(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        manager.setSpiderInZone(false);
    }
}
