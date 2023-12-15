using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    GameObject Player;
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindWithTag("Player");
        transform.position = Player.transform.position;
    }
}
