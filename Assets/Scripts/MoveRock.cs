using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveRock : MonoBehaviour
{
    float num = 0;
    float number = 1;
    void Update()
    {
        Vector3 pos = transform.position;
        num = number * Time.deltaTime;
        if (pos.z >= 10)
        {
            ChangeDir(-1);
        }
        else if (pos.z <= -10)
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
            collision.transform.SetParent(transform); // �÷��̾ �ڽ�����
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // �θ𿡼� �и�
        }
    }
}
