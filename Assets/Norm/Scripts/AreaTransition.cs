using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaTransition : MonoBehaviour
{
    enum Directions
    {
        Left,
        Right,
        Up,
        Down
    }
    [SerializeField] Directions exitDirection;
    [SerializeField] string sceneName;
    SceneLoader sceneLoader;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (sceneLoader == null) sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();

        if (collision.gameObject.transform.position.x > transform.position.x && exitDirection == Directions.Right)
        {
            sceneLoader.moveScenes(sceneName);
        }
        else if(collision.gameObject.transform.position.x < transform.position.x && exitDirection == Directions.Left)
        {
            sceneLoader.moveScenes(sceneName);
        }
        else if(collision.gameObject.transform.position.y > transform.position.y && exitDirection == Directions.Up)
        {
            sceneLoader.moveScenes(sceneName);
        }
        else if (collision.gameObject.transform.position.y < transform.position.y && exitDirection == Directions.Down)
        {
            sceneLoader.moveScenes(sceneName);
        }
    }
}
