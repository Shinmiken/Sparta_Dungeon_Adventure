using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemCount : MonoBehaviour
{
    public TextMeshProUGUI[] ItemText;
    public TextMeshProUGUI[] CoolTime;

    public int[] num = new int[3];
    int[] coolTime = {5,5,5};
    Coroutine[] coroutines = new Coroutine[3];
    void Start()
    {
        for(int i = 0; i < ItemText.Length; i++)
        {
            ItemText[i].text = num[i].ToString();
        }
        PlayerManager.Instance.Player.addItem += ShowCount;
    }

    private void ShowCount(ConsumType type)
    {
        int index = (int)type;
        num[index]++;
        ItemText[index].text = num[index].ToString();
    }

    public bool UseItem(ConsumType type)
    {
        int index = (int)type;
        if(num[index] <= 0) return false;

        
        if(coroutines[index] == null)
        {
            num[index]--;
            ItemText[index].text = num[index].ToString();
            coroutines[index] = StartCoroutine(ShowCoolTime(type));

            return true;
        }
        return false;
    }

    IEnumerator ShowCoolTime(ConsumType type)
    {
        int index = (int)type;
        for (int i = 0; i< 5; i++)
        {
            CoolTime[index].text = coolTime[index].ToString();
            yield return new WaitForSeconds(1f);
            coolTime[index]--;
        }
        CoolTime[index].text = "";
        coroutines[index] = null;
        coolTime[index] = 5;
    }
}
