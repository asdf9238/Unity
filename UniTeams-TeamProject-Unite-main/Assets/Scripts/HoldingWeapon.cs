using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingWeapon : MonoBehaviour
{
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0, -0.25f, 0);
    Vector3 rightPosReverse = new Vector3(-0.25f, 0, 0);
    Quaternion leftRot = Quaternion.Euler(0, 0, -1);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -1);

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        bool isReverse = player.flipX;
        transform.localRotation = isReverse ? leftRotReverse : leftRot;
        spriter.flipX = isReverse;
    }
}
