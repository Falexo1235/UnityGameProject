using UnityEngine;
public class LimbControlScript : MonoBehaviour
{
    public float accelerationTorque = 10f;
    public float maxSpeed = 600f;
    public KeyCode controlButton = KeyCode.Mouse0;
    public Transform startPoint;
    public bool reverseEndPoint = true;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private int reversed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        reversed = (reverseEndPoint ? -1 : 1);
    }

    void Update()
    {
        float targetSpeed = 0f;

        if (Input.GetKey(controlButton))
        {
            
            Vector2 cursorWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 limbToCursor = cursorWorldPos - (Vector2)startPoint.position;
            Vector2 limbDirection = reversed * transform.up;
            float angle = Vector2.SignedAngle(limbDirection, limbToCursor);
            float dotProduct = Vector2.Dot(limbToCursor.normalized, reversed*transform.up);
            targetSpeed = maxSpeed*dotProduct;
            float currentSpeed = rb.angularVelocity;
            float speedDifference = targetSpeed - currentSpeed;

            rb.AddTorque(speedDifference * accelerationTorque * Time.deltaTime);

        }
        else
        {
            rb.angularVelocity = 0;
            rb.totalTorque = 0;
        }

        

    }
}
