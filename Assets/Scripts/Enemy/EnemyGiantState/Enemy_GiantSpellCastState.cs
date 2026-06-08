public class Enemy_GiantSpellCastState : EnemyState
{
    private Enemy_Giant enemyGiant;

    public Enemy_GiantSpellCastState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyGiant = enemy as Enemy_Giant;
    }

    public override void Enter()
    {
        base.Enter();

        enemyGiant.SetVelocity(0, 0);
        enemyGiant.SetSpellCastPerformed(false);
        enemyGiant.SetSpellCastOnCooldown();
    }

    public override void Update()
    {
        base.Update();

        if (enemyGiant.spellCastPerformed)
            anim.SetBool("spellCast_performed", true);

        if (triggerCalled)
        {
            if (enemyGiant.ShouldTeleport())
                stateMachine.ChangeState(enemyGiant.giantTeleportState);
            else
                stateMachine.ChangeState(enemyGiant.giantBattleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        anim.SetBool("spellCast_performed", false);
    }
}
