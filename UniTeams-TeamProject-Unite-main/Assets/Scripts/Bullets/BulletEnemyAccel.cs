using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemyAccel : MonoBehaviour
{
    private Rigidbody2D rigid;

    public float slowSpeed = 2f; // 가속 전 느린 속도
    public float fastSpeed = 10f; // 가속 후 빠른 속도
    protected float timetoLive = 5f;
    private float accelerationTime = 1f; // 가속 시점 (발사 후 3초)
    private float timeSinceFired;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Invoke("DestroyBullet", timetoLive);
        timeSinceFired = 0f; // 시간 초기화
    }

    public void Fire(Vector3 dir)
    {
        rigid.velocity = dir * slowSpeed; // 처음에는 느린 속도로 발사
    }

    private void Update()
    {
        timeSinceFired += Time.deltaTime;

        if (timeSinceFired >= accelerationTime)
        {
            rigid.velocity = rigid.velocity.normalized * fastSpeed; // 가속 시점에 도달하면 속도 증가
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
