using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    public override void Update()
    {
        base.Update();
        // Check if the player object is null

        if (IsPlayerInLineOfSight())
        {
            MoveTowardsPlayer();
            animator.Play("Run");
        }
        else
        {
            // Wander if player is not in line of sight
            Wander();
        }

    }
}
