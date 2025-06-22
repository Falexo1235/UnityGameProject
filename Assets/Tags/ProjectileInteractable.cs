using UnityEngine;
using UnityEngine.Events;

public class ProjectileInteractable : MonoBehaviour
{
    [Tooltip("Put an object with a needed function here")]
    public UnityEvent OnProjectileHit;
    public void Activate()
    {
        OnProjectileHit?.Invoke();
    }
}
