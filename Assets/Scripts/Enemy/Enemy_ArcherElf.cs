using UnityEngine;


public class Enemy_ArcherElf : Enemy
{

    public bool CanBeCountered { get => canBeStunned; }
    public Enemy_ArcherElfBattleState elfBattleState { get; set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
        deadState = new Enemy_DeadState(this, stateMachine, "dead");

        elfBattleState = new Enemy_ArcherElfBattleState(this, stateMachine, "battle");
        battleState = elfBattleState;
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public void HandleCounter()
    {
        if (CanBeCountered == false)
            return;

        stateMachine.ChangeState(stunnedState);
    }

}
