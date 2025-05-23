using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemCount : MonoBehaviour
{
    public TextMeshProUGUI[] ItemText; // 아이템 개수 텍스트
    public TextMeshProUGUI[] CoolTime; // 쿨타임 텍스트

    public int[] num = new int[3];
    int[] coolTime = {5,5,5};
    Coroutine[] coroutines = new Coroutine[3];
    void Start()
    {
        for(int i = 0; i < ItemText.Length; i++) // 각자의 위치에 개수 표시
        {
            ItemText[i].text = num[i].ToString();
        }
        PlayerManager.Instance.Player.addItem += ShowCount; // 아이템 개수 증가 이벤트 구독
    }

    private void ShowCount(ConsumType type) // 아이템 타입의 숫자 변수를 받아서 어떤 아이템인지 판단
    {
        int index = (int)type;
        num[index]++;
        ItemText[index].text = num[index].ToString();// 개수 증가 표시
    }

    public bool UseItem(ConsumType type) // 아이템 사용
    {
        int index = (int)type;
        if(num[index] <= 0) return false; // 아이템이 없은면 사용 x

        
        if(coroutines[index] == null) // 표시 개수 감소
        {
            num[index]--;
            ItemText[index].text = num[index].ToString();
            coroutines[index] = StartCoroutine(ShowCoolTime(type)); // 지속 시간 표시

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
        CoolTime[index].text = ""; // 지속 시간 표시 초기화
        coroutines[index] = null;
        coolTime[index] = 5;
    }
}
