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
        if(player.position.y > 300) // �÷��̾� ��ġ�� ���� skybox material����
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

    public void SetNight() // ��
    {
        RenderSettings.skybox = Night;
        Light.intensity = 0.1f;
        RenderSettings.ambientLight = Color.black;
    }

    public void SetDay() // ��
    {
        RenderSettings.skybox = Day;
        Light.intensity = 1f;
        RenderSettings.ambientLight = Color.white;
    }
}
