using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public float speed = 5.0f; // �̵� �ӵ� ������ ����
    public float amplitude = 0.1f; // �յ� ���ٴϴ� ���� ������ ����

    private Vector3 startPos; // �ʱ� ��ġ ����� ����

    void Start()
    {
    }

    void Update()
    {
        startPos = transform.position;
        float newY = startPos.y + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
