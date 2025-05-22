using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    public Material Day;
    public Material Night;
    public Light Light;
    public Transform player;

    private void Update()
    {
        if(player.position.y > 300)
        {
            if (RenderSettings.skybox == Night) return;
            Debug.Log("dd");
            SetNight();
        }
        else if(player.position.y < 300)
        {
            if (RenderSettings.skybox == Day) return;
            Debug.Log("ddd");
            SetDay();
        }
    }

    public void SetNight()
    {
        RenderSettings.skybox = Night;
        Light.intensity = 0.1f;
        RenderSettings.ambientLight = Color.black;
    }

    public void SetDay()
    {
        RenderSettings.skybox = Day;
        Light.intensity = 1f;
        RenderSettings.ambientLight = Color.white;
    }
}
