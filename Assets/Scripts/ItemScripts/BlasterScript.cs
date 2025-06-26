using UnityEngine;

public class BlasterScript : MonoBehaviour, IUsableItem
{
    [Header("Shooting")]
    //Can be used as a base for new tools, like using missiles that follow cursor
    public GameObject projectilePrefab;  
    public Transform firePoint;
    
    [Header("Timing")]
    public float fireRate = 0.5f;
    private float nextFireTime;
    public void UseStart()
    {
        TryShoot();
    }

    public void UseHold()
    {
        TryShoot();
    }

    public void UseEnd()
    {
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime)
            return;
        nextFireTime = Time.time + fireRate;

        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
} 