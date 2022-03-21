using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyBean : MonoBehaviour
{
    bool canCollect;

    private void Start()
    {
        StartCoroutine("waitToCollect");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canCollect) return;
        canCollect = false;
        gameObject.transform.parent.gameObject.GetComponent<JellybeanGraphic>().collect();
    }

    IEnumerator waitToCollect()
    {
        yield return new WaitForSeconds(0.01f);
        canCollect = true;
    }
}
