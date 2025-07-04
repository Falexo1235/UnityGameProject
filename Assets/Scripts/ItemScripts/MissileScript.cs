using UnityEngine;
//Homing missile, follows cursor. Make a narrow maze for missile to navigate.
public class MissileScript : MonoBehaviour
{
    public static MissileScript Instance { get; private set; }
    
    [Header("Missile settings")]
    public float thrustForce = 20f;
    public float rotationSpeed = 400f;
    public float maxSpeed = 10f;
    public float lifetime = 10f;
    public float lifetimeDecayMultiplier = 1f;
    public AudioClip launchSound;
    public AudioClip bounceSound;
    
    [Header("Maze Mode")]
    public float bounceForce = 5f;
    public float mazeLifetimeReduction = 2f;
    
    [Header("Explosion settings")]
    public AnimationClip explosionClip;
    public GameObject explosionPrefab;
    public AudioClip explosionSound;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private bool isInMazeMode = false;
    private float originalThrustForce;
    private float originalMaxSpeed;
    private Vector2 lastVelocity;
    private Vector3 animationOffset = Vector3.forward * -0.5f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(launchSound);
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        originalThrustForce = thrustForce;
        originalMaxSpeed = maxSpeed;
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    void FixedUpdate()
    {
        lifetime -= Time.fixedDeltaTime * lifetimeDecayMultiplier;
        if (lifetime <= 0)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position + animationOffset, Quaternion.identity);
            explosion.GetComponent<ExplosionScript>().Playexplosion(explosionClip, explosionSound);
            Destroy(gameObject);
            return;
        }
        
        Vector2 targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 directionToTarget = (targetPosition - (Vector2)transform.position).normalized;

        float targetAngle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg - 90f;
        float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetAngle + 90f, rotationSpeed * Time.fixedDeltaTime);
        rb.MoveRotation(angle);

        rb.AddForce(transform.right * thrustForce);

        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
        lastVelocity = rb.linearVelocity;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        MissileInteractable hitScript = collision.collider.GetComponent<MissileInteractable>();
        if (hitScript != null)
        {
            hitScript.Activate();
        }

        if (isInMazeMode)
        {
            var inDirection = lastVelocity.normalized;
            var normal = collision.contacts[0].normal;
            var reflectDirection = Vector2.Reflect(inDirection, normal);

            rb.linearVelocity = reflectDirection * bounceForce;
            
            lifetime = Mathf.Max(0, lifetime - mazeLifetimeReduction);
            GetComponent<AudioSource>().PlayOneShot(bounceSound);
            Debug.Log(lifetime);
            if (lifetime <= 0)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position + animationOffset, Quaternion.identity);
                explosion.GetComponent<ExplosionScript>().Playexplosion(explosionClip, explosionSound);
                Destroy(gameObject);
            }
        }
        else
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position + animationOffset, Quaternion.identity);
            explosion.GetComponent<ExplosionScript>().Playexplosion(explosionClip, explosionSound);
            Destroy(gameObject);
        }
    }
    
    public void SetMazeMode(bool enabled, float speedMult =0.5f)
    {
        isInMazeMode = enabled;
        
        if (enabled)
        {
            lifetimeDecayMultiplier = speedMult;
            thrustForce = originalThrustForce * speedMult;
            maxSpeed = originalMaxSpeed * speedMult;
        }
        else
        {
            lifetimeDecayMultiplier = speedMult;
            thrustForce = originalThrustForce;
            maxSpeed = originalMaxSpeed;
        }
        
    }
    
} 