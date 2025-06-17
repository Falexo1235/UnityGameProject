using UnityEngine;

public class GrabScript : MonoBehaviour
{
    public KeyCode grabKey = KeyCode.Q;
    public Collider2D grabTrigger;
    public float grabDistance = 0.5f;
    public float grabForce = 100f;

    private GameObject grabbedObject;
    private DistanceJoint2D grabJoint;
    private bool isGrabbing;
    private Vector2 grabPoint; // Точка, в которой произошло захватывание

    void TryGrab()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(grabTrigger.bounds.center, grabDistance);
        float closestDist = Mathf.Infinity;
        GameObject closestObj = null;
        Vector2 closestPoint = Vector2.zero;

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Grabbable"))
            {
                // Находим ближайшую точку на коллайдере объекта к нашему грабберу
                Vector2 closestPointOnCollider = col.ClosestPoint(transform.position);
                float dist = Vector2.Distance(transform.position, closestPointOnCollider);

                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestObj = col.gameObject;
                    closestPoint = closestPointOnCollider;
                }
            }
        }

        if (closestObj != null)
        {
            grabbedObject = closestObj;
            grabPoint = closestPoint; // Сохраняем точку захвата

            // Создаем пустой GameObject для точки крепления
            GameObject jointAnchor = new GameObject("JointAnchor");
            jointAnchor.transform.position = closestPoint;
            jointAnchor.transform.SetParent(grabbedObject.transform);
            grabJoint = grabbedObject.AddComponent<DistanceJoint2D>();
            grabJoint.connectedBody = GetComponent<Rigidbody2D>();
            grabJoint.anchor = grabbedObject.transform.InverseTransformPoint(closestPoint); // Локальная точка крепления
            grabJoint.connectedAnchor = transform.InverseTransformPoint(closestPoint); // Локальная точка на граббере
            grabJoint.maxDistanceOnly = true;
            grabJoint.distance =0.1f;
            grabJoint.autoConfigureDistance = false;
            grabJoint.enableCollision = true;
            isGrabbing = true;
        }
    }

    void ReleaseObj()
    {
        if (grabJoint != null)
        {
            // Уничтожаем также anchor объект, если он существует
            if (grabJoint.connectedBody != null && grabJoint.connectedBody.gameObject.name == "JointAnchor")
            {
                Destroy(grabJoint.connectedBody.gameObject);
            }
            Destroy(grabJoint);
        }
        grabbedObject = null;
        isGrabbing = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(grabKey) && !isGrabbing)
        {
            TryGrab();
        }
        else if (Input.GetKeyUp(grabKey) && isGrabbing)
        {
            ReleaseObj();
        }
    }
}