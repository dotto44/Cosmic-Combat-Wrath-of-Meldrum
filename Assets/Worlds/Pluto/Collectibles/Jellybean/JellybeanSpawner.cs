using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellybeanSpawner : MonoBehaviour
{
    [SerializeField] GameObject jellyBean;
    
    
    public void spawnBeans(Vector3 spawnPos, Bounds bounds, int count)
    {
        for(int i = 0; i < count; i++)
        {
            Vector2 force = new Vector2(Random.Range(-2000, 2000) * 0.001f, Random.Range(1500, 3500) * 0.001f);

            float randomX = Random.Range(0, bounds.size.x / 2);
            if (force.x < 0) randomX *= -1;

            float randomY = Random.Range(-bounds.size.y / 2, bounds.size.y / 2);

            GameObject bean = Instantiate(jellyBean, new Vector2(spawnPos.x + randomX, spawnPos.y + randomY), jellyBean.transform.rotation);

            bean.GetComponent<Rigidbody2D>().AddForce(force * Random.Range(150, 180));
        }
    }

    public void spawnBeansFromMound(Vector3 spawnPos, Bounds bounds, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 force = new Vector2(Random.Range(-1000, 1000) * 0.001f, Random.Range(5000, 7000) * 0.001f);

            float randomX = Random.Range(0, bounds.size.x / 2);
            if (force.x < 0) randomX *= -1;

            float randomY = Random.Range(-bounds.size.y / 2, bounds.size.y / 2);

            GameObject bean = Instantiate(jellyBean, new Vector2(spawnPos.x + randomX, spawnPos.y + randomY), jellyBean.transform.rotation);

            bean.GetComponent<Rigidbody2D>().AddForce(force * Random.Range(100, 140));
        }
    }
}
