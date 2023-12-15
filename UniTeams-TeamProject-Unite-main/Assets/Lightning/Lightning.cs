using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public int damage;

    void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Monster")
        {
            if (!collision.isActiveAndEnabled)
                return;
            collision.GetComponent<Monster>().OnHit(damage);
        }
    }
}
