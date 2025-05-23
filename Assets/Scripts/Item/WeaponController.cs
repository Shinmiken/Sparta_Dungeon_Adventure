using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject[] Weapon;
    void Start()
    {
        PlayerManager.Instance.Player.addWeapon += EquipWeapon; // 이벤트 구독
    }

    void EquipWeapon(WeaponType type)
    {
        int index = (int)type;
        if(index == 0) // 검
        {
            PlayerManager.Instance.Player.jumpPos *= 1.5f; // 점프 증가
        }
        else if(index == 1) // 도끼
        {
            PlayerManager.Instance.Player.moveSpeed *= 1.5f; // 이속 증가
        }
        else if(index == 2) // 모자
        {
            ConditionManager.instance.condition.maxHp = 10; // 스테미나 증가
            ConditionManager.instance.condition.curHp = 10;
        }
        Weapon[index].SetActive(true); // 장비 표시
    }
}
