using UnityEngine;

public enum ItemType
{
    Resource,
    CanConsum,
    CanWear
}

public enum ConsumType
{
    health
}

[System.Serializable]
public class ItemDataConsum
{
    public ConsumType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName ="New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string ItemName;
    public string ItemDiscription;
    public ItemType type;
    public Sprite Icon;
    public GameObject ItemPrefabs;

    [Header("Stacking")]
    public bool canStack;
    public float maxStack;

    [Header("Consum")]
    public ItemDataConsum[] consumables;
}
