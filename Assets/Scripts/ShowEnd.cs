using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowEnd : MonoBehaviour
{
    public GameObject emdText;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            emdText.SetActive(true);
        }
    }
}
