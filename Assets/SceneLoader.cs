using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] CanvasGroup canvasGroup;
    string sceneToLoad;

    public string lastScene { get; set; }

    private void Awake()
    {
        lastScene = "Pluto";
        DontDestroyOnLoad(gameObject);
    }

    public void moveScenes(string sceneToLoad)
    {
        StartCoroutine(StartLoad());
        lastScene = SceneManager.GetActiveScene().name;
        this.sceneToLoad = sceneToLoad;
    }

    public void MoveScenesLong(string sceneToLoad)
    {
        StartCoroutine(StartLongLoad());
        lastScene = SceneManager.GetActiveScene().name;
        this.sceneToLoad = sceneToLoad;
    }

    IEnumerator StartLoad()
    {
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 0.5f));

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!operation.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 0.5f));
        loadingScreen.SetActive(false);
    }

    IEnumerator StartLongLoad()
    {
        loadingScreen.SetActive(true);
        yield return StartCoroutine(FadeLoadingScreen(1, 0.5f));
        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        while (!operation.isDone)
        {
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0, 0.5f));
        loadingScreen.SetActive(false);
    }


    IEnumerator FadeLoadingScreen(float targetValue, float duration)
    {
        float startValue = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetValue;
    }
}

