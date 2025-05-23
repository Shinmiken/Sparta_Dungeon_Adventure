using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCamera : MonoBehaviour
{
    public Transform Camera;

    private void OnCollisionEnter(Collision collision) // ī�޶� 1��Ī���� ����
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 pos = Camera.localPosition;
            pos.z = 0;
            Camera.localPosition = pos; // ī�޶� ��ġ 0���� ����

            Vector3 rot = Camera.localEulerAngles;
            rot.x = 0;
            Camera.localEulerAngles = rot; // ī�޶� ȸ�� 0���� ����
        }
    }
}
