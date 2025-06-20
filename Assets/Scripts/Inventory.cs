using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    [Header("Hand References")]
    public Transform leftHandSocket;
    public Transform rightHandSocket;
    
    [Header("Items")]
    //Do not put useful items in index 0, that index is used to select hands
    public GameObject[] availableItems;
    
    private GameObject leftHandItem;
    private GameObject rightHandItem;
    private bool isRightHandActive = true;
    private int currentItemIndex = -1;
    
    private Collider2D leftHandCollider;
    private Collider2D rightHandCollider;
    
    private void Awake()
    {
        Instance = this;
        leftHandCollider = leftHandSocket.GetComponent<Collider2D>();
        rightHandCollider = rightHandSocket.GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchActiveHand();
        }
    }

    private void UpdateHandColliders()
    {
        leftHandCollider.enabled = (leftHandItem == null);
        rightHandCollider.enabled = (rightHandItem == null);
    }

    public void EquipItem(int itemIndex)
    {
        if (itemIndex == 0)
        {
            RemoveCurrentItem();
            currentItemIndex = -1;
            return;
        }
        if (itemIndex < 0 || itemIndex >= availableItems.Length)
            return;

        if (itemIndex == currentItemIndex)
            return;

        currentItemIndex = itemIndex;
        UpdateItemInHand();
    }

    private void UpdateItemInHand()
    {
        if (leftHandItem != null)
        {
        Destroy(leftHandItem);
            leftHandItem = null;
        }
        if (rightHandItem != null)
        {
            Destroy(rightHandItem);
            rightHandItem = null;
        }

        UpdateHandColliders();

        if (currentItemIndex > 0 && currentItemIndex < availableItems.Length)
        {
            Transform activeSocket = isRightHandActive ? rightHandSocket : leftHandSocket;
            
            GameObject newItem = Instantiate(availableItems[currentItemIndex], activeSocket.position, activeSocket.rotation);
            newItem.transform.SetParent(activeSocket);

            if (!isRightHandActive)
            {
                Vector3 scale = newItem.transform.localScale;
                scale.x *= -1;
                newItem.transform.localScale = scale;

                //Scaling didn't change firePoint direction, had to change it manually
                Blaster blaster = newItem.GetComponent<Blaster>();
                if (blaster != null && blaster.firePoint != null)
                {
                    blaster.firePoint.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }
            FixedJoint2D joint = newItem.AddComponent<FixedJoint2D>();
            Rigidbody2D handRb = null;
            if (activeSocket.parent != null)
            {
                handRb = activeSocket.parent.GetComponent<Rigidbody2D>();
            }
            joint.connectedBody = handRb;
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector2.zero;
            joint.connectedAnchor = activeSocket.localPosition;
                
            if (isRightHandActive)
            {
                rightHandItem = newItem;
            }
            else
            {
                leftHandItem = newItem;
            }
            UpdateHandColliders();
        }
    }

    public void SwitchActiveHand()
    {
        isRightHandActive = !isRightHandActive;
        UpdateItemInHand();
        Debug.Log($"Switched to {(isRightHandActive ? "right" : "left")} hand");
    }

    public void RemoveCurrentItem()
    {
        if (isRightHandActive && rightHandItem != null)
        {
            Destroy(rightHandItem);
            rightHandItem = null;
        }
        else if (!isRightHandActive && leftHandItem != null)
        {
            Destroy(leftHandItem);
            leftHandItem = null;
        }
        UpdateHandColliders();
    }
    public bool HasItemInHand(Transform handTransform)
    {
        if (handTransform == leftHandSocket)
        {
            return leftHandItem != null;
        }
        else if (handTransform == rightHandSocket)
        {
            return rightHandItem != null;
        }
        
        Debug.LogError("Wrong hand transform");
        return false;
    }

    public GameObject GetItemInHand(Transform handTransform)
    {
        if (handTransform == leftHandSocket)
        {
            return leftHandItem;
        }
        else if (handTransform == rightHandSocket)
        {
            return rightHandItem;
        }
        Debug.LogError("Wrong hand transform");
        return null;
    }
} 