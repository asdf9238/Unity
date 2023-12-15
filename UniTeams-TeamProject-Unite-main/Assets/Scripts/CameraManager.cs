using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject target; // ī�޶� ���� ���
    public float moveSpeed; // ī�޶� ���� �ӵ�
    private Vector3 targetPosition; // ����� ���� ��ġ

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (target.gameObject != null)
        {
            // this�� ī�޶� �ǹ� (z���� ī�޶��� �״�� ����)
            targetPosition.Set(
                target.transform.position.x,
                target.transform.position.y,
                this.transform.position.z
            );

            // vectorA -> B���� T�� �ӵ��� �̵�
            this.transform.position = Vector3.Lerp(
                this.transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );
        }
    }
}
