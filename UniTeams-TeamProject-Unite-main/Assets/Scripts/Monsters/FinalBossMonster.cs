using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class FinalBossMonster : MonoBehaviour, Monster
{
    public enum MonsterState
    {
        Patrol,
        Chase,
        Attack,
        Dead
    }

    public int hp = 1500;
    public Slider Hp;

    [SerializeField]
    protected float speed = 5.5f;

    [SerializeField]
    protected bool isAttacking = false;
    public GameObject questManager;

    public CapsuleCollider2D colliderRight;
    public CapsuleCollider2D colliderLeft;
    protected float chaseRange = 13f;
    protected float patrolRange = 5f;
    public MonsterState monsterState;
    public GameObject player;
    public GameObject bulletPrefab;
    protected SpriteRenderer spriteRenderer;

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
                break;
            case MonsterState.Chase:
                Chase();
                break;
            case MonsterState.Dead:
                break;
        }

        if (speed > 5)
        {
            speed *= 0.97f;
        }
        else
        {
            speed *= 1.03f;
        }
        Hp.value = this.hp;
    }

    void FixedUpdate()
    {
        spriteRenderer.flipX = targetPosition.x < rigidbody2D.position.x;
        if (!spriteRenderer.flipX)
        {
            colliderRight.enabled = true;
            colliderLeft.enabled = false;
        }
        else
        {
            colliderRight.enabled = false;
            colliderLeft.enabled = true;
        }
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

    void Attack()
    {
        if (!isAttacking)
        {
            anim.Play("Melee");
            isAttacking = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            monsterState = MonsterState.Attack;
            anim.SetBool("isAttacking", true);
        }
    }

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {

    //     }
    // }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            anim.SetBool("isAttacking", false);
            monsterState = MonsterState.Patrol;
        }
    }

    void Summon() { }

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
        hp -= damage;

        if (hp <= 0)
            Die();

        return;
    }

    void Die()
    {
        // 죽을 때 애니메이션 처리도 나중에 추가

        monsterState = MonsterState.Dead;
        Destroy(gameObject);
        SceneManager.LoadScene("ending");

    }
}
