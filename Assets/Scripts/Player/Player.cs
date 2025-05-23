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
    private Vector2 moveVec; // ������ ����
    private Rigidbody rb;
    public Rigidbody Rb => rb;

    [Header("Camera")]
    public Transform cameraPos;
    public float minCur;
    public float maxCur;
    private float camCurY; // ����
    private float camCurX; // �¿�
    public float camSensitivity;

    private Vector2 curDelta;

    [Header("Ray")]
    public LayerMask groundLayer; // ���̷� üũ�� ��
    public float rayLength = 0.1f; // ���� ����
    public float rayOffset = 0.2f; // ���� ���� ����

    [Header("Jump")]
    private bool wasGround;
    public float jumpPos; // ���� ����

    [Header("Run")]
    public bool isRun = false;
    public bool isSuper = false;

    [Header("Ladder")]
    public bool isClimb = false;
    private float climbSpeed = 3.0f;

    public ItemData itemData;
    public Action<ConsumType> addItem;
    public Action<WeaponType> addWeapon;
    public Animator animator;

    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        bool isGround = IsGround();
        if (isGround && !wasGround)
        {
            animator.SetBool("isJump", false);
        }
        wasGround = isGround;
    }

    private void FixedUpdate()
    {
        if (isClimb && PlayerManager.Instance.Interaction.CheckLadder()) // ��ٸ� üũ
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
            animator.SetBool("isWalk", true);

        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            moveVec = Vector2.zero;
            animator.SetBool("isWalk", false);
        }
    }

    private void PlayerMove()
    {
        //ī�޶� ���� ���� ���ϱ�(XZ ��� ����)
        Vector3 camForward = cameraPos.forward;
        Vector3 camRight = cameraPos.right;

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

        // �̵��� �� �÷��̾ ���� ���⵵ ���� ȸ��
        if (moveVec != Vector2.zero)
        {
            Vector3 lookDir = moveDir;
            lookDir.y = 0f; // ���� ���� ���� (Y�� ȸ���� �ݿ��ǵ���)

            if (lookDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGround())
        {
            rb.AddForce(Vector2.up * jumpPos, ForceMode.Impulse);
            animator.SetBool("isJump", true);
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

        cameraPos.localRotation = Quaternion.Euler(camCurY, camCurX,0f);
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isRun = true;
            animator.SetBool("isRun", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isRun = false;
            animator.SetBool("isRun", false);
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
