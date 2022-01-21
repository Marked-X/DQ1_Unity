using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC_RandMovement : Movement
{
    [SerializeField]
    private LayerMask ObstacleLayer;
    [SerializeField]
    private Transform leftNode;
    [SerializeField]
    private Transform rightNode;
    [SerializeField]
    private Transform topNode;
    [SerializeField]
    private Transform bottomNode;

    private Dictionary<Vector2, bool> avaliableDirections;
    private int avaliableDirectionsCount;

    private float moveTimer = 4f;
    private float currentMoveTime = 0f;

    private void Start()
    {
        avaliableDirections = new Dictionary<Vector2, bool>();
        avaliableDirections.Add(Vector2.up, false);
        avaliableDirections.Add(Vector2.down, false);
        avaliableDirections.Add(Vector2.left, false);
        avaliableDirections.Add(Vector2.right, false);
    }

    private void Update()
    {
        if (!IsMoving)
        {
            currentMoveTime += Time.fixedDeltaTime;
        }
        if(currentMoveTime >= moveTimer)
        {
            ChooseDirection();
            currentMoveTime = 0f;
        }
    }

    private void ChooseDirection()
    {
        CheckValiableDirections();
        CurrentMovement = Vector2.zero;

        if(avaliableDirectionsCount > 0)
        {
            int rand = UnityEngine.Random.Range(1, avaliableDirectionsCount + 1);

            foreach(KeyValuePair<Vector2, bool> direction in avaliableDirections)
            {
                if (direction.Value)
                {
                    rand--;
                    if (rand == 0)
                    {
                        CurrentMovement = direction.Key;
                        AnimatorController.SetMovementAnimation(CurrentMovement);
                    }
                }
            }

            StartCoroutine(Move(RB.position + CurrentMovement * 0.5f));
        }
    }

    private void CheckValiableDirections()
    {
        avaliableDirectionsCount = 0;
        for(int i = 0; i < avaliableDirections.Count; i++)
        {
            Vector2 temp = avaliableDirections.ElementAt(i).Key;
            avaliableDirections[temp] = CollisionCheck(temp);
            if (avaliableDirections[temp])
            {
                avaliableDirectionsCount++;
            }
        }
    }

    private bool CollisionCheck(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(RB.position, direction, 0.75f, ObstacleLayer);
        Vector2 nextPosition = RB.position + direction;
        if (hit.collider == null)
        {
            if (nextPosition.x < rightNode.position.x && nextPosition.x > leftNode.position.x && nextPosition.y < topNode.position.y && nextPosition.y > bottomNode.position.y)
            {
                return true;
            }
            Debug.Log("Tried going " + direction);
        }
        return false;
    }
}
