using UnityEngine;

public class DrillEffectScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip idleSound;
    public AudioClip drillingSound;
    public AudioClip hitSound;
    public ParticleSystem particles;
    public Collider2D drillZone;
    private bool isDrillActive = false;

    private bool HasDrillablesInZone()
    {
        Collider2D[] results = Physics2D.OverlapBoxAll(drillZone.bounds.center, drillZone.bounds.size, 0f);

        int drillableCount = 0;
        foreach (var col in results)
        {
            if (col != null && col.GetComponent<Undrillable>() == null)
            {
                drillableCount++;
            }
        }
        return drillableCount > 0;
    }

    private void Update()
    {
        if (!isDrillActive)
        {
            audioSource.Stop();
            particles.Stop();
            return;
        }

        bool hasDrillables = HasDrillablesInZone();

        if (hasDrillables)
        {
            if (audioSource.clip != drillingSound)
            {
                SetSound(drillingSound);
            }
            if (!particles.isPlaying)
            {
                particles.Play();
            }
        }
        else
        {
            if (audioSource.clip != idleSound)
            {
                SetSound(idleSound);
            }
            if (particles.isPlaying)
            {
                particles.Stop();
            }
        }
    }

    public void OnDrillStart()
    {
        isDrillActive = true;
        SetSound(idleSound);
    }
    public void OnDrillEnd()
    {
        isDrillActive = false;
        audioSource.Stop();
        particles.Stop();
    }

    public void OnDrillHit()
    {
        audioSource.PlayOneShot(hitSound);
    }

    private void SetSound(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}