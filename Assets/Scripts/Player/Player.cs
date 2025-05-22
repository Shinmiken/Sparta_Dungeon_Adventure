using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed; // �÷��̾� ���ǵ�
    private float speed;
    public float jumpPos; // ���� ����
    private Vector2 moveVec; // ������ ����
    private Rigidbody rb;
    public Rigidbody Rb => rb;

    [Header("Camera")]
    public Transform cameraRoot;     // �¿� ȸ����
    public Transform cameraVertical; // ���� ȸ����
    public Transform cameraTransform;
    public float minCur;
    public float maxCur;
    private float camCurY; // ����
    private float camCurX; // �¿�
    public float camSensitivity;
    public float distance = 4f;

    private Vector2 curDelta;

    [Header("Ray")]
    public LayerMask groundLayer; // ���̷� üũ�� ��
    public float rayLength = 0.1f; // ���� ����
    public float rayOffset = 0.2f; // ���� ���� ����


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
        // ī�޶� ���� ���� ���ϱ� (XZ ��� ����)
        Vector3 camForward = cameraRoot.forward;
        Vector3 camRight = cameraRoot.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // �Է� ���� ����
        speed = isRun ? moveSpeed * 2 : moveSpeed;
        Vector3 moveDir = camForward * moveVec.y + camRight * moveVec.x;
        moveDir *= speed;
        moveDir.y = rb.velocity.y; // y���� ���� �ӵ� ���� (���� ��)

        rb.velocity = moveDir;

        //// �̵��� �� �÷��̾ ���� ���⵵ ���� ȸ����Ű�� �ʹٸ� �� �ڵ� �߰�
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

        transform.rotation = Quaternion.Euler(0, camCurX, 0); // �÷��̾� �ֺ� ȸ��
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
        Vector3 basePosition = transform.position; // ���� ��� ������ġ
        basePosition.y += 0.5f;
        Ray[] ray = new Ray[5]
        {
            new Ray(basePosition + transform.forward * rayOffset, Vector3.down), // ���̰� ��� ��ġ
            new Ray(basePosition - transform.forward * rayOffset, Vector3.down),
            new Ray(basePosition + transform.right *  rayOffset, Vector3.down),
            new Ray(basePosition - transform.right * rayOffset, Vector3.down),
            new Ray(basePosition, Vector3.down)
        };

        for(int i = 0; i < ray.Length; i++)
        {
            Debug.DrawRay(ray[i].origin, ray[i].direction * rayLength, Color.red); //��� �ִ� ���� Ȯ��

            if (Physics.Raycast(ray[i], rayLength, groundLayer)) // ���̰� ���̶� ��Ҵ��� Ȯ��
            {
                return true;
            }
        }
        return false;
    }
}
