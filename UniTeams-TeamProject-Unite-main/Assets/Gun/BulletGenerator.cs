using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGenerator : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject BulletPrefab1;
    public GameObject BulletPrefab2;
    public float timeBetweenShots = 0.5f;
    public float timeBetweenShots1 = 0.25f;
    public float timeBetweenShots2 = 1f;
    public int numberOfShots1 = 3;
    public float fanSpreadAngle = 45f; // ��ä���� ����
    private float shotTime;
    private int act = 0;

    public int GenerateBullet(int BaseAttack, Vector2 direction, Vector3 playerPos)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
        if (BaseAttack == 0)
        {
            act = 0;
            if (Time.time >= shotTime)
            {
                GameObject bullet = Instantiate(BulletPrefab, playerPos, Quaternion.AngleAxis(angle - 90, Vector3.forward));
                shotTime = Time.time + timeBetweenShots;
            }
        }
        if (BaseAttack == 1)
        {
            if (Time.time >= shotTime)
            {
                GameObject bullet = Instantiate(BulletPrefab1, playerPos, Quaternion.AngleAxis(angle - 90, Vector3.forward));
                shotTime = Time.time + timeBetweenShots1;
                act++;
            }
        }
        if (BaseAttack == 2)
        {
            if (Time.time >= shotTime)
            {
                Vector2 gunDirection = transform.up; // ���� �ٶ󺸴� ����
                float startAngle = fanSpreadAngle * (numberOfShots1 - 1) / 2; // ��ä�� ���� ���� ����

                for (int i = 0; i < numberOfShots1; i++)
                {
                    float bulletAngle = startAngle - (i * fanSpreadAngle); // �� �Ѿ��� �߻� ���� ����
                    Vector2 bulletDirection = Quaternion.Euler(0, 0, bulletAngle) * -gunDirection; // �Ѿ��� ���� ���� (���� �ݴ� ����)
                    float bulletRotation = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;
                    Quaternion bulletRotationQuat = Quaternion.AngleAxis(bulletRotation, Vector3.forward);
                    Instantiate(BulletPrefab, playerPos, bulletRotationQuat); // �Ѿ� �߻�
                }
                shotTime = Time.time + timeBetweenShots;
                act++;
            }
        }
        if (BaseAttack == 3)
        {
            if(Time.time >= shotTime)
            {
                GameObject bullet = Instantiate(BulletPrefab2, playerPos, Quaternion.AngleAxis(angle - 90, Vector3.forward));
                shotTime = Time.time + timeBetweenShots2;
                act++;
            }
        }
        return act;
    }
}
