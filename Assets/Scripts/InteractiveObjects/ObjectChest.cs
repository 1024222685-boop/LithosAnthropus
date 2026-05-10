using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectChest : MonoBehaviour, IDamagable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
     private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponent<Entity_VFX>();
    private Entity_DropManager dropManager => GetComponent<Entity_DropManager>();

    [Header("Open Details")]
    [SerializeField] private Vector2 knockback;
    [SerializeField] private bool canDropItems = true;

    public bool TakeDamage(float damage,float elementalDamage, ElementType element, Transform damageDealer)
    {
        if(canDropItems == false)
            return false;

        canDropItems = false;
        dropManager?.DropItems();
        fx.PlayOnDamagevfx();
        anim.SetBool("chestOpen", true);
        rb.velocity = knockback;
        rb.angularVelocity = Random.Range(-200, 200f);

        return true;
    }
}
