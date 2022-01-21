using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public Rigidbody2D RB { get; protected set; }
    public AnimatorController AnimatorController { get; protected set; }

    public bool IsMoving { get; protected set; } = false;
    public Vector2 CurrentMovement { get; protected set; } = Vector2.zero;
    public Vector2 NextMovement { get; protected set; } = Vector2.zero;

    public virtual void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        AnimatorController = GetComponent<AnimatorController>();
    }

    public void SetCurrentMovement(Vector2 direction)
    {
        CurrentMovement = direction;
    }

    public void StopMovement()
    {
        CurrentMovement = Vector2.zero;
    }

    public IEnumerator Move(Vector2 targetPos)
    {
        IsMoving = true;

        while ((targetPos - RB.position).sqrMagnitude > Mathf.Epsilon)
        {
            RB.position = Vector2.MoveTowards(RB.position, targetPos, 4f * Time.deltaTime);
            yield return null;
        }
        RB.position = targetPos;

        IsMoving = false;
    }
}
