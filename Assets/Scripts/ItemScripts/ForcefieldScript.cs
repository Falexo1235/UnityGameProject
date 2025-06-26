using UnityEngine;
//Had to rip the shield script in 2 parts due to the "OnCollisionEnter" inner workings. Much easier that way. 
public class ForcefieldScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D otherRb = collision.rigidbody;
        if (otherRb != null && otherRb.bodyType == RigidbodyType2D.Dynamic)
        {
            Vector2 normal = -transform.up;
            //Only works on dynamic objects.
            otherRb.AddForce(normal*10, ForceMode2D.Impulse);
        }
    }
}
