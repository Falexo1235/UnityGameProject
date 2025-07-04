using UnityEngine;

public class DamagingObject : MonoBehaviour
{
    public int damageAmount = 1;

    private InventoryScript inventoryScript;

    private void Awake()
    {
        GameObject inventoryManager = GameObject.Find("InventoryManager");
        if (inventoryManager != null)
        {
            inventoryScript = inventoryManager.GetComponent<InventoryScript>();
        }
        else
        {
            Debug.LogError("InventoryManager not found in scene!");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerTag player = collision.collider.GetComponent<PlayerTag>();
        if (player != null && inventoryScript != null)
        {
            inventoryScript.TakeDamage(damageAmount);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerTag player = other.GetComponent<PlayerTag>();
        if (player != null && inventoryScript != null)
        {
            inventoryScript.TakeDamage(damageAmount);
        }
    }
} 