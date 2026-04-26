using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillObject_Sickle : SkillObject_Base
{
    protected Skill_SickleThrow sickleManager;
    protected Rigidbody2D rb;

    protected Transform playerTransform;
    protected bool shouldComeback;
    protected float comebackSpeed = 20;
    protected float maxAllowedDistance = 25;

    private float ScaleSign;
    private Quaternion LocalRot;

    private void Update()
    {
        if (rb != null && rb.simulated && rb.velocity != Vector2.zero)
        {
            float dir = Mathf.Sign(rb.velocity.x);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);
            transform.right = dir * rb.velocity;
        }

        HandleComeback();
    }
    private void LateUpdate()
    {
        if (transform.parent != null)
        {
            float parentDir = Mathf.Sign(transform.parent.localScale.x);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * parentDir * ScaleSign, transform.localScale.y, transform.localScale.z);
            transform.localRotation = LocalRot;
        }
    }

    public virtual void SetupSickle(Skill_SickleThrow sickleManager, Vector2 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction;

        this.sickleManager = sickleManager;

        playerTransform = sickleManager.transform.root;
        playerStats = sickleManager.player.stats;
        damageScaleData = sickleManager.damageScaleData;
    }

    public void GetSickleBackToPlayer() => shouldComeback = true;

    protected void HandleComeback()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance > maxAllowedDistance)
            GetSickleBackToPlayer();

        if (shouldComeback == false)
            return;

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, comebackSpeed * Time.deltaTime);

        if (distance < .5f)
            Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        StopSickle(collision);
        DamageEnemiesInRadius(transform, 1);
    }

    protected void StopSickle(Collider2D collision)
    {
        rb.simulated = false;
        transform.parent = collision.transform;

        ScaleSign = Mathf.Sign(transform.localScale.x);
        LocalRot = transform.localRotation;
    }
}