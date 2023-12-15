using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Vector3 targetPos;
    float speed = 10.0f;
    public GameObject ExplosionPrefab;

    public void RotateMove(Vector3 targetPos)
    {
        Vector3 dist = targetPos - transform.position;
        float speed = 4f;
        transform.position += dist * speed * Time.deltaTime;
        float angle = Mathf.Atan2(dist.y, dist.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        this.targetPos = targetPos;
        
    }
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
