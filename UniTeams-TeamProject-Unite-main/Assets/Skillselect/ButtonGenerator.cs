using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGenerator : MonoBehaviour
{
    public GameObject SkillSelect1;
    public GameObject SkillSelect2;
    public GameObject SkillSelect3;
    public GameObject SkillSelect4;
    GameObject nowbutton;
    // Start is called before the first frame update
    void Start()
    {
        GenerateButton(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateButton(int num)
    {
        switch (num)
        {
            case 1:
                nowbutton =Instantiate(SkillSelect1);
                break;
            case 2: nowbutton =Instantiate(SkillSelect2); break;
            case 3: nowbutton = Instantiate(SkillSelect3); break;
            case 4: nowbutton = Instantiate(SkillSelect4); break;
        }
    }




}
