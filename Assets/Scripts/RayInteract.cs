using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayInteract : MonoBehaviour
{
    void Start()
    {
        Vector3 basePosition = transform.position;

        Ray ray = new Ray(basePosition + transform.forward, Vector3.up); 
    }
}
