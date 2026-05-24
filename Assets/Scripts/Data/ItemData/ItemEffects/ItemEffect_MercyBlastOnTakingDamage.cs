using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item effect/Mercy Blast", fileName = "Item effect data - Mercy blast on taking damage")]
public class ItemEffect_MercyBlastOnTakingDamage : ItemEffect_DataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float mercyDamage;
    [SerializeField] private LayerMask whatIsEnemy;

    [Space]
    [SerializeField] private float healthPercnetTrigger = .25f;
    [SerializeField] private float cooldown;
    private float lastTimeUsed = -999;
    [Header("vfx objects")]
    [SerializeField] private GameObject mercyBlastVfx;
    [SerializeField] private GameObject onHitVfx;

    public override void ExcuteEffect()
    {
        bool noCooldown = Time.time >= lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthPercent() <= healthPercnetTrigger;

        if (noCooldown && reachedThreshold)
        {
            player.vfX.CreateEffectOf(mercyBlastVfx, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemiesWithMercy();
        }
    }

    private void DamageEnemiesWithMercy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (var target in enemies)
        {
            IDamagable damagable = target.GetComponent<IDamagable>();

            if(damagable == null) continue;

            bool targetGotHit = damagable.TakeDamage(0, mercyDamage, ElementType.Mercy, player.transform);

            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();
            statusHandler?.ApplyStatusEffect(ElementType.Mercy, effectData);

            if (targetGotHit)
                player.vfX.CreateEffectOf(onHitVfx, target.transform);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTakingDamage += ExcuteEffect;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= ExcuteEffect;
        player = null;
    }
}
