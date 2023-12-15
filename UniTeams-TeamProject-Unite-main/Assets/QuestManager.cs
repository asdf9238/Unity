using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public int FirstMonsterKill = 0;
    public int MiddleBoss1MonsterKill = 0;
    public int MiddleBoss2MonsterKill = 0;
    public int SecondMonsterKill = 0;
    public GameObject Q1txt;
    public GameObject Q2txt;
    public GameObject Q3txt;
    public GameObject Q4txt;
    public GameObject Q5txt;

    public GameObject skillsel1;
    public GameObject skillsel2;
    public GameObject skillsel3;
    public GameObject skillsel4;

    public GameObject wall1;
    public GameObject wall2;
    

    // Update is called once per frame
    void Update()
    {
        if (FirstMonsterKill >= 10)
        {
            FirstMonsterKill = -1;
            Debug.Log("10마리 처치 완료");
            skillsel1.GetComponent<SkillSelect1>().ShowButtons();
            DestroyAllMonsters();
            Q1txt.SetActive(false);
            Q2txt.SetActive(true);
        }
        if (MiddleBoss1MonsterKill == 1)
        {
            MiddleBoss1MonsterKill = -1;
            Debug.Log("중간보스 처치 완료");
            skillsel2.GetComponent<SkillSelect1>().ShowButtons();
            wall1.SetActive(false);
            Q2txt.SetActive(false);
            Q3txt.SetActive(true);
        }
        if (SecondMonsterKill >= 15)
        {
            SecondMonsterKill = -1;
            Debug.Log("15마리 처치 완료");
            DestroyAllMonsters();
            skillsel3.GetComponent<SkillSelect1>().ShowButtons();
            Q3txt.SetActive(false);
            Q4txt.SetActive(true);
        }
        if (MiddleBoss2MonsterKill == 1)
        {
            MiddleBoss2MonsterKill = -1;
            Debug.Log("중간보스 처치 완료");
            skillsel4.GetComponent<SkillSelect1>().ShowButtons();
            wall2.SetActive(false);
            Q4txt.SetActive(false);
            Q5txt.SetActive(true);
        }
    }
    void DestroyAllMonsters()
    {
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monsters)
        {
            Destroy(monster);
        }
    }
}
