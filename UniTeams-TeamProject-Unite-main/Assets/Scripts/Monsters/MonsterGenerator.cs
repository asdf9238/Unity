using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject monsterPrefab; // 몬스터 프리팹
    public int poolSize = 10; // 풀 사이즈
    public float spawnRate = 2f; // 스폰 비율 (초)
    public Vector2 spawnArea = new Vector2(0f, 0f); // 스폰 영역

    public List<GameObject> monsterPool;
    private float nextSpawnTime;

    private void Start()
    {
        CreatePool();
    }

    private void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void CreatePool()
    {
        monsterPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Instantiate(monsterPrefab);
            monster.SetActive(false);
            monsterPool.Add(monster);
        }
    }

    void SpawnMonster()
    {
        if (monsterPool != null)
        {
            foreach (GameObject monster in monsterPool)
            {
                if (!monster.activeInHierarchy)
                {
                    monster.transform.position = GetRandomPosition();
                    monster.SetActive(true);
                    monster.GetComponent<FirstMonster>().hp = 80;
                    monster.GetComponent<FirstMonster>().monsterState = FirstMonster.MonsterState.Chase;
                    break;
                }
            }
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(spawnArea.x - 20, spawnArea.x + 20);
        float y = Random.Range(spawnArea.y - 5, spawnArea.y + 5);
        return new Vector3(x, y, 0);
    }
}
