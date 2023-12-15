using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelect2 : MonoBehaviour
{
    public Button skillButton4;
    public Button skillButton5;
    public Button skillButton6;
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
        skillButton4.gameObject.SetActive(false);
        skillButton5.gameObject.SetActive(false);
        skillButton6.gameObject.SetActive(false);
    }

    // ��ư ���̱�
    void ShowButtons()
    {
        skillButton4.gameObject.SetActive(true);
        skillButton5.gameObject.SetActive(true);
        skillButton6.gameObject.SetActive(true);
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnSkillButtonClick(int buttonIndex)
    {
        Debug.Log("Clicked Button Index: " + buttonIndex);
        HideButtons();
    }
}
