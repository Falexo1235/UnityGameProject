using UnityEngine;

public class DrillScript : MonoBehaviour, IUsableItem
{
    [Header("Drill Settings")]
    public float drillSpeed = 1f;
    public Collider2D drillTrigger;
    public Animator drillAnimator;
    public DrillEffectScript drillEffect;
    private float nextDrillTime;

    void Start()
    {
        drillAnimator.speed = 0;
    }
    public void UseStart()
    {
        nextDrillTime = Time.time + (1f / drillSpeed);
        drillAnimator.speed = 2;
        drillEffect.OnDrillStart();
    }

    public void UseHold()
    {
        TryDrill();
    }

    public void UseEnd()
    {
        drillAnimator.speed = 0;
        drillEffect.OnDrillEnd();
    }

    private void TryDrill()
    {
        if (Time.time < nextDrillTime)
            return;

        nextDrillTime = Time.time + (1f / drillSpeed);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(drillTrigger.bounds.center, drillTrigger.bounds.size, 0f);
        
        foreach (Collider2D col in colliders)
        {
            if (col != null && col.GetComponent<Drillable>() != null)
            {
                Destroy(col.gameObject);
                drillEffect.OnDrillHit();
            }
        }
    }
} 