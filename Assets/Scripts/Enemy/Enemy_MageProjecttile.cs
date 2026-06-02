using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MageProjecttile : MonoBehaviour
{
    private Entity_Combat combat;
    private Rigidbody2D rb;
    private Collider2D col;
    private Animator anim;
    private Transform target;

    [SerializeField] private float arcHeight = 2f;
    [SerializeField] private LayerMask whatCanColliderWith;

    [Header("Tracking & Rotation Settings")]
    [Tooltip("Enable homing tracking for projectile")]
    [SerializeField] private bool enableTracking = true;
    [Tooltip("0 = no tracking (pure parabola), 1 = full tracking (straight line)")]
    [Range(0f, 1f)]
    [SerializeField] private float trackingStrength = 0.4f; // 提高默认追踪强度
    [Tooltip("Rotate projectile to face movement direction")]
    [SerializeField] private bool enableRotation = true;
    [Tooltip("Force projectile to always face target on spawn")]
    [SerializeField] private bool forceTargetDirection = true; // 新增：强制初始方向朝向目标

    [Header("Debug Settings")]
    [SerializeField] private bool showDebugLogs = false;

    public void SetupProjecttile(Transform target, Entity_Combat combat)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();

        this.combat = combat;
        this.target = target;

        // 核心修复：先计算正确的初始弹道
        Vector2 velocity = CalculateBallisticVelocity(transform.position, target.position);

        // 新增：强制修正方向，确保永远朝向目标
        if (forceTargetDirection)
        {
            float directionSign = Mathf.Sign(target.position.x - transform.position.x);
            velocity.x = Mathf.Abs(velocity.x) * directionSign;
        }

        rb.velocity = velocity;

        // 调试日志
        if (showDebugLogs)
        {
            Debug.Log($"Projectile spawned: Target X={target.position.x}, Start X={transform.position.x}");
            Debug.Log($"Direction sign: {Mathf.Sign(target.position.x - transform.position.x)}, Velocity X={velocity.x}");
        }
    }

    private void FixedUpdate()
    {
        if (target == null || rb.velocity == Vector2.zero)
            return;

        if (enableTracking)
        {
            Vector2 desiredDirection = (target.position - transform.position).normalized;
            Vector2 newVelocity = Vector2.Lerp(
                rb.velocity.normalized,
                desiredDirection,
                trackingStrength
            ) * rb.velocity.magnitude;

            rb.velocity = newVelocity;
        }

        if (enableRotation)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatCanColliderWith) != 0)
        {
            combat.PerformAttackOnTarget(collision.transform);

            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            col.enabled = false;
            target = null;

            if (anim != null)
            {
                anim.SetTrigger("Explode");
            }

            Destroy(gameObject, 2f);
        }
    }

    private Vector2 CalculateBallisticVelocity(Vector2 start, Vector2 end)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale;

        float displacementY = end.y - start.y;
        float displacementX = end.x - start.x;

        float peakHeight = Mathf.Max(arcHeight, end.y - start.y + .1f);

        float timeToApex = Mathf.Sqrt(2 * peakHeight / gravity);
        float timeFromApex = Mathf.Sqrt(2 * (peakHeight - displacementY) / gravity);
        float totalTime = timeToApex + timeFromApex;

        float velocityY = Mathf.Sqrt(2 * gravity * peakHeight);
        float velocityX = displacementX / totalTime;

        return new Vector2(velocityX, velocityY);
    }
}