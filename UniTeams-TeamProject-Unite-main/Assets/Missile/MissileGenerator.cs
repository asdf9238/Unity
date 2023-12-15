using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileGenerator : MonoBehaviour
{
    public GameObject missilePrefab;

    public void generateMissile(Vector3 targetPos)
    {
        GameObject go = Instantiate(missilePrefab);
        go.transform.position = new Vector3(targetPos.x + 2.8f, targetPos.y + 4, 0);
        go.GetComponent<Missile>().RotateMove(targetPos);
    }

}
