using UnityEngine;
using UnityEngine.Events;

public class MissileInteractable : MonoBehaviour
{
    [Tooltip("Put an object with a needed function here")]
    public UnityEvent OnMissileHit;
    public void Activate()
    {
        OnMissileHit?.Invoke();
    }
}
