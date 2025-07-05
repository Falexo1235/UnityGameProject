using UnityEngine;
public class MeteorScript : MonoBehaviour
{
    [Header("Meteor Settings")]
    public float lifetime = 5f;
    public float fallSpeed = 10f;
    public int damageAmount = 1;
    
    private Rigidbody2D rb;
    private bool isActivated = false;
    private float activationTime;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerTag player = other.GetComponent<PlayerTag>();
        if (player != null && !isActivated)
        {
            ActivateMeteor();
        }
    }
    
    void ActivateMeteor()
    {
        isActivated = true;
        activationTime = Time.time;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = Vector2.down * fallSpeed;
        gameObject.GetComponent<AudioSource>().Play();
        Destroy(gameObject, lifetime);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerTag player = collision.collider.GetComponent<PlayerTag>();
        if (player != null && isActivated)
        {
            if (InventoryScript.Instance != null)
            {
                InventoryScript.Instance.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }
}