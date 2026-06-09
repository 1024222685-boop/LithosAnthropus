public class Enemy_GiantTeleportState : EnemyState
{
    private Enemy_Giant enemyGiant;

    public Enemy_GiantTeleportState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyGiant = enemy as Enemy_Giant;
    }

    public override void Enter()
    {
        base.Enter();
        enemyGiant.MakeUntargetable(false);
    }

    public override void Update()
    {
        base.Update();

        if (enemyGiant.teleportTrigger)
        {
            enemyGiant.transform.position = enemyGiant.FindTeleportPoint();
            enemyGiant.SetTeleportTrigger(false);
        }

        if (triggerCalled)
        {
            if (enemyGiant.CanDoSpellCast())
                stateMachine.ChangeState(enemyGiant.giantSpellCastState);
            else
                stateMachine.ChangeState(enemyGiant.giantBattleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
        enemyGiant.MakeUntargetable(true);
    }
}
