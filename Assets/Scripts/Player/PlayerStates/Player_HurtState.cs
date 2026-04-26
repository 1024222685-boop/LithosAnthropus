public class Player_HurtState : Player_GroundState
{
    public Player_HurtState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        // 进入状态时触发受击动画
        player.anim.SetBool("isHurt", true);
    }

    public override void Update()
    {
        base.Update();
        // 击退结束后，自动切回Idle
        if (!player.isKnocked)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // 退出状态时关闭受击动画
        player.anim.SetBool("isHurt", false);
    }
}
