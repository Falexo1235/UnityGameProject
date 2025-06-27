using UnityEngine;

public class GrabScript : MonoBehaviour
{
    public KeyCode grabKey = KeyCode.Q;
    public Collider2D grabTrigger;
    public Transform grabSocket;
    public float grabDistance = 0.5f;
    public float grabForce = 100f;
    public ControlSpritesScript spriteScript;
    public AudioClip grabSound;
    public AudioClip ungrabSound;

    private GameObject grabbedObject;
    private DistanceJoint2D grabJoint;
    private bool isGrabbing;
    private InventoryScript inventory;
    private IUsableItem currentUsableItem;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        inventory = InventoryScript.Instance;
        if (inventory == null)
        {
            Debug.LogError("Inventory instance not found!");
        }
    }

    bool CanGrab()
    {
        return inventory != null && !inventory.HasItemInHand(grabSocket);
    }

    void TryGrab()
    {
        if (!CanGrab())
        {
            return;
        }
        spriteScript.SetColor(1);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(grabTrigger.bounds.center, grabDistance);
        float closestDist = Mathf.Infinity;
        GameObject closestObj = null;
        Vector2 closestPoint = Vector2.zero;

        foreach (Collider2D col in colliders)
        {
            if (col.GetComponent<Ungrabbable>() == null)
            {
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

            GameObject jointAnchor = new GameObject("JointAnchor");
            jointAnchor.transform.position = closestPoint;
            jointAnchor.transform.SetParent(grabbedObject.transform);
            grabJoint = grabbedObject.AddComponent<DistanceJoint2D>();
            grabJoint.connectedBody = GetComponent<Rigidbody2D>();
            grabJoint.anchor = grabbedObject.transform.InverseTransformPoint(closestPoint); 
            grabJoint.connectedAnchor = transform.InverseTransformPoint(closestPoint);
            grabJoint.maxDistanceOnly = true;
            grabJoint.distance = 0.1f;
            grabJoint.autoConfigureDistance = false;
            grabJoint.enableCollision = false;
            isGrabbing = true;
            spriteScript.SetColor(2);
            audioSource.PlayOneShot(grabSound);
        }
    }

    void ReleaseObj()
    {
        if (grabJoint != null)
        {
            if (grabJoint.connectedBody != null && grabJoint.connectedBody.gameObject.name == "JointAnchor")
            {
                Destroy(grabJoint.connectedBody.gameObject);
            }
            Destroy(grabJoint);
            audioSource.PlayOneShot(ungrabSound);
        }
        grabbedObject = null;
        isGrabbing = false;
    }

    void Update()
    {
        if (!isGrabbing && inventory.HasItemInHand(grabSocket))
        {
            GameObject itemInHand = inventory.GetItemInHand(grabSocket);
            if (itemInHand != null)
            {
                currentUsableItem = itemInHand.GetComponent<IUsableItem>();
            }
        }
        else
        {
            currentUsableItem = null;
        }

        if (Input.GetKeyDown(grabKey))
        {
            if (currentUsableItem != null)
            {
                currentUsableItem.UseStart();
            }
            
        }

        else if (Input.GetKey(grabKey))
        {
            if (currentUsableItem != null)
            {
                currentUsableItem.UseHold();
            }
            else if (!isGrabbing)
            {
                TryGrab();
            }
        }

        else if (Input.GetKeyUp(grabKey))
        {
            if (currentUsableItem != null)
            {
                currentUsableItem.UseEnd();
            }
            else if (isGrabbing)
            {
                ReleaseObj();
            }
            spriteScript.SetColor(0);

        }
    }
}