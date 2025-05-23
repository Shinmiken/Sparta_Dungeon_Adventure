using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveRock : MonoBehaviour
{
    float num = 0;
    float number = 1;
    Vector3 pos;

    private Transform original;

    private void Start()
    {
        pos = transform.position;
    }
    void Update()
    {
        Vector3 movePos = transform.position;
        num = number * Time.deltaTime;
        if (movePos.z >= pos.z + 5)
        {
            ChangeDir(-1);
        }
        else if (movePos.z <= pos.z -5)
        {
            ChangeDir(1);
        }
        transform.position += Vector3.forward * (num * 2);
    }

    void ChangeDir(float value)
    {
        number = value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            original = collision.transform.parent;
            collision.transform.SetParent(transform); // 플레이어를 자식으로
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(original); // 부모에서 분리
        }
    }
}
