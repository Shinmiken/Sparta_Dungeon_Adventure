using UnityEngine;

public class RayInteract : MonoBehaviour
{
    public LayerMask layerMask;
    [SerializeField] private float maxCheck = 5.0f;
    private GameObject curInteractGameObject = null;

    private void Update()
    {
        CheckInfo();
    }

    void CheckInfo()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * maxCheck, Color.red);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, maxCheck, layerMask))
        {
            if(hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                Debug.Log(hit.collider.gameObject.name);
            }
        }
        else
        {
            curInteractGameObject = null;
        }
    }
}
