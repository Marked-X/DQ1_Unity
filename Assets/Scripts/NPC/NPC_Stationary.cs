using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Stationary : MonoBehaviour
{
    [SerializeField]
    private Vector2 direction;
    private AnimatorController animatorController;

    private void Awake()
    {
        animatorController = GetComponent<AnimatorController>();
    }

    void Start()
    {
        animatorController.SetMovementAnimation(direction);
    }
}
