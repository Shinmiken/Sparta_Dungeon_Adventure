using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public HpCondition health;
    public HpCondition stemina;

    private void Update()
    {
        if (PlayerManager.Instance.Player.isRun)
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
