using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormShooting : MonoBehaviour
{
    [SerializeField] GameObject[] bullets;
    [SerializeField] Transform firePos;
    [SerializeField] SpriteRenderer armRenderer;
    [SerializeField] LayerMask bulletMask;
    Animator animator;

    public int ammo = 10;

    Vector2[] directions =
    {
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(1, 0),
        new Vector2(1, -1),
        new Vector2(0, -1),
        new Vector2(-1, -1),
        new Vector2(-1, 0),
        new Vector2(-1, 1),
    };

    Vector2[] positions =
    {
        new Vector2(0, 0),
        new Vector2(0.37f, 1.65f),
        new Vector2(1, 1),
        new Vector2(1.67f, 0.06f),
        new Vector2(1, -1),
        new Vector2(0.6f, -1.65f),
        new Vector2(-1, -1),
        new Vector2(-1.67f, 0.06f),
        new Vector2(-1, 1),
    };

    Quaternion[] rotations =
    {
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 0, 90),
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 0, 90),
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 0, 0),
    };
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void shoot()
    {
        int direction = animator.GetInteger("direction");

        Vector2 position = positions[direction];
        if ((direction == 1 || direction == 5) && armRenderer.flipX)
        {
            position.x *= -1;
        }

        ammo--;

        if(Physics2D.OverlapPoint((Vector2)transform.position + position, bulletMask, -100) != null) return;
        spawnBullet(bullets[0], (Vector2)transform.position + position, rotations[direction], directions[direction]);

    }
    public bool isShootInWall()
    {
        int direction = animator.GetInteger("direction");

        Vector2 position = positions[direction];
        if ((direction == 1 || direction == 5) && armRenderer.flipX)
        {
            position.x *= -1;
        }
        if (direction == 3)
        {
            if(Physics2D.OverlapPoint((Vector2)transform.position + position + new Vector2(0.75f, 0), Physics2D.DefaultRaycastLayers, -100) != null)
            {
                return true;
            }
        }
        if (direction == 7)
        {
            if (Physics2D.OverlapPoint((Vector2)transform.position + position + new Vector2(-0.75f, 0), Physics2D.DefaultRaycastLayers, -100) != null)
            {
                return true;
            }
        }
        if (Physics2D.OverlapPoint((Vector2)transform.position + position, Physics2D.DefaultRaycastLayers, -100) != null)
        {
            return true;
        }
        
        return false;
    }
    public void spawnBullet(GameObject bulletType, Vector2 position, Quaternion rotation, Vector2 direction)
    {
        var bullet = Instantiate(
          bulletType,
          position,
          rotation);

         bullet.GetComponent<BulletBase>().shoot(direction);
    }
}
