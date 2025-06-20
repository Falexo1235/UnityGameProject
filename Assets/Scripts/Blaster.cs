using UnityEngine;

public class Blaster : MonoBehaviour, IUsableItem
{
    [Header("Shooting")]
    public float projectileSpeed = 20f;
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

        Vector2 fireDirection = firePoint.right;

        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        
        if (projectile != null)
        {
            projectile.speed = projectileSpeed;
            projectile.Initialize(fireDirection);
        }
    }
} 