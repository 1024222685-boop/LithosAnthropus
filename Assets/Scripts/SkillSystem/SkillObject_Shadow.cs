using System;
using UnityEngine;

public class SkillObject_Shadow : SkillObject_Base
{
    public event Action OnSlash;
    private Skill_Shadow shadowManager;

    [SerializeField] private GameObject skillPrefab;

    private Transform target;
    private float speed;

    private void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public void MoveTowardsClosestTarget(float speed)
    {
        target = FindClosestTarget();
        this.speed = speed;
    }

    public void SetupShadow(Skill_Shadow shadowManager)
    {
        this.shadowManager = shadowManager;

        playerStats = shadowManager.player.stats;
        damageScaleData = shadowManager.damageScaleData;

        float detonationTime = shadowManager.GetDetonateTime();

        Invoke(nameof(Slash), detonationTime);
    }

    public void SetupShadow(Skill_Shadow shadowManager, float detonationTime, bool canMove, float shadowSpeed)
    {
        this.shadowManager = shadowManager;

        playerStats = shadowManager.player.stats;
        damageScaleData = shadowManager.damageScaleData;

        Invoke(nameof(Slash), detonationTime);

        if (canMove)
            MoveTowardsClosestTarget(shadowSpeed);
    }

    public void Slash()
    {
        DamageEnemiesInRadius(transform, checkRadius);
        Instantiate(skillPrefab, transform.position, Quaternion.identity);

        OnSlash?.Invoke();
        Destroy(gameObject);
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null)
            return;

        Slash();
    }
}
