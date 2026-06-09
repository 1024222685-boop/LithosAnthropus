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

        // 进入战斗时显示并初始化BOSS血条
        UI_InGame.Instance.ShowBossHealthBar();
        UI_InGame.Instance.UpdateBossHealthBar(enemyGiant.health.GetCurrentHealth(), enemyGiant.stats.GetMaxHealth());
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

            if (enemy.groundDetected == false)
                xVeloicty = 0.00001f;

            enemy.SetVelocity(xVeloicty * DirectionToPlayer(), rb.velocity.y);
        }
    }

    public override void Exit()
    {
        base.Exit();

        // UI_InGame.Instance.HideBossHealthBar();
    }
}