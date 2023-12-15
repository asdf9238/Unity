using System.Collections;
using UnityEngine;

public class SecondMonster : MonoBehaviour, Monster
{
    public enum MonsterState
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public int hp = 100;

    [SerializeField]
    protected float speed = 4f;

    [SerializeField]
    protected int attckConstant = 250;

    protected float attackRange = 8f;
    protected float chaseRange = 14f;
    protected float patrolRange = 5f;
    public MonsterState monsterState;
    public GameObject player; // 지금은 드래그로 연결되어 있는데 나중에 Find로 수정해야 할 수도 있음
    public GameObject bulletPrefab; // 이건 상위 몬스터를 만들면서 수정
    protected SpriteRenderer spriteRenderer;
    public GameObject questManager;

    protected new Rigidbody2D rigidbody2D;

    protected Vector2 targetPosition;
    protected int attackTimer = 0;
    private Animator anim;

    void Start()
    {
        questManager = GameObject.FindWithTag("Quest");
        player = GameObject.FindWithTag("Player");
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
                if (attackTimer > attckConstant)
                {
                    DelegateAttack();
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
            StartCoroutine(UpdatePatrolTarget());
        }
    }

    void DelegateAttack()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 dir = playerPosition - this.transform.position;
        dir = dir.normalized;

        GameObject bullet = Instantiate(bulletPrefab, this.transform);
        if (bullet != null)
        {
            bullet.GetComponent<Transform>().position = transform.position;
            bullet.GetComponent<Transform>().rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<BulletEnemy>().Fire(dir);
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
            Die();
    }

    void Die()
    {
        if (questManager.GetComponent<QuestManager>().SecondMonsterKill != -1)
        {
            questManager.GetComponent<QuestManager>().SecondMonsterKill++;
        }
        monsterState = MonsterState.Dead;
        gameObject.SetActive(false);
    }
}
