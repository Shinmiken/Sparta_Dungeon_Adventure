using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    private static ConditionManager Instance;
    public static ConditionManager instance => Instance;

    HpCondition hpCondition;
    public HpCondition condition => hpCondition;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        hpCondition = GetComponentInChildren<HpCondition>();
    }
}
