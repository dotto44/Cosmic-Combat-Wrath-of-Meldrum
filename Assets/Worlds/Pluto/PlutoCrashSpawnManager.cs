using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoCrashSpawnManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    GameObject norm;

    [SerializeField] Transform normCaveTransform;

    [SerializeField] GameObject section1;
    [SerializeField] GameObject section2;

    [SerializeField] GameObject camera0;
    [SerializeField] GameObject camera4;

    [SerializeField] GameObject snowHolder;

    void Start()
    {
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        norm = GameObject.FindGameObjectWithTag("Player");

        updateSpawn();
    }

    void updateSpawn()
    {
        if(sceneLoader.lastScene == "PlutoCave")
        {
            spawnAtCave();
        }
    }

    void spawnAtCave()
    {
        norm.transform.position = normCaveTransform.position;
        norm.GetComponent<NormMovement>().flipSprite = true;
        section1.SetActive(false);
        section2.SetActive(true);
        camera0.SetActive(false);
        camera4.SetActive(true);
        snowHolder.SetActive(false);
        StartCoroutine("rewarmSnow");
    }

    IEnumerator rewarmSnow()
    {
        yield return new WaitForSeconds(0.01f);
        snowHolder.SetActive(true);
    }
}
