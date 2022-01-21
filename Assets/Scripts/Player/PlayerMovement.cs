using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : Movement
{
    [SerializeField]
    private LayerMask ObstacleLayer;
    [SerializeField]
    private InputReader inputReader = default;

    public Vector2 FacingDirection { get; private set; }

    private void OnEnable()
    {
        inputReader.moveEvent += OnMovement;
        inputReader.interactEvent += OnInteract;
    }

    private void OnDisable()
    {
        inputReader.moveEvent -= OnMovement;
        inputReader.interactEvent -= OnInteract;
    }

    private void Update()
    {
        MovementUpdate();
    }

    public void OnInteract()
    {
        if (GameManager.Instance.CurrentState == GameManager.States.Freeroam)
        {
            RaycastHit2D hit = Physics2D.Raycast(RB.position, FacingDirection, 0.75f, LayerMask.GetMask("Interactable"));
            if (hit)
            {
                hit.collider.gameObject.GetComponent<Interactable>().Activate();
            }
        }
    }

    #region PlayerMovement

    private void OnMovement(Vector2 value)
    {
        if (value.x != 0) value.y = 0;

        CurrentMovement = value * 0.5f;

    }

    private void MovementUpdate()
    {
        if (!IsMoving && CurrentMovement != Vector2.zero)
        {
            NextMovement = CurrentMovement;
            CurrentMovement = MovementCheck(CurrentMovement);

            if (CurrentMovement != Vector2.zero)
            {
                FacingDirection = CurrentMovement;
                AnimatorController.SetMovementAnimation(FacingDirection);

                if (NextMovement == CurrentMovement)
                    StartCoroutine(Move(RB.position + CurrentMovement));
                if (NextMovement != CurrentMovement)
                {
                    StartCoroutine(Move(RB.position + CurrentMovement));
                    CurrentMovement = NextMovement;
                }
            }
            else
            {
                FacingDirection = NextMovement;
                AnimatorController.SetMovementAnimation(FacingDirection);
            }
        }
    }

    private Vector2 MovementCheck(Vector2 movement)
    {
        Vector2 firstPos;
        Vector2 secondPos;

        if (movement.x != 0)
        {
            firstPos = new Vector2(0, 0.25f);
            secondPos = new Vector2(0, -0.25f);
        }
        else
        {
            firstPos = new Vector2(0.25f, 0);
            secondPos = new Vector2(-0.25f, 0);
        }

        RaycastHit2D firstHit = Physics2D.Raycast(RB.position + firstPos, movement, 0.75f, ObstacleLayer);
        RaycastHit2D secondHit = Physics2D.Raycast(RB.position + secondPos, movement, 0.75f, ObstacleLayer);

        if(firstHit.collider != null && secondHit.collider != null)
        {
            return Vector2.zero;
        } 
        else if(firstHit.collider != null && secondHit.collider == null)
        {
            return secondPos * 2f;
        }
        else if(firstHit.collider == null && secondHit.collider != null)
        {
            return firstPos * 2f;
        }
        return movement;
    }
    #endregion
}
