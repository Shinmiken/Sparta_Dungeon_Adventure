using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed; // �÷��̾� ���ǵ�
    public float jumpPos; // ���� ����
    private Vector2 moveVec; // ������ ����
    private Rigidbody rb;
    public Rigidbody Rb => rb;

    [Header("Camera")]
    public Transform cameraRoot;     // �¿� ȸ����
    public Transform cameraVertical; // ���� ȸ����
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


    [Header("Run")]
    public bool isRun = false;


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
        PlayerMove();
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
        float speed = isRun ? moveSpeed * 2 : moveSpeed;
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

        // �¿� ȸ���� �÷��̾� ȸ������ ó�� (ī�޶� ��Ʈ ȸ��)
        cameraRoot.localRotation = Quaternion.Euler(0, camCurX, 0);

        // ���� ȸ���� ī�޶� ��ü�� ����
        cameraVertical.localRotation = Quaternion.Euler(camCurY, 0, 0);
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

    bool IsGround()
    {
        Vector3 basePosition = transform.position + Vector3.down * 0.5f; // ���� ��� ������ġ

        Ray[] ray = new Ray[4]
        {
            new Ray(basePosition + transform.forward * rayOffset, Vector3.down), // ���̰� ��� ��ġ
            new Ray(basePosition - transform.forward * rayOffset, Vector3.down),
            new Ray(basePosition + transform.right *  rayOffset, Vector3.down),
            new Ray(basePosition - transform.right * rayOffset, Vector3.down)
        };

        for(int i = 0; i < ray.Length; i++)
        {
            //Debug.DrawRay(ray[i].origin, ray[i].direction * rayLength, Color.red); ��� �ִ� ���� Ȯ��

            if (Physics.Raycast(ray[i], rayLength, groundLayer)) // ���̰� ���̶� ��Ҵ��� Ȯ��
            {
                return true;
            }
        }
        return false;
    }
}
