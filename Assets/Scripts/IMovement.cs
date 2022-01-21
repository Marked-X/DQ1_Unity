using UnityEngine;

public interface IMovement
{
    Vector2 facingDirection { get; }

    void OnMovement(Vector2 value);
    void SetCurrentMovement(Vector2 direction);
    void StopMovement();
}