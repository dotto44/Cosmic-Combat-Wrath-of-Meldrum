using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoCaveSpawnManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    GameObject norm;

    [SerializeField] GameObject camera0;
    [SerializeField] GameObject camera3;

    [SerializeField] Transform normCornTransform;

    void Start()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        norm = GameObject.FindGameObjectWithTag("Player");

        UpdateSpawn();
    }

    void UpdateSpawn()
    {
        if (sceneLoader.lastScene == "PlutoCorn")
        {
            SpawnAtSpirox();
        }
    }

    void SpawnAtSpirox()
    {
        norm.transform.position = normCornTransform.position;
        norm.GetComponent<NormMovement>().flipSprite = true;
        camera0.SetActive(false);
        camera3.SetActive(true);
    }
}
