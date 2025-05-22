using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CharactoManager : MonoBehaviour
{
    public Transform player;
    public Transform camera;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.position;
        pos.y += 3;
        camera.position = pos;
    }
}
