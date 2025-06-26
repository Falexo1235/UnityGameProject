using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 15f;
    public float damage = 10f;
    public GameObject explosionPrefab;
    public AnimationClip hitClip;
    public AudioClip hitSound;
    private Vector3 animationOffset = Vector3.forward * -0.5f;

    private Vector2 direction;

    private void Start()
    {
        direction = gameObject.transform.right;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ProjectileInteractable hitScript = collision.collider.GetComponent<ProjectileInteractable>();
        if (hitScript != null)
        {
            hitScript.Activate();
        }

        ContactPoint2D contact = collision.contacts[0];
        Vector2 normal = contact.normal;
        float angle = Mathf.Atan2(normal.y, normal.x) * Mathf.Rad2Deg - 90f;
        GameObject explosion = Instantiate(explosionPrefab, transform.position + animationOffset, Quaternion.AngleAxis(angle, Vector3.forward));
        explosion.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        explosion.GetComponent<ExplosionScript>().Playexplosion(hitClip, hitSound);
        Destroy(gameObject);
    }
} 