using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCamera : MonoBehaviour
{
    public Transform Camera;

    private void OnCollisionEnter(Collision collision) // 카메라 1인칭으로 변경
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 pos = Camera.localPosition;
            pos.z = 0;
            Camera.localPosition = pos; // 카메라 위치 0으로 변경

            Vector3 rot = Camera.localEulerAngles;
            rot.x = 0;
            Camera.localEulerAngles = rot; // 카메라 회전 0으로 변경
        }
    }
}
