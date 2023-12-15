using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SecondBossMonster : MonoBehaviour , Monster
{
    public enum MonsterState
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public int hp = 1000;
    public Slider Hp;

    [SerializeField]
    protected float speed = 4.5f;

    [SerializeField]
    protected int attackConstant = 2000;
    protected bool isAttacking = false;

    protected float attackRange = 9f;
    protected float chaseRange = 13f;
    protected float patrolRange = 5f;
    public MonsterState monsterState;
    public GameObject player;
    public GameObject bulletPrefab;
    public GameObject questManager;
    protected SpriteRenderer spriteRenderer;

    protected new Rigidbody2D rigidbody2D;

    protected Vector2 targetPosition;
    protected int attackTimer = 0;
    private Animator anim;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        monsterState = MonsterState.Patrol;
        questManager = GameObject.FindWithTag("Quest");

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
            anim.SetBool("isIdle", false);
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
        // Debug.Log("Attack 실행");
        if (!isAttacking)
        {
            anim.Play("Attack");
            isAttacking = true;
        }
    }

    void DelegateAttack()
    {
        anim.SetBool("isIdle", true);
        monsterState = MonsterState.Patrol;

        Vector3 playerPosition = player.transform.position;
        Vector3 dir = playerPosition - this.transform.position;
        dir = dir.normalized;

        float distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        GameObject bullet = Instantiate(bulletPrefab, this.transform);
        if (bullet != null)
        {
            Vector3 shootPosition = transform.position;
            if (spriteRenderer.flipX)
            {
                shootPosition.x -= 2;
            }
            else
            {
                shootPosition.x += 2;
            }

            bullet.GetComponent<Transform>().position = shootPosition;
            bullet.GetComponent<Transform>().rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<BulletEnemySlow>().Fire(dir);
        }

        Invoke("returnAttack", 2.5f);
    }

    void returnAttack()
    {
        Debug.Log("return attack");
        isAttacking = false;
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
        Debug.Log("1");
        hp -= damage;

        if (hp <= 0)
        {
            Debug.Log("2번째 보스 사망");
            Die();
        }
    }

    void Die()
    {
        if (questManager.GetComponent<QuestManager>().MiddleBoss2MonsterKill != -1)
        {
            questManager.GetComponent<QuestManager>().MiddleBoss2MonsterKill++;
        }
        monsterState = MonsterState.Dead;
        Destroy(gameObject);
    }
}
