using UnityEngine;

public class Character : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimation(CharacterAnimations animations)
    {
        animator.SetTrigger(animations.ToString());
    }
}
