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
    public float[] maxItemCharges; //Max charge for each item. Easier to do it that way, than to try finding out if an item has charge
    
    private GameObject leftHandItem;
    private GameObject rightHandItem;
    private bool isRightHandActive = true;
    private int currentItemIndex = -1;
    
    private Collider2D leftHandCollider;
    private Collider2D rightHandCollider;

    private float[] currentItemCharges; //Current item charges
    
    private void Awake()
    {
        Instance = this;
        leftHandCollider = leftHandSocket.GetComponent<Collider2D>();
        rightHandCollider = rightHandSocket.GetComponent<Collider2D>();

        //All items are fully charged at the start of the level
        if (availableItems.Length > 0 && maxItemCharges.Length == availableItems.Length)
        {
            currentItemCharges = new float[availableItems.Length];
            for (int i = 0; i < maxItemCharges.Length; i++)
            {
                currentItemCharges[i] = maxItemCharges[i];
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SwitchActiveHand();
        }

        RegenerateAllItemCharges();
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

            //If it's a shield, put itemIndex in it for charge management
            Shield shield = newItem.GetComponent<Shield>();
            if (shield != null)
            {
                shield.itemIndex = currentItemIndex;
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

    //New charge management functions

    public float GetCharge(int itemIndex)
    {
        if (itemIndex > 0 && itemIndex < currentItemCharges.Length)
        {
            return currentItemCharges[itemIndex];
        }
        return 0f;
    }

    public void SetCharge(int itemIndex, float value)
    {
        if (itemIndex > 0 && itemIndex < currentItemCharges.Length)
        {
            currentItemCharges[itemIndex] = value;
        }
    }
    private void RegenerateAllItemCharges()
    {
        if (currentItemCharges == null || currentItemCharges.Length == 0) return;

        for (int i = 0; i < currentItemCharges.Length; i++)
        {
            if (maxItemCharges[i] > 0)
            {
                bool itemIsInUse = false;
                GameObject currentItemObject = null;
                if (currentItemIndex == i)
                {
                    currentItemObject = isRightHandActive ? rightHandItem : leftHandItem;
                }

                if (currentItemObject != null)
                {
                    //I don't know if it's better than just putting recharge script in the item itself,
                    //But that way at least charge speed is consistent.
                    Shield shield = currentItemObject.GetComponent<Shield>();
                    //Might change "shield.forceField.activeSelf" with a simple bool inside an item if problems arise.
                    if (shield != null && shield.forceField.activeSelf)
                    {
                        itemIsInUse = true;
                    }
                }
                if (!itemIsInUse && currentItemCharges[i] < maxItemCharges[i])
                {
                    float regenRate = 0.5f;
                    currentItemCharges[i] += regenRate * Time.deltaTime;
                    currentItemCharges[i] = Mathf.Clamp(currentItemCharges[i], 0, maxItemCharges[i]);
                }
            }
        }
    }
} 

