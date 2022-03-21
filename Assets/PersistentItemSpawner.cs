using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject uiPrefab;
    [SerializeField] GameObject loadPrefab;

    void Awake()
    {
        if (!GameObject.FindGameObjectWithTag("UIHolder"))
        {
            Instantiate(uiPrefab);
        }
        if (!GameObject.FindGameObjectWithTag("SceneLoader"))
        {
            Instantiate(loadPrefab);
        }
    }
}
