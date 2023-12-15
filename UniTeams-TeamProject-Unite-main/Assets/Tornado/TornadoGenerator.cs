using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoGenerator : MonoBehaviour
{
    public GameObject TornadoPrefab;

    // 토네이도를 생성하는 함수
    public void GenerateTornado(Vector3 spawnPosition)
    {
        // 토네이도 프리팹을 생성하고 초기화
        GameObject tornadoInstance = Instantiate(TornadoPrefab, spawnPosition, Quaternion.identity);
    }
}
