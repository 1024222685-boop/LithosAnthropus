public class Enemy_GiantAttackState : EnemyState
{
    private Enemy_Giant enemyGiant;

    public Enemy_GiantAttackState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyGiant = enemy as Enemy_Giant;
    }

    public override void Enter()
    {
        base.Enter();
        SyncAttackSpeed();
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            if (enemyGiant.ShouldTeleport())
                stateMachine.ChangeState(enemyGiant.giantTeleportState);
            else
                stateMachine.ChangeState(enemyGiant.giantBattleState);
        }
    }
}
