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
        //Player 스크립트 먼저 수정
        PlayerManager.Instance.Player.itemData = data;
        PlayerManager.Instance.Player.addItem?.Invoke(data.consumType);
        Destroy(gameObject);
    }
}
