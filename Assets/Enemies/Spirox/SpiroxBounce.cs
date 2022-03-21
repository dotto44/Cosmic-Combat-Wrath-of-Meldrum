using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiroxBounce : BounceTile
{
    Animator anim;
    [SerializeField] Animator headAnim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.gameObject.tag == "Rake")
        {
            anim.CrossFade("BodyShake", 0.0f, 0, 0);
        }       
    }

    protected override void normBounce(Collision2D collision)
    {
        base.normBounce(collision);

        anim.CrossFade("BodyShake", 0.0f);
    }

    private void returnToSleep()
    {
        anim.CrossFade("BodySleeping", 0.0f, 0, headAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }
}
