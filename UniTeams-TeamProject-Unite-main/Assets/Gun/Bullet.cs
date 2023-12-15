using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float lifetime = 5f;
    public float BulletSpeed = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * BulletSpeed * Time.deltaTime);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Monster")
        {

            if (!collision.isActiveAndEnabled)
                return;

            Monster monsterComponent = collision.GetComponent<Monster>();
            if (monsterComponent != null)
            {
                monsterComponent.OnHit(damage);
            }
            DestroyBullet();
        }
    }
}
