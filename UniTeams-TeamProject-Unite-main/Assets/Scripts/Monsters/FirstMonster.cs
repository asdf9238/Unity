using System.Collections;
using UnityEngine;

public class FirstMonster : MonoBehaviour, Monster
{
    public enum MonsterState
    {
        Patrol,
        Chase,
        Dead
    }

    public int hp = 80;

    [SerializeField]
    protected float speed = 4f;

    [SerializeField]
    protected int attckConstant = 250;

    protected float chaseRange = 14f;
    protected float patrolRange = 4f;
    public MonsterState monsterState;
    public GameObject player;
    public GameObject questManager;
    protected SpriteRenderer spriteRenderer;

    protected new Rigidbody2D rigidbody2D;

    protected Vector2 targetPosition;
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

        if (distanceToPlayer > chaseRange)
        {
            // If the player is outside the chase range, go back to patrolling
            monsterState = MonsterState.Patrol;
            StartCoroutine(UpdatePatrolTarget());
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
        if (questManager.GetComponent<QuestManager>().FirstMonsterKill != -1)
        {
            questManager.GetComponent<QuestManager>().FirstMonsterKill++;
        }

        monsterState = MonsterState.Dead;
        gameObject.SetActive(false);
    }
}
