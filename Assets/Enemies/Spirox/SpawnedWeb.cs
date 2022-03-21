using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedWeb : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine("waitToStart");
    }

    IEnumerator waitToStart()
    {
        yield return new WaitForSeconds(2.00f);
        StartCoroutine("dropTransparency");
    }


        IEnumerator dropTransparency()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - 0.1f);

        if(spriteRenderer.color.a <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine("dropTransparency");
        }
    }
}
