using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private Collider2D col;
    private bool isDestroyed = false;

    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string animBoolName)
        : base(enemy, stateMachine, animBoolName)
    {
        col = enemy.GetComponent<Collider2D>();
    }

    public override void Enter()
    {
        base.Enter();

        // 清零物理，防止坠机乱飞
        enemy.rb.velocity = Vector2.zero;
        enemy.rb.gravityScale = 0;

        // 关闭碰撞，防止死后被打
        if (col != null)
            col.enabled = false;

        // 关闭状态机，防止切换状态
        stateMachine.SwitchoffStateMachine();
    }

    public override void Update()
    {
        base.Update();

        // 死亡后强制保持不动
        enemy.rb.velocity = Vector2.zero;
    }

    public void CurrentStateAnimationTrigger()
    {
        if (isDestroyed) return; // 防止重复销毁
        isDestroyed = true;

        // 动画结束直接销毁敌人
        Object.Destroy(enemy.gameObject);
    }
}