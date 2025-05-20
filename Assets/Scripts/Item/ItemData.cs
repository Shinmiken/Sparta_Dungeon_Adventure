using UnityEngine;

public enum ItemType
{
    Resource,
    CanConsum,
    CanWear
}

public enum ConsumType
{
    Jump = 0,
    Speed = 1,
    health = 2
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
    public ItemType type;
    public ConsumType consumType;
}
