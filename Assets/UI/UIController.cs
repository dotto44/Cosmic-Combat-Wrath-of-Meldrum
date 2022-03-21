using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] Image[] healthBobbles;
    [SerializeField] Animator[] bobbleAnimators;
    [SerializeField] Animator headAnimator;
    [SerializeField] TMP_Text beanText;
    [SerializeField] Animator beanAnimator;

    bool firstBean = true;

    int maxHealth = 6;

    private int _health;
    public int health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
        }
    }

    private int _beans;
    public int beans
    {
        get
        {
            return _beans;
        }
        set
        {
            beanText.text = value.ToString("0000");
            _beans = value;
        }
    }

    void Start()
    {
        health = maxHealth;
        setExcessBobblesInactive(maxHealth);
        updateAllBobbles();
    }

    public void tookDamage(int amt)
    {
        if (health <= 0) return;

        health -= amt;
        for(int i = health; i < health + amt; i++)
        {
            bobblePop(i);
        }
        headAnimator.CrossFade("Hurt", 0.0f);

        StartCoroutine("addOneHealthEffectDelayed");
    }

    public void collectedBean(string color)
    {
        beans += 1;

        if (firstBean)
        {
            firstBean = false;
            beanAnimator.CrossFade("FadeIn", 0.0f);
        }
        else if (!beanAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeIn") && !beanAnimator.GetCurrentAnimatorStateInfo(0).IsName("Faded"))
        {
            beanAnimator.CrossFade(color, 0.0f, 0, 0);
        }
    }

    private void updateAllBobbles()
    {
        for(int i = 0; i < maxHealth; i++)
        {
            updateBobble(i);
        }
    }

    private void bobblePop(int index)
    {

        bobbleAnimators[index].CrossFade("LoseHealth", 0.0f);
    }

    void updateBobble(int index)
    {
        if (index >= maxHealth)
        {
            return;
        }
        else if (index == 0 && health == 1)
        {
            bobbleAnimators[index].CrossFade("LastBobble", 0.0f);
        }
        else if(index < health)
        {
            bobbleAnimators[index].CrossFade("Full", 0.0f);
        }
        else
        {
            bobbleAnimators[index].CrossFade("Empty", 0.0f);
        }
    }

    void setExcessBobblesInactive(int count)
    {
        for(int i = 9; i >= maxHealth; i--)
        {
            healthBobbles[i].gameObject.SetActive(false);
        }
    }

    IEnumerator addOneHealthEffectDelayed()
    {
        yield return new WaitForSeconds(0.6f);
        updateBobble(0);
    }
}
