using UnityEngine;
using UnityEngine.Events;
public class EventInvokeScript : MonoBehaviour
{
    public GameObject explosionPrefab;
    public AudioClip itemSound;
    public AnimationClip itemClip;
    public UnityEvent events;
    private Vector3 animationOffset = Vector3.forward * -0.5f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerTag playerPart = collision.GetComponent<PlayerTag>();
        if (playerPart != null)
        {

            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position + animationOffset, Quaternion.identity);
                explosion.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                explosion.GetComponent<ExplosionScript>().Playexplosion(itemClip ? itemClip : null, itemSound);
            }
            events?.Invoke();
            Destroy(gameObject);
        }
    }
}
