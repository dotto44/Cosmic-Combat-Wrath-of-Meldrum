using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaTransition : MonoBehaviour
{
    [SerializeField] bool exitRight;
    [SerializeField] string sceneName;
    SceneLoader sceneLoader;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sceneLoader == null) sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        if (collision.gameObject.transform.position.x > transform.position.x && exitRight)
        {
            sceneLoader.moveScenes(sceneName);
        }
        else if(collision.gameObject.transform.position.x < transform.position.x && !exitRight)
        {
            sceneLoader.moveScenes(sceneName);
        } 
    }
}
