using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TsDeadState : Enemy_DeadState
{
    private Enemy_TreeSprite enemyTs;
    private bool hasSplited = false;

    public Enemy_TsDeadState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        enemyTs = enemy as Enemy_TreeSprite;
    }

    public override void Enter()
    {
        base.Enter();
        enemyTs.CreateTsOnDeath();
    }
}
