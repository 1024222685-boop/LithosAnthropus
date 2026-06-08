using UnityEngine;

public class Enemy_GiantAnimationTriggers : Enemy_AnimationTriggers
{
    private Enemy_Giant enemyGiant;

    protected override void Awake()
    {
        base.Awake();
        enemyGiant = GetComponentInParent<Enemy_Giant>();
    }

    private void TeleportTrigger()
    {
        enemyGiant.SetTeleportTrigger(true);
    }
}
