using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EntityState currentState { get; private set; }//当前正在运行的状态（外部可读，内部可写）
    public bool canChangeState = true;//标记是否允许切换状态（用于临时锁定状态，如受击，硬直时）

    public void Initialize(EntityState starState)//设置初始状态，startState-状态机启动时要进入的第一个状态
    {
        canChangeState = true;//重置状态切换权限为允许
        currentState = starState;//将当前状态设置为初始状态
        currentState.Enter();//调用当前状态的Enter方法，执行进入逻辑（像播放动画，设置参数这样）
    }

    public void ChangeState(EntityState newState)//切换新状态，退出当前状态并进入新状态；newState-要切换到的目标状态
    {
        if (canChangeState == false)//如果当前不允许切换状态，直接返回
            return;

        currentState.Exit();//调用当前状态的Exit方法，执行退出逻辑
        currentState = newState;//将当前状态更新为新状态
        currentState.Enter();//调用新状态的Enter方法，执行进入逻辑
    }

    public void UpdateActiveState()//更新当前活跃状态：每帧调用，执行当前状态的逻辑更新
    {
        currentState.Update();//调用当前状态的Update方法，执行该状态下的每帧逻辑（像移动，检测输入这样的）
    }

    public void SwitchoffStateMachine() => canChangeState = false;//关闭状态机：临时禁止状态切换（用于打断状态流，比如像播放过场动画的时候不能让角色或者敌人突然出现动作，或者说进入下一个状态的时候上一个状态没取消会显得很诡异）
}
