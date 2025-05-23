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
        if(player.position.y > 300) // 플레이어 위치에 따른 skybox material변경
        {
            if (RenderSettings.skybox == Night) return;
            SetNight();
        }
        else if(player.position.y < 300)
        {
            if (RenderSettings.skybox == Day) return;
            SetDay();
        }
    }

    public void SetNight() // 밤
    {
        RenderSettings.skybox = Night;
        Light.intensity = 0.1f;
        RenderSettings.ambientLight = Color.black;
    }

    public void SetDay() // 낮
    {
        RenderSettings.skybox = Day;
        Light.intensity = 1f;
        RenderSettings.ambientLight = Color.white;
    }
}
