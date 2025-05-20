using UnityEngine;
using UnityEngine.UI;

public class ShowInventory : MonoBehaviour
{
    public GameObject invetory;
    public GameObject Icon;

    void Start()
    {
        invetory.SetActive(true);
        GameObject go = Instantiate(Icon);
        go.transform.parent = this.transform;
    }
}
