using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class JellybeanCounter : MonoBehaviour
{
    int count = 0;

    [SerializeField] TMP_Text text;

    public void augmentCounter()
    {
        count++;
        text.text = "" + count;
    }
}
