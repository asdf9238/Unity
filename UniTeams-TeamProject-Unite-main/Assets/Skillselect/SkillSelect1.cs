using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect1 : MonoBehaviour
{
    public Button skillButton1;
    public Button skillButton2;
    public Button skillButton3;
    public GameObject SkillSlot;
    public GameObject FirstBossPrefab;
    public GameObject SecondGenerator;
    public GameObject SecondBossPrefab;
    public GameObject FinalBossPrefab;
    public float delayToShowButtons = 5.0f;
    private bool buttonsVisible = false;
    private float timer = 0.0f;
    private Vector3 BossPos;

    void Start()
    {
        HideButtons();
    }


    // 버튼 숨기기
    void HideButtons()
    {
        skillButton1.gameObject.SetActive(false);
        skillButton2.gameObject.SetActive(false);
        skillButton3.gameObject.SetActive(false);
    }


    // 버튼 보이기
    public void ShowButtons()
    {
        skillButton1.gameObject.SetActive(true);
        skillButton2.gameObject.SetActive(true);
        skillButton3.gameObject.SetActive(true);
    }
    public void DelayedSpawn()
    {
        SecondGenerator.GetComponent<SecondGenerator>().CreatePool();
    }

    // 버튼 클릭 시 호출되는 함수
    public void OnSkillButtonClick(int buttonIndex)
    {
        GameObject Player = GameObject.FindWithTag("Player");
        Debug.Log("Clicked Button Index: " + buttonIndex);
        SkillSlot.GetComponent<SlotManager>().SetSkillIcon(buttonIndex);
        switch (buttonIndex)
        {
            case 1:
            case 2:
            case 3:
                BossPos = new Vector3(0, 7, 0);
                Instantiate(FirstBossPrefab, BossPos, Quaternion.identity);
                Player.GetComponent<PlayerSkill>().QskillReady = buttonIndex;
                break;
            case 4:
            case 5:
            case 6:
                Player.GetComponent<PlayerSkill>().WskillReady = buttonIndex;
                Invoke("DelayedSpawn", 3f);
                break;
            case 7:
            case 8:
            case 9:
                Player.GetComponent<PlayerSkill>().EskillReady = buttonIndex;
                BossPos = new Vector3(0, 44, 0);
                Instantiate(SecondBossPrefab, BossPos, Quaternion.identity);
                break;
            case 10:
            case 11:
            case 12:
                Player.GetComponent<PlayerSkill>().RskillReady = buttonIndex;
                BossPos = new Vector3(1, 72, 0);
                Instantiate(FinalBossPrefab, BossPos, Quaternion.identity);
                break;
        }
        HideButtons();
    }
}
