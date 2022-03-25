using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPieces : MonoBehaviour
{
    [SerializeField] GameObject roofBlocker;
    [SerializeField] GameObject floorPiece2;
    [SerializeField] GameObject floorPiece3;
    [SerializeField] GameObject floorPiece4;
    [SerializeField] GameObject floorPiece5;
    [SerializeField] GameObject floorPiece6;

    [SerializeField] GameObject camera4Transition;
    [SerializeField] GameObject spirox;

    public void staggerFloorPieces()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        floorPiece2.GetComponent<BoxCollider2D>().enabled = true;
        floorPiece3.GetComponent<BoxCollider2D>().enabled = true;
        floorPiece4.GetComponent<BoxCollider2D>().enabled = true;
        floorPiece5.GetComponent<BoxCollider2D>().enabled = true;
        floorPiece6.GetComponent<BoxCollider2D>().enabled = true;
        floorPiece2.transform.position = new Vector3(floorPiece2.transform.position.x, floorPiece2.transform.position.y + 0.5f, floorPiece2.transform.position.z);
        floorPiece3.transform.position = new Vector3(floorPiece3.transform.position.x, floorPiece3.transform.position.y - 0.2f, floorPiece3.transform.position.z);
        floorPiece4.transform.position = new Vector3(floorPiece4.transform.position.x, floorPiece4.transform.position.y - 0.8f, floorPiece4.transform.position.z);
        spirox.transform.position = new Vector3(spirox.transform.position.x, spirox.transform.position.y - 0.8f, spirox.transform.position.z);
        floorPiece5.transform.position = new Vector3(floorPiece5.transform.position.x, floorPiece5.transform.position.y + 0.1f, floorPiece5.transform.position.z);
        floorPiece6.transform.position = new Vector3(floorPiece6.transform.position.x, floorPiece6.transform.position.y - 0.4f, floorPiece6.transform.position.z);
        StartCoroutine("drop");
    }

    public void shakeFloorPiece2()
    {
        floorPiece2.GetComponent<Animator>().CrossFade("Shake", 0);
    }

    public void shakeFloorPiece3()
    {
        floorPiece3.GetComponent<Animator>().CrossFade("Shake", 0);
    }

    public void shakeFloorPiece4()
    {
        floorPiece4.GetComponent<Animator>().CrossFade("Shake", 0);
    }

    public void shakeFloorPiece5()
    {
        floorPiece5.GetComponent<Animator>().CrossFade("Shake", 0);
    }

    public void shakeFloorPiece6()
    {
        floorPiece6.GetComponent<Animator>().CrossFade("Shake", 0);
    }

    public void dropFloorPiece4()
    {
        floorPiece4.GetComponent<Rigidbody2D>().gravityScale = 4;
    }

    public void dropFloorPiece3()
    {
        floorPiece3.GetComponent<Rigidbody2D>().gravityScale = 4;
    }

    public void dropFloorPiece2()
    {
        floorPiece2.GetComponent<Rigidbody2D>().gravityScale = 4;
    }

    public void dropFloorPiece5()
    {
        floorPiece5.GetComponent<Rigidbody2D>().gravityScale = 4;
    }

    public void dropFloorPiece6()
    {
        floorPiece6.GetComponent<Rigidbody2D>().gravityScale = 4;
    }

    public void destroyAll()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        Destroy(floorPiece2);
        Destroy(floorPiece3);
        Destroy(floorPiece4);
        Destroy(floorPiece5);
        Destroy(floorPiece6);
        Destroy(roofBlocker);
        Destroy(camera4Transition);
    }

    IEnumerator drop()
    {
        yield return new WaitForSeconds(0.25f);
        shakeFloorPiece4();
        yield return new WaitForSeconds(1.2f);
        shakeFloorPiece5();
        yield return new WaitForSeconds(0.1f);
        shakeFloorPiece3();
        yield return new WaitForSeconds(1.3f);
        shakeFloorPiece2();
        yield return new WaitForSeconds(0.1f);
        shakeFloorPiece6();

    }

}

