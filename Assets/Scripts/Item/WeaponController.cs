using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject[] Weapon;
    void Start()
    {
        PlayerManager.Instance.Player.addWeapon += EquipWeapon;
    }

    void EquipWeapon(WeaponType type)
    {
        int index = (int)type;
        if(index == 0)
        {
            PlayerManager.Instance.Player.jumpPos *= 1.5f;
        }
        else if(index == 1)
        {
            PlayerManager.Instance.Player.moveSpeed *= 1.5f;
        }
        else if(index == 2) 
        {
            ConditionManager.instance.condition.maxHp = 10;
            ConditionManager.instance.condition.curHp = 10;
        }
        Weapon[index].SetActive(true);
    }
}
