using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Teleporter : MonoBehaviour
{
    Animator anim;
    bool unlocked;
    bool unlocking;

    void Awake()
    {
        unlocked = false;
        unlocking = false;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (getTeleporterUnlocked())
        {
            instantUnlock();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!unlocked)
        {
            unlocked = true;
            unlocking = true;
            anim.CrossFade("Unlocking", 0.0f);
            return;
        }

        if (unlocking) return;

        anim.CrossFade("WalkBy", 0.0f);
    }

    void instantUnlock()
    {
        unlocked = true;
        anim.CrossFade("Unlocked", 0.0f);
    }

    void finishedUnlocking()
    {
        setTeleporterUnlocked();
        unlocking = false;
        anim.CrossFade("Unlocked", 0.0f);
    }

    void finishedWalkBy()
    {
        anim.CrossFade("Unlocked", 0.0f);
    }

    abstract protected bool getTeleporterUnlocked();

    abstract protected void setTeleporterUnlocked();

}
