using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMoveToClick : MonoBehaviour
{
    public Slider HealthBar;

    [SerializeField]
    float speed = 5f;
    bool isSlow = false;

    [SerializeField]
    public float playerHP = 300;

    [SerializeField]
    bool playerIsDead = false;

    Vector3 mousePos,
        transPos,
        targetPos,
        dist;
    SpriteRenderer spriter;
    Animator animator;

    void CalTargetPos()
    {
        mousePos = Input.mousePosition;
        transPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetPos = new Vector3(transPos.x, transPos.y, 0);
        dist = targetPos - transform.position;
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            Time.deltaTime * speed
        );
    }

    public bool Run(Vector3 targetPos)
    {
        float distance = Vector3.Distance(transform.position, targetPos);
        if (distance >= 0.00001f)
        {
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (this.playerHP <= 0)
        {
            animator.SetTrigger("dead");
            this.playerIsDead = true;
        }
        if (this.playerIsDead == false)
        {
            if (other.gameObject.tag == "MonsterBullet")
            {
                this.playerHP = this.playerHP - 10;
            }
        }
    }

    public void Slow()
    {
        speed = 2.5f;
        Invoke("SlowRestore", 2f);
    }

    void SlowRestore()
    {
        speed = 5f;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (this.playerHP <= 0)
        {
            animator.SetTrigger("dead");
            this.playerIsDead = true;
            Invoke("GameOver", 3f);
        }
        if (this.playerIsDead == false)
        {
            if (collision.gameObject.tag == "Monster")
            {
                this.playerHP = this.playerHP - Time.deltaTime * 10;
            }
        }
    }
    private void GameOver(){
        SceneManager.LoadScene("GameOver");
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (this.playerHP <= 0)
        {
            animator.SetTrigger("dead");
            this.playerIsDead = true;
        }
        if (this.playerIsDead == false)
        {
            if (collision.gameObject.tag == "Monster")
            {
                this.playerHP = this.playerHP - Time.deltaTime * 30;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsDead == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                CalTargetPos();
                spriter.flipX = dist.x < 0;
            }
            if (Run(targetPos))
            {
                animator.SetBool("iswalk", true);
                Move();
            }
            else
            {
                animator.SetBool("iswalk", false);
            }
        }
        HealthBar.value = this.playerHP;
    }
}
