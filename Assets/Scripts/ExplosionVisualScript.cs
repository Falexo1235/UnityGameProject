using UnityEngine;

public class ExplosionVisualScript : MonoBehaviour
{
    public void Playexplosion(AnimationClip explosionClip)
    {
        Debug.Log(explosionClip);
        float animationLength = explosionClip.length;
        Animator animator = GetComponent<Animator>();
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        overrideController.runtimeAnimatorController = animator.runtimeAnimatorController;
        overrideController["ExplosionAnimation"] = explosionClip;
        animator.runtimeAnimatorController = overrideController;
        Destroy(gameObject, animationLength);
    }
}
