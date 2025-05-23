using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewJumpObject : MonoBehaviour
{
    public Transform target; // 도착할 위치
    public GameObject charactor;
    private Transform originalParent; // 다시 부모 오브젝트로 옮기기 위한 변수
    public float speed = 20f;
    bool isRide;

    void Update()
    {
        if (isRide)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isRide = true;
            originalParent = collision.transform.parent;
            collision.transform.SetParent(transform); // 플레이어를 자식으로
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isRide = false;
            collision.transform.SetParent(originalParent); // 부모에서 분리
        }
    }
}
