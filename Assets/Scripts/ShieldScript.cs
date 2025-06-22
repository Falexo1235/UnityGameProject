using UnityEngine;

public class ShieldScript : MonoBehaviour, IUsableItem
{
    public GameObject forceField;
    
    //I don't know if it's even useful with customizable charge in the inventory, but I'll leave it here
    public float drainRate = 1.0f;
    
    //Public for inventory manager, needed to drain charge in the inventory itself
    [HideInInspector]
    public int itemIndex = -1;

    void Start()
    {
        forceField.SetActive(false);
    }

    void Update()
    {
        if (itemIndex == -1 || !forceField.activeSelf) return;

        float currentCharge = InventoryScript.Instance.GetCharge(itemIndex);
        currentCharge -= drainRate * Time.deltaTime;

        if (currentCharge <= 0)
        {
            currentCharge = 0;
            forceField.SetActive(false);
        }
        
        InventoryScript.Instance.SetCharge(itemIndex, currentCharge);
    }

    public void UseStart()
    {
        if (InventoryScript.Instance.GetCharge(itemIndex) > 0)
        {
            forceField.SetActive(true);
        }
    }

    public void UseHold() { }

    public void UseEnd()
    {
        forceField.SetActive(false);
    }
}
