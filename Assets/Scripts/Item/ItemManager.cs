using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager Instance;
    public static ItemManager instance => Instance;

    ShowItemCount itemCount;
    public ShowItemCount ItemCount => itemCount;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        itemCount = GetComponent<ShowItemCount>();
    }
}
