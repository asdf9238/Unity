using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SlotManager : MonoBehaviour
{
    public Image SkillIcon1;
    public Image SkillIcon2;
    public Image SkillIcon3;
    public Image CoolTimeIcon;

    void Start()
    {
        SkillIcon1.gameObject.SetActive(false);
        SkillIcon2.gameObject.SetActive(false);
        SkillIcon3.gameObject.SetActive(false);
        CoolTimeIcon.gameObject.SetActive(false);
    }

    public void CoolTimeOn()
    {
        CoolTimeIcon.gameObject.SetActive(true);
    }
    public void CoolTimeOff()
    {
        CoolTimeIcon.gameObject.SetActive(false);

    }
    public void SetSkillIcon(int num)
    {
        switch (num)
        {
            case 1:
                SkillIcon1.gameObject.SetActive(true);
                break;
            case 2:
                SkillIcon2.gameObject.SetActive(true);
                break;
            case 3:
                SkillIcon3.gameObject.SetActive(true);
                break;
            case 4:
                SkillIcon1.gameObject.SetActive(true);
                break;
            case 5:
                SkillIcon2.gameObject.SetActive(true);
                break;
            case 6:
                SkillIcon3.gameObject.SetActive(true);
                break;
            case 7:
                SkillIcon1.gameObject.SetActive(true);
                break;
            case 8:
                SkillIcon2.gameObject.SetActive(true);
                break;
            case 9:
                SkillIcon3.gameObject.SetActive(true);
                break;
            case 10:
                SkillIcon1.gameObject.SetActive(true);
                break;
            case 11:
                SkillIcon2.gameObject.SetActive(true);
                break;
            case 12:
                SkillIcon3.gameObject.SetActive(true);
                break;
        }
    }
    
}
