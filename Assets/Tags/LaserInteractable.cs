using UnityEngine;
using UnityEngine.Events;
//Special tag, it actually does something
public class LaserInteractable : MonoBehaviour
{
    [Tooltip("Put an object with a needed function here")]
    public UnityEvent OnLaserHit;
    public void Activate()
    {
        OnLaserHit?.Invoke();
    }
} 