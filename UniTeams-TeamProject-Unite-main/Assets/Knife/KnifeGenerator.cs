using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeGenerator : MonoBehaviour
{
    public GameObject KnifePrefab;
    public GameObject KnifePrefab1;
    public GameObject KnifePrefab2;
    public float timeBetweenShots = 0.1f; //��ų 1�� ��߻� �ӵ�
    public int numberOfShots = 7; //��ų 1�� �ܰ� ���󰡴� ����
    public int numberOfShots1 = 10; // ��ų 2�� ��ä�� ������� ������ �Ѿ� ����
    public float fanSpreadAngle = 15f; // ��ä���� ����
    public float timeBetweenShots3 = 0.25f; //��ų 3�� ��߻� �ӵ�
    public float skillDuration = 5f;
    public float detectionRadius = 7f; // �ֺ� ���� Ž���� �ݰ�
    private Transform playerTransform;
    private Transform targetEnemy;

    void Start()
    {
        // �÷��̾��� Transform ������Ʈ ��������
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void GenerateKnife(int skillReady, Vector2 direction)
    {
        Vector3 playerPos = playerTransform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        if(skillReady == 7)
        {
            StartCoroutine(ShootKnife(angle,playerPos, rotation));
        }
        if(skillReady == 8)
        {
            Vector2 gunDirection = transform.up;
            float startAngle = fanSpreadAngle * (numberOfShots1 - 1) / 2;

            for (int i = 0; i < numberOfShots1; i++)
            {
                float bulletAngle = startAngle - (i * fanSpreadAngle); // �� �Ѿ��� �߻� ���� ����
                Vector2 bulletDirection = Quaternion.Euler(0, 0, bulletAngle) * -gunDirection; // �Ѿ��� ���� ���� (���� �ݴ� ����)
                float bulletRotation = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
                Quaternion bulletRotationQuat = Quaternion.AngleAxis(bulletRotation, Vector3.forward);
                Instantiate(KnifePrefab1, playerPos, bulletRotationQuat); // �Ѿ� �߻�
            }
        }
        if (skillReady == 9)
        {
            StartCoroutine(AutoShootKnife(playerPos));
        }
    }

    void Update()
    {
        
    }

    IEnumerator ShootKnife(float angle, Vector3 playerPos, Quaternion rotation) //��ų 1�� �����ؼ� �ܰ� 7�� ����
    {
        Quaternion knifeRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        for (int i = 0; i < numberOfShots; i++) { 
                
                Instantiate(KnifePrefab, playerPos, knifeRotation);
                yield return new WaitForSeconds(timeBetweenShots);
            }
    }

    IEnumerator AutoShootKnife(Vector3 playerPos)
    {
        float elapsedTime = 0f;

        while (elapsedTime < skillDuration)
        {
            playerPos = playerTransform.position;

            FindClosestEnemy(playerPos);
            if (targetEnemy != null)
            {
                Vector2 enemyDirection = (targetEnemy.position - playerPos).normalized;
                Quaternion enemyRotation = Quaternion.LookRotation(Vector3.forward, enemyDirection);
                Instantiate(KnifePrefab2, playerPos, enemyRotation);
            }

            yield return new WaitForSeconds(timeBetweenShots3);
            elapsedTime += timeBetweenShots3;
        }        
    }
    private void FindClosestEnemy(Vector3 playerPos)
    {
        // Ư�� �ݰ� ������ Enemy �±׸� ���� �� ã��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerPos, detectionRadius);

        if (colliders.Length > 0)
        {
            // ���� ����� �� ã��
            float closestDistance = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy") || collider.CompareTag("Monster"))
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        targetEnemy = collider.transform;
                    }
                }
            }
        }
    }

}
