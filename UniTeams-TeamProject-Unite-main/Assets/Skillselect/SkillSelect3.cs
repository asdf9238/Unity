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
        // ���� ���� �� ��ư �����
        HideButtons();
    }

    void Update()
    {
        // ���� �ð��� ����ϸ� ��ư�� ���̰� ��
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

    // ��ư �����
    void HideButtons()
    {
        skillButton7.gameObject.SetActive(false);
        skillButton8.gameObject.SetActive(false);
        skillButton9.gameObject.SetActive(false);
    }

    // ��ư ���̱�
    void ShowButtons()
    {
        skillButton7.gameObject.SetActive(true);
        skillButton8.gameObject.SetActive(true);
        skillButton9.gameObject.SetActive(true);
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnSkillButtonClick(int buttonIndex)
    {
        Debug.Log("Clicked Button Index: " + buttonIndex);
        HideButtons();
    }
}
