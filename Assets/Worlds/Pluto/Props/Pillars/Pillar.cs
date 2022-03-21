using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    [SerializeField] GameObject[] pillarPieces;
    [SerializeField] GameObject[] explodedPieces;
    [SerializeField] GameObject[] particleEmitters;

    [SerializeField] int pillarNumber;


    private void Start()
    {
        setStartingPillarState();
    }


    void setStartingPillarState()
    {
        if (DataService.Instance.saveData.getRockPillarBroken(pillarNumber)) destroyPillarPieces();
    }

    void setPillarBroken()
    {
        DataService.Instance.saveData.setRockPillarBroken(pillarNumber);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Rake") return;

        explode();
    }

    private void explode()
    {
        destroyPillarPieces();
        setPillarBroken();

        for (int i = 0; i < explodedPieces.Length; i++)
        {

            GameObject norm = GameObject.FindGameObjectWithTag("Player");

            Vector2 force = new Vector2(Random.Range(2000, 4000) * 0.001f, Random.Range(-1000, 2000) * 0.001f);

            if (norm.transform.position.x > transform.position.x) force.x *= -1;

            explodedPieces[i].SetActive(true);
            explodedPieces[i].GetComponent<Rigidbody2D>().gravityScale = 3;
            explodedPieces[i].GetComponent<Rigidbody2D>().AddForce(force * Random.Range(130, 150) * 0.75f);
        }

        for (int i = 0; i < particleEmitters.Length; i++)
        {
            particleEmitters[i].SetActive(true);
        }
    }

    void destroyPillarPieces()
    {
        GetComponent<BoxCollider2D>().enabled = false;

        for (int i = 0; i < pillarPieces.Length; i++)
        {
            Destroy(pillarPieces[i]);
        }
    }

}
