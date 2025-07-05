using UnityEngine;
using System.Collections;

public class AudioChangeScript : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource[] audioSourcesToDisable;
    public AudioSource audioSourceToChange;
    public AudioClip newAudioClip;
    
    [Header("Fade Settings")]
    public float fadeOutTime = 1f;
    public float fadeInTime = 1f;
    
    private bool hasTriggered = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerTag player = other.GetComponent<PlayerTag>();
        if (player != null && !hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(SmoothAudioChange());
        }
    }
    
    private IEnumerator SmoothAudioChange()
    {
        if (audioSourcesToDisable != null)
        {
            foreach (AudioSource audioSource in audioSourcesToDisable)
            {
                if (audioSource != null && audioSource.isPlaying)
                {
                    StartCoroutine(FadeOutAudio(audioSource));
                }
            }
        }
        yield return new WaitForSeconds(fadeOutTime);

        if (audioSourcesToDisable != null)
        {
            foreach (AudioSource audioSource in audioSourcesToDisable)
            {
                if (audioSource != null)
                {
                    audioSource.Stop();
                    audioSource.enabled = false;
                }
            }
        }
        if (audioSourceToChange != null && newAudioClip != null)
        {
            audioSourceToChange.clip = newAudioClip;
            StartCoroutine(FadeInAudio(audioSourceToChange));
        }
        Destroy(this);
    }
    
    private IEnumerator FadeOutAudio(AudioSource audioSource)
    {
        float startVolume = audioSource.volume;
        float timer = 0f;
        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeOutTime);
            yield return null;
        }
        audioSource.volume = 0f;
    }
    
    private IEnumerator FadeInAudio(AudioSource audioSource)
    {
        audioSource.volume = 0f;
        audioSource.Play();
        float timer = 0f;
        while (timer < fadeInTime)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / fadeInTime);
            yield return null;
        }
        
        audioSource.volume = 1f;
    }
    public void ManualTrigger()
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            StartCoroutine(SmoothAudioChange());
        }
    }
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
