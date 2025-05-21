using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpObject : MonoBehaviour
{
    [SerializeField] private int jumpPower = 3;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.Player.Rb.AddForce(Vector3.up * (PlayerManager.Instance.Player.jumpPos * jumpPower), ForceMode.Impulse);
        }
    }
}
