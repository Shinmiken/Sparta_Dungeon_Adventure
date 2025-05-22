using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed; // 플레이어 스피드
    private float speed;
    public float jumpPos; // 점프 높이
    private Vector2 moveVec; // 움직일 벡터
    private Rigidbody rb;
    public Rigidbody Rb => rb;

    [Header("Camera")]
    public Transform cameraRoot;     // 좌우 회전용
    public Transform cameraVertical; // 상하 회전용
    public Transform cameraTransform;
    public float minCur;
    public float maxCur;
    private float camCurY; // 상하
    private float camCurX; // 좌우
    public float camSensitivity;
    public float distance = 4f;

    private Vector2 curDelta;

    [Header("Ray")]
    public LayerMask groundLayer; // 레이로 체크할 땅
    public float rayLength = 0.1f; // 레이 길이
    public float rayOffset = 0.2f; // 레이 간에 간격


    [Header("Run")]
    public bool isRun = false;
    public bool isSuper = false;

    [Header("Ladder")]
    public bool isClimb = false;
    private float climbSpeed = 3.0f;

    public ItemData itemData;
    public Action<ConsumType> addItem;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        IsGround();
    }

    private void FixedUpdate()
    {
        if (isClimb && PlayerManager.Instance.Interaction.CheckLadder())
        {
            rb.velocity = Vector2.up * climbSpeed;
        }
        else
        {
            if (isClimb)
            {
                isClimb = false;
                rb.useGravity = true;
                rb.velocity = Vector3.zero;
            }
            PlayerMove();
        }
    }

    private void LateUpdate()
    {
        CameraMove();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            moveVec = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            moveVec = Vector2.zero;
        }
    }

    private void PlayerMove()
    {
        // 카메라 기준 방향 구하기 (XZ 평면 기준)
        Vector3 camForward = cameraRoot.forward;
        Vector3 camRight = cameraRoot.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // 입력 방향 적용
        speed = isRun ? moveSpeed * 2 : moveSpeed;
        Vector3 moveDir = camForward * moveVec.y + camRight * moveVec.x;
        moveDir *= speed;
        moveDir.y = rb.velocity.y; // y축은 기존 속도 유지 (점프 등)

        rb.velocity = moveDir;

        //// 이동할 때 플레이어가 보는 방향도 같이 회전시키고 싶다면 이 코드 추가
        //if (moveVec != Vector2.zero)
        //{
        //    Quaternion targetRot = Quaternion.LookRotation(moveDir);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
        //}
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGround())
        {
            rb.AddForce(Vector2.up * jumpPos, ForceMode.Impulse);
        }
    }

    public void OnCamera(InputAction.CallbackContext context)
    {
        curDelta = context.ReadValue<Vector2>();
    }

    private void CameraMove()
    {
        camCurX += curDelta.x * camSensitivity;
        camCurY -= curDelta.y * camSensitivity;
        camCurY = Mathf.Clamp(camCurY, minCur, maxCur);

        transform.rotation = Quaternion.Euler(0, camCurX, 0); // 플레이어 주변 회전
        cameraVertical.localRotation = Quaternion.Euler(camCurY, 0, 0);

        cameraTransform.localPosition = new Vector3(0, 0, -distance);
        cameraTransform.LookAt(cameraRoot);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRun = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRun = false;
        }
    }

    public void OnLadder(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && PlayerManager.Instance.Interaction.CheckLadder())
        {
            isClimb = true;
            rb.useGravity = false;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isClimb = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
        }
    }

    public void Item_1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (ItemManager.instance.ItemCount.UseItem(ConsumType.Jump))
            {
                StartCoroutine(FirstItem());
            }
        }
    }
    IEnumerator FirstItem()
    {
        jumpPos *= 2;
        yield return new WaitForSeconds(5f);
        jumpPos /= 2;
    }
    public void Item_2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (ItemManager.instance.ItemCount.UseItem(ConsumType.Speed))
            {
                StartCoroutine(SecondItem());
            }
        }
    }
    IEnumerator SecondItem()
    {
        moveSpeed *= 2;
        yield return new WaitForSeconds(5f);
        moveSpeed /= 2;
    }
    public void Item_3(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (ItemManager.instance.ItemCount.UseItem(ConsumType.health))
            {
                StartCoroutine(ThirdItem());
            } 
        }
    }
    IEnumerator ThirdItem()
    {
        isSuper = true;
        yield return new WaitForSeconds(5f);
        isSuper = false;
    }

    bool IsGround()
    {
        Vector3 basePosition = transform.position; // 레이 쏘는 시작위치
        basePosition.y += 0.5f;
        Ray[] ray = new Ray[5]
        {
            new Ray(basePosition + transform.forward * rayOffset, Vector3.down), // 레이가 쏘는 위치
            new Ray(basePosition - transform.forward * rayOffset, Vector3.down),
            new Ray(basePosition + transform.right *  rayOffset, Vector3.down),
            new Ray(basePosition - transform.right * rayOffset, Vector3.down),
            new Ray(basePosition, Vector3.down)
        };

        for(int i = 0; i < ray.Length; i++)
        {
            Debug.DrawRay(ray[i].origin, ray[i].direction * rayLength, Color.red); //쏘고 있는 레이 확인

            if (Physics.Raycast(ray[i], rayLength, groundLayer)) // 레이가 땅이라 닿았는지 확인
            {
                return true;
            }
        }
        return false;
    }
}
