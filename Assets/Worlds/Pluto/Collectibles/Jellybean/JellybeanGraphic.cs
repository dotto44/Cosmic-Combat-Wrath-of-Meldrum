using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellybeanGraphic : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2d;

    string[] colors = { "Blue", "Red", "Yellow" };

    int beanColor;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        beanColor = Random.Range(0, 3);
        animator.CrossFade(colors[beanColor], 0.0f, 0, Random.Range(0, 100) * 0.01f);
        
    }

    public void collect()
    {
        GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>().collectedBean(colors[beanColor]);
        animator.CrossFade("JellybeanCollect", 0.0f, 0, 0);
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void kill()
    {
        Destroy(gameObject);
    }
}
