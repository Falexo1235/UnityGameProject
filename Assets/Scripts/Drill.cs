using UnityEngine;

public class Drill : MonoBehaviour, IUsableItem
{
    [Header("Drill Settings")]
    public float drillSpeed = 1f;
    public Collider2D drillTrigger;
    
    private bool isDrilling = false;
    private float nextDrillTime;

    public void UseStart()
    {
    }

    public void UseHold()
    {
        if (!isDrilling)
        {
            StartDrilling();
        }
        TryDrill();
    }

    public void UseEnd()
    {
        StopDrilling();
    }

    private void StartDrilling()
    {
        isDrilling = true;
        nextDrillTime = Time.time;
    }

    private void StopDrilling()
    {
        isDrilling = false;
    }

    private void TryDrill()
    {
        //Add particle generation before that check, they need to appear when any collision is triggered
        //Also needs something to prevent the drill from triggering itself with a collider
        if (Time.time < nextDrillTime)
            return;

        nextDrillTime = Time.time + (1f / drillSpeed);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(drillTrigger.bounds.center, drillTrigger.bounds.size, 0f);
        
        foreach (Collider2D col in colliders)
        {
            if (col != null && col.CompareTag("Drillable"))
            {
                Destroy(col.gameObject);
            }
        }
    }
} 