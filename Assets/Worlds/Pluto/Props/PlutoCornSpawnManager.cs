using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoCornSpawnManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    GameObject norm;

    [SerializeField] Pigeon pigeon;

    void Start()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        norm = GameObject.FindGameObjectWithTag("Player");

        UpdateSpawn();
    }

    void UpdateSpawn()
    {
        if (sceneLoader.lastScene.Substring(0,5) != "Pluto")
        {
            SpawnAtPigeon();
        }
    }

    void SpawnAtPigeon()
    {
        pigeon.Land();
    }
}
