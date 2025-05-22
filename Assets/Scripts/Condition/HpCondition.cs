using UnityEngine;
using UnityEngine.UI;

public class HpCondition : MonoBehaviour
{
    public float curHp;
    public float maxHp = 5;
    public float startHp;

    public Image HpBar;

    private void Start()
    {
        curHp = startHp;
    }

    private void Update()
    {
        HpBar.fillAmount = GetPer();
    }

    public void Add(float value)
    {
        curHp = Mathf.Min(curHp + value, maxHp);
    }

    public void Minus(float value)
    {
        curHp = Mathf.Max(curHp - value, 0);
    }

    private float GetPer()
    {
        return curHp / maxHp;
    }
}
