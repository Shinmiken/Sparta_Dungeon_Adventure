using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCamera : MonoBehaviour
{
    public Transform Camera;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 pos = Camera.localPosition;
            pos.z = 0;
            Camera.localPosition = pos;

            Vector3 rot = Camera.localEulerAngles;
            rot.x = 0;
            Camera.localEulerAngles = rot;
        }
    }
}
