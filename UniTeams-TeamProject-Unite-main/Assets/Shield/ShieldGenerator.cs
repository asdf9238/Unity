using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    public GameObject ShieldPrefab;

    public void GenerateShield()
    {
        GameObject Shield = Instantiate(ShieldPrefab);
    }
}
