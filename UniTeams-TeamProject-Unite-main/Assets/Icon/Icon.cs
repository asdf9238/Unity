using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public float speed = 5.0f; // 이동 속도 조절용 변수
    public float amplitude = 0.1f; // 둥둥 떠다니는 강도 조절용 변수

    private Vector3 startPos; // 초기 위치 저장용 변수

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
