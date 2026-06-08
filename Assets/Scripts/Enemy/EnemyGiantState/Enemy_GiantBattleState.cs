using UnityEngine;

public class Enemy_GiantBattleState : Enemy_BattleState
{
    private Enemy_Giant enemyGiant;

    public Enemy_GiantBattleState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyGiant = enemy as Enemy_Giant;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemyGiant.maxBattleIdleTime;
    }

    public override void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemyGiant.giantTeleportState);

        if (enemy.PlayerDetected())
            UpdateTargetIfNeeded();

        if (WithinAttackRange() && enemy.PlayerDetected() && CanAttack())
        {
            lastTimeAttacked = Time.time;
            stateMachine.ChangeState(enemyGiant.giantAttackState);
        }
        else
        {
            float xVeloicty = enemy.canChasePlayer ? enemy.GetBattleMoveSpeed() : 0.0001f;

            if(enemy.groundDetected == false)
                xVeloicty = 0.00001f;

            enemy.SetVelocity(xVeloicty * DirectionToPlayer(), rb.velocity.y);
        }
    }
}
