using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    private Rigidbody2D rigid;

    public float bulletSpeed = 4f;
    protected float timetoLive = 2f;
    private Transform playerTransform; // 플레이어의 Transform

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player").transform; // 플레이어 찾기
    }

    private void Start()
    {
        Invoke("DestroyBullet", timetoLive);
    }

    public void Fire(Vector3 dir)
    {
        rigid.velocity = dir * bulletSpeed;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized; // 플레이어 방향으로의 벡터
            rigid.velocity = direction * bulletSpeed; // 탄환의 속도와 방향 갱신

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // 회전 각도 계산
            transform.rotation = Quaternion.Euler(0, 0, angle); // 탄환 회전
        }

        if (rigid.velocity.magnitude < 0.5)
        {
            DestroyBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<AudioSource>().Play();
            Debug.Log("플레이어와 충돌 처리");
            Destroy(gameObject);
        }
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
