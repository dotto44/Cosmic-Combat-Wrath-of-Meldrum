using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoCornSpawnManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    GameObject norm;

    [SerializeField] Pigeon pigeon;
    [SerializeField] Transform normCliffTransform;

    [SerializeField] GameObject snowHolder;

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
        else if(DataService.Instance.saveData.getBeatSpirox())
        {
            SpawnOnCliff();
        }

        if (!DataService.Instance.saveData.getBeatSpirox())
        {
            DataService.Instance.saveData.setBeatSpirox();
        }
    }

    void SpawnAtPigeon()
    {
        pigeon.Land();
        norm.GetComponent<NormMovement>().PlaceIntoCockpit(pigeon);
        snowHolder.SetActive(false);
        StartCoroutine("rewarmSnow");
    }

    void SpawnOnCliff()
    {
        norm.transform.position = normCliffTransform.position;
        snowHolder.SetActive(false);
        StartCoroutine("rewarmSnow");
    }

    IEnumerator rewarmSnow()
    {
        yield return new WaitForSeconds(0.01f);
        snowHolder.SetActive(true);
    }
}
