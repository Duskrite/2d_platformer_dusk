using Pathfinding;
using UnityEditor.Tilemaps;
using UnityEngine;

public class FlyingEnemyAI : BaseEnemyAI
{
    public override void Move(Vector2 direction)
    {
        rb.linearVelocity = direction * speed * Time.deltaTime;

        Flip(direction.x);
    }

    public void Flip(float directionX)
    {
        if (directionX > 0.1 && !isFacingRight || directionX < -0.1 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1;
            transform.localScale = ls;
        }
    }
}
