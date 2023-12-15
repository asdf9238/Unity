using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    public GameObject Player;
    public GameObject crosshairPrefab, circlePrefab, rifleIconPrefab, shotgunIconPrefab, sniperIconPrefab, missileIconPrefab, lightningIconPrefab, kunaiIconPrefab, dartIconPrefab, shieldIconPrefab, knifeIconPrefab, tornadoIconPrefab, healIconPrefab, targetLightningIconPrefab;   
    public GameObject TornadoPrefab;
    public GameObject CoolTimePrefab;
    private GameObject crosshairInstance, circleInstance, baseAttackIconInstance, iconInstance, mouseCircleInstance, CoolTimeInstance, BaseAttackCoolTimeInstance;

    public float iconHeight = 1.1f;
    private float followRange = 0f;
    Quaternion iconRotation = Quaternion.Euler(0f, 0f, 45f);

    private Vector3 IconPos, BaseAttackIconPos;

    void Update()
    {
        UpdateRangePosition();
        if(Input.GetMouseButtonDown(0))
        {
            HideRangeIndicator();
        }

    }
    public void setRangeType(int skillReady)
    {
        if (iconInstance != null)
        {
            Destroy(iconInstance);
        }
        else if (CoolTimeInstance != null)
        {
            Destroy(CoolTimeInstance);
        }
        switch (skillReady)
        {
            case 1:
                baseAttackIconInstance = Instantiate(rifleIconPrefab, BaseAttackIconPos, iconRotation);
                break;
            case 2:
                baseAttackIconInstance = Instantiate(shotgunIconPrefab, BaseAttackIconPos, iconRotation);
                break;
            case 3:
                baseAttackIconInstance = Instantiate(sniperIconPrefab, BaseAttackIconPos, iconRotation);
                break;
            case 4: //미사일
                ShowMissileRange();
                iconInstance = Instantiate(missileIconPrefab, IconPos, iconRotation);
                break;
            case 5: //번개
                ShowLightningRange();
                iconInstance = Instantiate(lightningIconPrefab, IconPos, Quaternion.identity);
                break;
            case 6:
                iconInstance = Instantiate(shieldIconPrefab, IconPos, Quaternion.identity);
                break;
            case 7:
                iconInstance = Instantiate(kunaiIconPrefab, IconPos, iconRotation);
                break;
            case 8:
                iconInstance = Instantiate(dartIconPrefab, IconPos, Quaternion.identity);
                break;
            case 9:
                iconInstance = Instantiate(knifeIconPrefab, IconPos, Quaternion.identity);
                break;
            case 10:
                ShowTornadoRange();
                iconInstance = Instantiate(tornadoIconPrefab, IconPos, Quaternion.identity);
                break;
            case 11:
                iconInstance = Instantiate(healIconPrefab, IconPos, Quaternion.identity);
                break;
            case 12:
                iconInstance = Instantiate(targetLightningIconPrefab, IconPos, Quaternion.identity);
                break;
        }
    }

    void UpdateRangePosition()
    {
        Vector3 playerPos = Player.transform.position;
        IconPos = new Vector3(playerPos.x, playerPos.y + iconHeight, playerPos.z);
        BaseAttackIconPos = new Vector3(playerPos.x + iconHeight, playerPos.y, playerPos.z);
        if (crosshairInstance != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            crosshairInstance.transform.position = mousePosition;
        }
        if (circleInstance != null)
        {
            circleInstance.transform.position = playerPos;
        }
        if (iconInstance != null)
        {
            iconInstance.transform.position = IconPos;
        }
        if (baseAttackIconInstance != null)
        {
            baseAttackIconInstance.transform.position = BaseAttackIconPos;
        }
        if (mouseCircleInstance != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            mouseCircleInstance.transform.position = mousePosition;
        }
        if(CoolTimeInstance != null)
        {
            CoolTimeInstance.transform.position = IconPos;
        }
        if(BaseAttackCoolTimeInstance != null)
        {
            BaseAttackCoolTimeInstance.transform.position = BaseAttackIconPos;
        }
    }
    public void ShowCoolTime(float CoolTime)
    {
        if (CoolTimeInstance != null)
        {
            Destroy(CoolTimeInstance);
        }
        CoolTimeInstance = Instantiate(CoolTimePrefab, IconPos, Quaternion.identity);
        Destroy(CoolTimeInstance, CoolTime);
    }

    public void BaseAttackShowCoolTime(float CoolTime)
    {
        if (BaseAttackCoolTimeInstance != null)
        {
            Destroy(BaseAttackCoolTimeInstance);
        }
        BaseAttackCoolTimeInstance = Instantiate(CoolTimePrefab, BaseAttackIconPos, Quaternion.identity);
        Destroy(BaseAttackCoolTimeInstance, CoolTime);
    }



    void ShowMissileRange()
    {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        if (crosshairInstance == null)
        {
            crosshairInstance = Instantiate(crosshairPrefab, mousePosition, Quaternion.identity);
        }
    }
    void ShowLightningRange()
    {
        if (circleInstance == null)
        {
            circleInstance = Instantiate(circlePrefab, Player.transform.position, Quaternion.identity);
        }
    }
    void ShowTornadoRange()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        followRange = TornadoPrefab.GetComponent<Tornado>().followRange;
        if (mouseCircleInstance == null)
        {
            mouseCircleInstance = Instantiate(circlePrefab, mousePosition, Quaternion.identity);
            Transform circleTransform = mouseCircleInstance.transform;
            Vector3 baseSize = new Vector3(1.0f, 1.0f, 1.0f);
            Vector3 newSize = baseSize * followRange / 5.0f;
            circleTransform.localScale = newSize;
        }
    }

    void HideRangeIndicator()
    {
        if (crosshairInstance != null)
        {
            Destroy(crosshairInstance);
        }
        if (circleInstance != null)
        {
            Destroy(circleInstance);
        }
        if (mouseCircleInstance != null)
        {
            Destroy(mouseCircleInstance);
        }
        Destroy(iconInstance);

    }
    public void HideBaseAttackIcon()
    {
        Destroy(baseAttackIconInstance);
    }
    public void HideCoolTime()
    {
        if(CoolTimeInstance != null)
        {
            Destroy (CoolTimeInstance);
        }
    }

}