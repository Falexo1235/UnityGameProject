using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public void Playexplosion(AnimationClip explosionClip, AudioClip sound = null)
    {
        float audioLength = 0f;
        float animationLength = explosionClip.length;
        if (sound != null)
        {
            audioLength = sound.length;
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.Play();
        }
        float duration = Mathf.Max(animationLength, audioLength);
        Animator animator = GetComponent<Animator>();
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
        overrideController["ExplosionAnimation"] = explosionClip;
        animator.runtimeAnimatorController = overrideController;
        Invoke(nameof(HideSprite), animationLength);
        Destroy(gameObject, duration);
    }

    private void HideSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;
    }
}
