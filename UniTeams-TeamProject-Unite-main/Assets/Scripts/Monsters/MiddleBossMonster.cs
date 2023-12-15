using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MiddleBossMonster : MonoBehaviour, Monster
{
    public enum MonsterState
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public int hp = 700;
    public Slider Hp;

    [SerializeField]
    protected float speed = 4.5f;

    [SerializeField]
    protected int attackConstant = 2000;

    protected float attackRange = 9f;
    protected float chaseRange = 13f;
    protected float patrolRange = 5f;
    public MonsterState monsterState;
    public int FirstMonsterKill = 0;
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject questManager;
    public GameObject middleboss;
    protected SpriteRenderer spriteRenderer;

    protected new Rigidbody2D rigidbody2D;

    protected Vector2 targetPosition;
    protected int attackTimer = 0;
    private Animator anim;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        questManager = GameObject.FindWithTag("Quest");
        monsterState = MonsterState.Patrol;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        StartCoroutine(UpdatePatrolTarget());
    }

    void Update()
    {
        switch (monsterState)
        {
            case MonsterState.Patrol:
                Patrol();
                break;
            case MonsterState.Attack:
                Attack();
                if (attackTimer > attackConstant)
                {
                    DelegateAttack();
                    Invoke("DelegateAttack", 0.5f);
                    Invoke("DelegateAttack", 1f);
                    attackTimer = 0;
                }
                else
                {
                    attackTimer++;
                }

                break;
            case MonsterState.Chase:
                Chase();
                break;
            case MonsterState.Dead:
                break;
        }
        Hp.value = this.hp;

    }

    void FixedUpdate()
    {
        spriteRenderer.flipX = targetPosition.x < rigidbody2D.position.x;
    }

    void Patrol()
    {
        Vector2 playerPosition = player.transform.position;
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        if (distanceToPlayer < chaseRange)
        {
            // 추격 사거리에 들어오면 추격으로 전환
            monsterState = MonsterState.Chase;
        }
        // Move towards the target position
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // Check if the monster has reached the target position
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        if (distanceToTarget < 0.1f)
        {
            // If reached the target, get a new random target position
            StartCoroutine(UpdatePatrolTarget());
        }
    }

    void Chase()
    {
        targetPosition = player.transform.position;
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
        // Check the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, targetPosition);

        if (distanceToPlayer < attackRange)
        {
            // If the player is within the attack range, start attacking
            monsterState = MonsterState.Attack;
            anim.SetBool("isAttack", true);
        }
        else if (distanceToPlayer > chaseRange)
        {
            // If the player is outside the chase range, go back to patrolling
            monsterState = MonsterState.Patrol;
            StartCoroutine(UpdatePatrolTarget());
        }
    }

    void Attack()
    {
        Vector2 playerPosition = player.transform.position;
        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        if (distanceToPlayer > attackRange)
        {
            monsterState = MonsterState.Chase;
            anim.SetBool("isAttack", false);
            StartCoroutine(UpdatePatrolTarget());
        }
    }

    // void DelegateAttack()
    // {
    //     Vector3 playerPosition = player.transform.position;
    //     Vector3 dir = playerPosition - this.transform.position;
    //     dir = dir.normalized;

    //     GameObject bullet = Instantiate(bulletPrefab, this.transform);
    //     if (bullet != null)
    //     {
    //         bullet.GetComponent<Transform>().position = transform.position;
    //         bullet.GetComponent<Transform>().rotation = Quaternion.FromToRotation(Vector3.up, dir);
    //         bullet.GetComponent<Transform>().rotation *= Quaternion.Euler(0, 0, 270);
    //         bullet.GetComponent<Bullet>().Fire(dir);
    //     }
    // }

    void DelegateAttack()
    {
        if (monsterState != MonsterState.Attack)
            return;

        Vector3 playerPosition = player.transform.position;
        Vector3 dir = playerPosition - transform.position;
        dir = dir.normalized;
        GameObject bullet;

        // 8개의 탄환을 발사
        for (int i = 0; i < 8; i++)
        {
            // 현재 방향에 대한 회전 각도 계산
            float angle = i * 45f;

            // 회전 적용
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 rotatedDir = rotation * dir;

            // 탄환 생성 및 초기화
            bullet = Instantiate(bulletPrefab);
            if (bullet != null)
            {
                bullet.GetComponent<Transform>().position = transform.position;
                bullet.GetComponent<Transform>().rotation = Quaternion.Euler(rotatedDir);
                bullet.GetComponent<Transform>().rotation *= Quaternion.Euler(0, 0, 135);
                bullet.GetComponent<BulletEnemyAccel>().Fire(rotatedDir);
            }
        }
    }

    IEnumerator UpdatePatrolTarget()
    {
        // Generate a new random target position within the patrol range
        targetPosition = new Vector2(
            transform.position.x + Random.Range(-patrolRange, patrolRange),
            transform.position.y + Random.Range(-patrolRange, patrolRange)
        );

        // Wait for a random amount of time before generating the next target
        float patrolWaitTime = Random.Range(1f, 3f);
        yield return new WaitForSeconds(patrolWaitTime);

        // If the current state is still Patrol, update the target and continue patrolling
        if (monsterState == MonsterState.Patrol)
        {
            StartCoroutine(UpdatePatrolTarget());
        }
    }

    public void OnHit(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            Debug.Log("1번째 보스 사망");
            Die();
        }      
    }

    void Die()
    {
        if (questManager.GetComponent<QuestManager>().MiddleBoss1MonsterKill != -1)
        {
            questManager.GetComponent<QuestManager>().MiddleBoss1MonsterKill++;
        }
        monsterState = MonsterState.Dead;
        Destroy(gameObject);
    }
}
