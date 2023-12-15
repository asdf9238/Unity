using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGenerator : MonoBehaviour
{
    public GameObject lightningPrefab; // Lightning �������� �Ҵ��ϼ���.
    public float skillRange = 5f; // ��ų�� ������ �����ϼ���.
    public int numberOfTargets = 5; // ������ Lightning�� ������ �����ϼ���.
    public float delayBetweenStrikes = 0.3f; // �� Lightning ���� ���� �����̸� �����ϼ���.

    void Start() { }

    public IEnumerator GenerateLightning()
    {
        // �ֺ� ���͸� ã�� ����Ʈ�� ����
        List<Transform> nearbyMonsters = new List<Transform>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, skillRange);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Monster"))
            {
                nearbyMonsters.Add(collider.transform);
            }
        }
        nearbyMonsters.Sort(
            (a, b) =>
                Vector2
                    .Distance(transform.position, a.position)
                    .CompareTo(Vector2.Distance(transform.position, b.position))
        );

        // �ִ� 5���� ���Ϳ��� Lightning�� ����
        for (int i = 0; i < Mathf.Min(numberOfTargets, nearbyMonsters.Count); i++)
        {
            Transform targetMonster = nearbyMonsters[i];
            if (targetMonster != null)
            {
                // Lightning�� ������ ��ġ�� ����
                Vector3 spawnPosition = new Vector3(
                    targetMonster.position.x,
                    targetMonster.position.y + 0.3f,
                    targetMonster.position.z
                );
                GameObject lightning = Instantiate(
                    lightningPrefab,
                    spawnPosition,
                    Quaternion.identity
                );

                // ���� ��ġ�� ������ Lightning�� �̵����� ����
                yield return new WaitForSeconds(delayBetweenStrikes);
            }
        }
    }
}
