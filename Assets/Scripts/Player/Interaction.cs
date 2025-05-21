using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    private IInteractable curInteractable;

    public TextMeshProUGUI promptText;

    string discripsion = "[E] »πµÊ«œ±‚";

    public LayerMask layerMask;
    public LayerMask ladderMask;
    [SerializeField] private float maxCheck = 5.0f;
    [SerializeField] private float ladderCheck = 0.5f;
    private GameObject curInteractGameObject = null;

    private void Update()
    {
        CheckInfo();
        CheckLadder();
    }

    public bool CheckLadder()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * ladderCheck, Color.red);

        if (Physics.Raycast(ray, out hit, ladderCheck, ladderMask))
        {
            return true;
        }
        return false;

    }

    void CheckInfo()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //Debug.DrawRay(ray.origin, ray.direction * maxCheck, Color.red);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCheck, layerMask))
        {
            if (hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                curInteractable = hit.collider.GetComponent<IInteractable>();
                SetPromptText();
            }
        }
        else
        {
            curInteractGameObject = null;
            curInteractable= null;
            promptText.gameObject.SetActive(false);
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = discripsion;
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null)
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
