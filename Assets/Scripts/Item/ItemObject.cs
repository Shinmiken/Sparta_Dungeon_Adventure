using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.ItemName}\n{data.ItemDiscription}";
        return str;
    }

    public void OnInteract()
    {
        //Player ��ũ��Ʈ ���� ����
        PlayerManager.Instance.Player.itemData = data;
        PlayerManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
