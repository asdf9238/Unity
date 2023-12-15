using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerSkill : MonoBehaviour
{
    public GameObject RangeIndicator;
    public GameObject BulletGenerator, MissileGenerator, KnifeGenerator, TornadoGenerator;
    public LightningGenerator lightningGenerator;
    public GameObject ShieldPrefab;
    public GameObject HealPrefab;
    public GameObject TornadoPrefab;
    public GameObject TargetLightningPrefab;
    public GameObject SkillSlot1, SkillSlot2, SkillSlot3, SkillSlot4;
    Vector3 mousePos, transPos, targetPos;
    Vector2 direction;
    public int attackRange = 10;
    public float ShieldSpan = 5f;
    public float HealAmount = 100f;
    public float PlayerMaxHP = 300f;
    public int QskillReady = 0;
    public int WskillReady = 0;
    public int EskillReady = 0;
    public int RskillReady = 0;
    public float QCoolTime, WCoolTime, ECoolTime, RCoolTime;
    int skillReady = 0;
    int BaseAttack = 0;
    int ShotStack = 0;
    float followRange = 0f;
    float CoolTime = 0f;
    float QReadyTime, WReadyTime, EReadyTime, RReadyTime = 0f;

    private Transform targetEnemy;


    void Start()
    {
    }

    void CalTargetPos()
    {
        mousePos = Input.mousePosition;
        transPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetPos = new Vector3(transPos.x, transPos.y, 0);
    }
    void CalDirection()
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

    }
    void CalEnemyDirection()
    {
        if(targetEnemy != null) 
            direction = targetEnemy.position - transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time < QReadyTime)
            SkillSlot1.GetComponent<SlotManager>().CoolTimeOn();
        else
            SkillSlot1.GetComponent<SlotManager>().CoolTimeOff();


        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(QskillReady == 0)
            {

            }
            else
            {
                if (Time.time > QReadyTime)
                {
                    BaseAttack = QskillReady;
                    RangeIndicator.GetComponent<RangeIndicator>().setRangeType(BaseAttack);
                    QReadyTime = Time.time + QCoolTime;
                }
                else
                {
                    if (BaseAttack == 0) //평타강화지속시간과 쿨타임 사이 시간
                    {
                        CoolTime = QReadyTime - Time.time;
                        RangeIndicator.GetComponent<RangeIndicator>().BaseAttackShowCoolTime(CoolTime);
                    }
                }
            }

        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (WskillReady == 0)
            {

            }
            else
            {
                if (Time.time > WReadyTime)
                {
                    skillReady = WskillReady;
                    RangeIndicator.GetComponent<RangeIndicator>().setRangeType(skillReady);

                }
                else
                {
                    CoolTime = WReadyTime - Time.time;
                    RangeIndicator.GetComponent<RangeIndicator>().ShowCoolTime(CoolTime);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (EskillReady == 0)
            {

            }
            else
            {
                if (Time.time > EReadyTime)
                {
                    skillReady = EskillReady;
                    RangeIndicator.GetComponent<RangeIndicator>().setRangeType(skillReady);

                }
                else
                {
                    CoolTime = EReadyTime - Time.time;
                    RangeIndicator.GetComponent<RangeIndicator>().ShowCoolTime(CoolTime);
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            if (RskillReady == 0)
            {

            }
            else
            {
                if (Time.time > RReadyTime)
                {
                    skillReady = RskillReady;
                    RangeIndicator.GetComponent<RangeIndicator>().setRangeType(skillReady);
                }
                else
                {
                    CoolTime = RReadyTime - Time.time;
                    RangeIndicator.GetComponent<RangeIndicator>().ShowCoolTime(CoolTime);
                }
            }
        }
        if (Input.GetMouseButtonDown(0) && skillReady != 0)
        {
            switch (skillReady)
            {
                case 4:
                    CalTargetPos();
                    MissileGenerator.GetComponent<MissileGenerator>().generateMissile(targetPos);
                    WReadyTime = Time.time + WCoolTime;
                    break;

                case 5:
                    ActivateLightningSkill();
                    WReadyTime = Time.time + WCoolTime;
                    break;

                case 6:
                    GameObject Shield = Instantiate(ShieldPrefab);
                    gameObject.layer = LayerMask.NameToLayer("NoCollision");
                    Invoke("ResetLayer", ShieldSpan);
                    Destroy(Shield, ShieldSpan);
                    WReadyTime = Time.time + WCoolTime;
                    break;

                case 7:
                case 8:
                case 9:
                    CalDirection();
                    KnifeGenerator.GetComponent<KnifeGenerator>().GenerateKnife(skillReady, direction);
                    EReadyTime = Time.time + ECoolTime;
                    break;
                case 10:
                    CalTargetPos();
                    if (IsEnemyInRange())
                    {
                        TornadoGenerator.GetComponent<TornadoGenerator>().GenerateTornado(targetPos);
                        RReadyTime = Time.time + RCoolTime;
                    }
                    else
                    {
                        Debug.Log("스킬 범위 안에 적이 존재하지 않습니다.");
                    }
                    break;
                case 11:
                    PlayerMoveToClick player = GetComponent<PlayerMoveToClick>();
                    if (player.playerHP > PlayerMaxHP - HealAmount) //풀피까지만 체력 채워줌
                        player.playerHP = PlayerMaxHP;
                    else if (player.playerHP == PlayerMaxHP) { // 풀피면 발동 안됨
                        Debug.Log("플레이어의 체력이 이미 가득찼습니다.");
                        break;
                    }   
                    else
                        player.playerHP += HealAmount;
                    GameObject Heal = Instantiate(HealPrefab);
                    Destroy(Heal,1.1f);                   
                    Heal.transform.position = transform.position;
                    RReadyTime = Time.time + RCoolTime;
                    break;
                case 12:
                    Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
                    if (hit.collider != null)
                    {
                        if(hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Monster"))
                        {
                            targetPos = hit.transform.position;
                            Instantiate(TargetLightningPrefab, targetPos, Quaternion.identity);
                            RReadyTime = Time.time + RCoolTime;
                        }
                        else
                        {
                            Debug.Log("적을 선택해주세요.");
                        }
                    }
                    else
                    {
                        Debug.Log("아무것도 선택되지 않았습니다.");
                    }
                    break;
                default:
                    break;            
            }
            skillReady = 0;

        }
        FindClosestEnemy();
        CalEnemyDirection();
        if (Input.GetKey(KeyCode.Space)&& BaseAttack == 1)
        {
            ShotStack = BulletGenerator.GetComponent<BulletGenerator>().GenerateBullet(BaseAttack, direction, transform.position);

            if ( ShotStack > 20)
            {
                BaseAttack = 0;
                RangeIndicator.GetComponent<RangeIndicator>().HideBaseAttackIcon();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShotStack = BulletGenerator.GetComponent<BulletGenerator>().GenerateBullet(BaseAttack, direction, transform.position);
            if (BaseAttack == 2 && ShotStack > 10)
            {
                BaseAttack = 0;
                RangeIndicator.GetComponent<RangeIndicator>().HideBaseAttackIcon();

            }
            if (BaseAttack == 3 && ShotStack > 5)
            {
                BaseAttack = 0;
                RangeIndicator.GetComponent<RangeIndicator>().HideBaseAttackIcon();

            }
        }
    }
    void ActivateLightningSkill()
    {
        if (lightningGenerator == null)
        {
            Debug.LogError("lightningGenerator가 null입니다. ActivateLightningSkill을 호출하기 전에 할당되었는지 확인하십시오.");
        }
        // LightningGenerator 스크립트의 GenerateLightning 메서드를 호출하여 번개를 생성
        StartCoroutine(lightningGenerator.GenerateLightning());
    }
    bool IsEnemyInRange()
    {
        CalTargetPos();
        followRange = TornadoPrefab.GetComponent<Tornado>().followRange;
        // targetPos 주변에 반경 followRange 안에 있는 모든 콜라이더들을 가져옴
        Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPos, followRange);

        // 가져온 콜라이더들을 순회하면서 Enemy 태그를 가진 물체가 있는지 확인
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") || collider.CompareTag("Monster"))
            {
                // Enemy 태그를 가진 물체가 있으면 true 반환
                return true;
            }
        }

        // Enemy 태그를 가진 물체가 없으면 false 반환
        return false;
    }

    private void FindClosestEnemy()
    {
        // 특정 반경 내에서 Enemy 태그를 가진 적 찾기
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);

        if (colliders.Length > 0)
        {
            // 가장 가까운 적 찾기
            float closestDistance = float.MaxValue;

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy") || collider.CompareTag("Monster"))
                {
                    float distance = Vector3.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        targetEnemy = collider.transform;
                    }
                }
            }
        }
    }
    void ResetLayer()
    {
        // 충돌 가능해지면 다시 원래의 Layer로 변경
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
