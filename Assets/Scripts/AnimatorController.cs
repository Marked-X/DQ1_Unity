using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMovementAnimation(Vector2 direction)
    {
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Horizontal", direction.x);
    }
}
