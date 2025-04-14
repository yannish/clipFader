using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WrappedAnimator : MonoBehaviour
{
    private Animator animator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = null;
    }

    void Update()
    {
        
    }
}
