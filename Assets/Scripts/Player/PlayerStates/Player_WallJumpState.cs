using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        int inputDir = player.GetInputDir();

        player.SetVelocity(player.wallJumpForce.x * -inputDir, player.wallJumpForce.y);

        stateTimer = 0.15f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
            return;
        }

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.fallState);
    }
}