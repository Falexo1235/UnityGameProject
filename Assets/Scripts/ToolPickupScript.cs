using UnityEngine;
using UnityEngine.UI;

public class ToolPickupScript : MonoBehaviour
{
    public Image menuOption;
    public Sprite newIcon;
    public InventoryScript inventoryScript;
    public int index;
    public GameObject itemPrefab;
    public GameObject explosionPrefab;
    public AudioClip itemSound;
    public AnimationClip itemClip;
    private Vector3 animationOffset = Vector3.forward * -0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerTag playerPart = collision.GetComponent<PlayerTag>();
        if (playerPart != null)
        {
            menuOption.sprite = newIcon;

            inventoryScript.availableItems[index] = itemPrefab;

            GameObject explosion = Instantiate(explosionPrefab, transform.position + animationOffset, Quaternion.identity);
            explosion.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            explosion.GetComponent<ExplosionScript>().Playexplosion(itemClip ? itemClip : null, itemSound);
            Destroy(gameObject);
        }
    }
}
