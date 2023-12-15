using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect3 : MonoBehaviour
{
    public Button skillButton7;
    public Button skillButton8;
    public Button skillButton9;
    public float delayToShowButtons = 5.0f;
    private bool buttonsVisible = false;
    private float timer = 0.0f;

    void Start()
    {
        // 게임 시작 시 버튼 숨기기
        HideButtons();
    }

    void Update()
    {
        // 일정 시간이 경과하면 버튼을 보이게 함
        if (!buttonsVisible)
        {
            timer += Time.deltaTime;

            if (timer >= delayToShowButtons)
            {
                ShowButtons();
                buttonsVisible = true;
            }
        }
    }

    // 버튼 숨기기
    void HideButtons()
    {
        skillButton7.gameObject.SetActive(false);
        skillButton8.gameObject.SetActive(false);
        skillButton9.gameObject.SetActive(false);
    }

    // 버튼 보이기
    void ShowButtons()
    {
        skillButton7.gameObject.SetActive(true);
        skillButton8.gameObject.SetActive(true);
        skillButton9.gameObject.SetActive(true);
    }

    // 버튼 클릭 시 호출되는 함수
    public void OnSkillButtonClick(int buttonIndex)
    {
        Debug.Log("Clicked Button Index: " + buttonIndex);
        HideButtons();
    }
}
