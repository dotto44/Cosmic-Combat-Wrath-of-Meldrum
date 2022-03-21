using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthCounter : MonoBehaviour
{
    int health = 5;

    [SerializeField] TMP_Text text;
    [SerializeField] GameObject deadText;
    [SerializeField] GameObject norm;
    public void augmentCounter()
    {
        health--;
        text.text = "" + health;

        if(health < 1)
        {
            deadText.SetActive(true);
            norm.SetActive(false);
            StartCoroutine("restartGame");
        }
    }

    IEnumerator restartGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("TestingGround");
    }
}
