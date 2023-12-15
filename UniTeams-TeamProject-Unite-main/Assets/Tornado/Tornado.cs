using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    // ����̵��� ������, ���ӽð�, ���� ����, ���󰡴� �ӵ� ����
    public int damage = 10;
    public float duration = 20f;
    public float attackInterval = 1f;
    public float followSpeed = 5f;
    public float followRange = 10f;

    private float elapsedTime = 0f;
    private Animator animator;
    private Transform targetEnemy;

    private void Start()
    {
        // Animator ������Ʈ ��������
        animator = GetComponent<Animator>();
        // ���� �� TornadoStart �ִϸ��̼� ����
        PlayAnimation("TornadoStart");
        animator.SetBool("Enter", true);
        animator.SetBool("End", false);

        // �ʱ⿡ ���� ����� �� ã��
        FindClosestEnemy();
    }

    private void Update()
    {
        // ��� �ð� ����
        elapsedTime += Time.deltaTime;

        // ����̵� ���� ���� ��
        if (elapsedTime < duration)
        {
            // ���� ����� �� ���󰡱� �� ���� üũ
            FollowClosestEnemy();
        }
        else
        {
            // ���� �ð��� ������ TornadoEnd �ִϸ��̼� ���� �� ������Ʈ �ı�
            animator.SetBool("End", true);
            animator.SetBool("Enter", false);
            Destroy(gameObject, 1f); // "TornadoEnd" �ִϸ��̼� ��� �� 1�� �ڿ� ������Ʈ �ı�
        }
    }

    // ���� ����� �� ���󰡱�
    private void FollowClosestEnemy()
    {
        // ���� Ÿ���� null�̰ų� Ÿ���� �ı��Ǿ��� ��� ���ο� Ÿ�� ã��
        if (targetEnemy == null || !targetEnemy.gameObject.activeSelf)
        {
            FindClosestEnemy();
        }

        // Ÿ���� ���� �� ���󰡱�
        if (targetEnemy != null)
        {
            Vector3 direction = targetEnemy.position - transform.position;
            transform.Translate(direction.normalized * followSpeed * Time.deltaTime);
        }
    }

    private void FindClosestEnemy()
    {
        // Ư�� �ݰ� ������ Enemy �±׸� ���� �� ã��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, followRange);

        if (colliders.Length > 0)
        {
            // ���� ����� �� ã��
            float closestDistance = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy") || collider.CompareTag("Monster"))
                {
                    float distance = Vector3.Distance(
                        transform.position,
                        collider.transform.position
                    );
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        targetEnemy = collider.transform;
                    }
                }
            }
            Debug.Log(targetEnemy);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Monster")
        {
            collision.GetComponent<Monster>().OnHit(damage);
            gameObject.layer = LayerMask.NameToLayer("NoCollision");
            Invoke("ResetLayer", attackInterval);
        }
    }

    void ResetLayer()
    {
        // �浹 ���������� �ٽ� ������ Layer�� ����
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    // �ִϸ��̼� ��� �Լ�
    void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
