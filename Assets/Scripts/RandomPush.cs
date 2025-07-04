using UnityEngine;

public class RandomPush : MonoBehaviour
{
    public float pushForce = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.AddForce(randomDirection * pushForce, ForceMode2D.Impulse);
    }
}