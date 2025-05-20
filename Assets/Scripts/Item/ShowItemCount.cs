using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemCount : MonoBehaviour
{
    public TextMeshProUGUI[] ItemText;

    public int[] num = new int[3];

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

        num[index]--;
        ItemText[index].text = num[index].ToString();
        return true;
    }
}
