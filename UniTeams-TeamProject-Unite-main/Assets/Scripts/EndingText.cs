using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingText : MonoBehaviour
{
    public TextMeshProUGUI TextTMP;
    public GameObject UIimage;
    public GameObject endBack;
    public GameObject background1;
    public GameObject background2;
    public GameObject background3;
    public GameObject background4;
    public GameObject endingCredit;
    int index = 0;
    public string[] dialogue = {
        "대사1.",
        "대사2.",
        "대사3.",
        "대사4.",
        "대사5.",
        "끝"
    };

    // Start is called before the first frame update
    void Start()
    {
        TextTMP.text = "10년 전 널 살려둔 실수가 결국 내 발목을 붙잡는구나...";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TextTMP.text = dialogue[index];
            UIimage.SetActive(true);
            switch (index)
            {
                case 1:
                    endBack.SetActive(false);
                    break;
                case 2:
                    background1.SetActive(false);
                    break;
                case 3:
                    background2.SetActive(false);
                    break;
                case 4:
                    background3.SetActive(false);
                    break;
                case 5:
                    background4.SetActive(false);
                    UIimage.SetActive(false);
                    break;

            }

            if (index < 5)
                index++;
        }

    }
}
