using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoGenerator : MonoBehaviour
{
    public GameObject TornadoPrefab;

    // ����̵��� �����ϴ� �Լ�
    public void GenerateTornado(Vector3 spawnPosition)
    {
        // ����̵� �������� �����ϰ� �ʱ�ȭ
        GameObject tornadoInstance = Instantiate(TornadoPrefab, spawnPosition, Quaternion.identity);
    }
}
