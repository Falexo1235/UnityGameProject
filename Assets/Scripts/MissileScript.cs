using UnityEngine;
//Homing missile, follows cursor. Make a narrow maze for missile to navigate.
//TODO: add slowmotion + camera follow trigger prefab for these mazes
public class MissileScript : MonoBehaviour
{
    public float thrustForce = 20f;
    public float rotationSpeed = 400f;
    public float maxSpeed = 10f;
    public float lifetime = 10f;
    
    private Rigidbody2D rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        Vector2 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToTarget = (targetPosition - (Vector2)transform.position).normalized;

        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle + 90f, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(angle);

        rb.AddForce(transform.right * thrustForce);

        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MissileInteractable hitScript = collision.GetComponent<MissileInteractable>();
        if (hitScript != null)
        {
            hitScript.Activate();
        }
        Destroy(gameObject);
    } 
} 