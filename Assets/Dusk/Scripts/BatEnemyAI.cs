using UnityEngine;
using System.Collections;
using static System.TimeZoneInfo;

public class BatEnemyAI : BaseEnemyAI
{
    [Header("Animation Settings")]
    [SerializeField] private Animator batAnimator;
    [SerializeField] private float ceilingTransitionDuration = 0.6f;

    private enum AnimationState { Flying, ToCeiling, OnCeiling, FromCeiling }
    private AnimationState currentAnimState;

    private float transitionTimer;
    private bool isTransitioning;

    public override void Move(Vector2 direction)
    {
        if (!isTransitioning)
        {
            var desiredState = GetDesiredAnimationState();
            if (desiredState != currentAnimState)
            {
                StartTransition(desiredState);
            }
        }
        else
        {
            transitionTimer -= Time.deltaTime;
            if (transitionTimer <= 0)
            {
                CompleteTransition();
            }
        }

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

    private AnimationState GetDesiredAnimationState()
    {
        if (isChasing) return AnimationState.Flying;

        float distanceToCeiling = Vector3.Distance(transform.position, guard.position);
        if (distanceToCeiling < 0.5f) return AnimationState.OnCeiling;

        return currentAnimState;
    }

    private void StartTransition(AnimationState newState)
    {
        isTransitioning = true;
        transitionTimer = ceilingTransitionDuration;

        switch (newState)
        {
            case AnimationState.OnCeiling:
                batAnimator.SetTrigger("ToCeiling");
                break;

            case AnimationState.Flying:
                batAnimator.SetTrigger("FromCeiling");
                break;
        }
    }

    private void CompleteTransition()
    {
        isTransitioning = false;

        switch (currentAnimState)
        {
            case AnimationState.Flying:
                currentAnimState = AnimationState.OnCeiling;
                break;

            case AnimationState.OnCeiling:
                currentAnimState = AnimationState.Flying;
                break;
        }

        UpdateAnimator(false);
    }

    private void UpdateAnimator(bool forceUpdate)
    {
        if (batAnimator == null) return;

        if (forceUpdate || !isTransitioning)
        {
            batAnimator.SetBool("IsFlying", currentAnimState == AnimationState.Flying);
            batAnimator.SetBool("IsOnCeiling", currentAnimState == AnimationState.OnCeiling);
        }
    }
}