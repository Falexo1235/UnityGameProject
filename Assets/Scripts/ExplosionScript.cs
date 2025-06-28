using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public void Playexplosion(AnimationClip explosionClip, AudioClip sound = null)
    {
        float audioLength = 0f;
        float animationLength = 0f;
        if (sound != null)
        {
            audioLength = sound.length;
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.Play();
        }
        if (explosionClip != null)
        {
            animationLength = explosionClip.length;
            Animator animator = GetComponent<Animator>();
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
            overrideController["ExplosionAnimation"] = explosionClip;
            animator.runtimeAnimatorController = overrideController;
            Invoke(nameof(HideSprite), animationLength);


        }
        float duration = Mathf.Max(animationLength, audioLength);
        Destroy(gameObject, duration);
    }

    private void HideSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.enabled = false;
    }
}
