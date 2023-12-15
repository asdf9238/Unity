using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemySlow : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Vector2 fastDirection;

    public float slowSpeed = 5f;
    public float fastSpeed = 10f;
    protected float timetoLive = 4f;
    protected float timeToChange = 1f;
    private Transform playerTransform; // 플레이어의 Transform

    private float timeSinceFired = 0f;
    private bool isFastMoving = false;

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
        fastDirection = dir.normalized; // 일직선 이동 방향 저장
        isFastMoving = false; // 초기에는 플레이어를 따라가는 상태
    }

    private void Update()
    {
        if (!isFastMoving)
        {
            // 플레이어를 따라가는 상태
            if (playerTransform != null)
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                rigid.velocity = direction * slowSpeed;
                RotateTowards(direction);
                fastDirection = direction;
            }

            // 일정 시간이 지나면 상태 변경
            if ((timeSinceFired += Time.deltaTime) >= timeToChange)
            {
                isFastMoving = true;
                RotateTowards(fastDirection);
            }
        }
        else
        {
            // 일직선으로 빠르게 이동하는 상태
            rigid.velocity = fastDirection * fastSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<AudioSource>().Play();
            Debug.Log("플레이어와 충돌 처리");

            other.GetComponent<PlayerMoveToClick>().Slow();
            Destroy(gameObject);
        }
    }

    private void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
