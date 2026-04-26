using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamagable
{
    private Slider healthBar;
    private Entity entity;
    private Entity_VFX entityvfx;
    private Entity_Stats entityStats;

    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead;
    [Header("Health regen")]
    [SerializeField] private float regenInterval = 1;
    [SerializeField] private bool canRegenerateHealth = true;

    [Header("On Damage Knockback")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float knockbackDuration = .2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;
    [Header("On Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = .3f;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityvfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();

        currentHealth = entityStats.GetMaxHealth();
        UpdateHealthBar();

        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead)
            return false;

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} evaded the attack!");
            return false;
        }

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = entityStats.GetArmorMitigation(armorReduction);
        float physicalDamageTaken = damage * (1 - mitigation);

        float resistance = entityStats.GetElementalResistance(element);
        float elementalDamageTaken = elementalDamage * (1 - mitigation);

        TakeKnockback(damage, damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);

        return true;
    }

    private void TakeKnockback(float damage, Transform damageDealer, float fianlDamage)
    {
        Vector2 knockback = CalculateKnockback(damage, damageDealer);
        float duration = CalculateDuration(fianlDamage);

        entity?.ReciveKnockback(knockback, duration);
    }

    private bool AttackEvaded() => Random.Range(0, 100) < entityStats.GetEvasion();

    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
            return;

        float regenAmount = entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if (isDead)
            return;

        float newHealth = currentHealth + healAmount;
        float maxHealth = entityStats.GetMaxHealth();

        currentHealth = Mathf.Min(newHealth,maxHealth);
        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        entityvfx?.PlayOnDamagevfx();
        currentHealth = currentHealth - damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }

    public float GetHealthPercent() => currentHealth / entityStats.GetMaxHealth();

    public void SetHealthPercent(float percent)
    {
        currentHealth = entityStats.GetMaxHealth() * Mathf.Clamp01(percent);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
            return;

        healthBar.value = currentHealth / entityStats.GetMaxHealth();
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;

        knockback.x = knockback.x * direction;

        return knockback;
    }

    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damage) => damage / entityStats.GetMaxHealth() > heavyDamageThreshold;
}
