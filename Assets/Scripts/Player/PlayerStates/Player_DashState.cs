using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DashState : PlayerState
{
    private float originalGravityScale;
    private int dashDir;

    public Player_DashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        skillManager.dash.OnStartEffect();
        player.vfX.DoImageEchoEffect(player.dashDuration);

        dashDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir;
        stateTimer = player.dashDuration;

        originalGravityScale = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        CancelDashfNeeded();
        player.SetVelocity(player.dashSpeed * player.facingDir,0);
      
        if(stateTimer < 0)
        {
            if(player.groundDetected)
            stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.fallState);
        }
    }

    public override void Exit() 
    {
        base.Exit();

        skillManager.dash.OnEndEffect();

        player.SetVelocity(0,0);
        rb.gravityScale = originalGravityScale;
    }

    private void CancelDashfNeeded()
    {
        if (player.wallDetected)
        {
            if (player.groundDetected)
                stateMachine.ChangeState(player.idleState);
        }
    }
}

