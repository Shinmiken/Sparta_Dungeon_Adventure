using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public HpCondition health;
    public HpCondition stemina;

    private void Update()
    {
        if (PlayerManager.Instance.Player.isRun && !PlayerManager.Instance.Player.isSuper) // 플레이어가 달리면 스테미나 감소
        {
            if (stemina.curHp <= 0)
            {
                if(health.curHp <= 0)
                {
                    Time.timeScale = 0.0f;
                }
                health.Minus(Time.deltaTime);
                return;
            }
            stemina.Minus(Time.deltaTime);
        }
        else
        {
            if (stemina.curHp >= stemina.maxHp) return;
            stemina.Add(Time.deltaTime);
        }
    }
}
