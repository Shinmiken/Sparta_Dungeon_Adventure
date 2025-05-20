using UnityEngine;

public interface IInteractable
{
    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public void OnInteract()
    {
        //Player ��ũ��Ʈ ���� ����
        PlayerManager.Instance.Player.itemData = data;
        PlayerManager.Instance.Player.addItem?.Invoke(data.consumType);
        Destroy(gameObject);
    }
}
