using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillObject_Stuntman : SkillObject_Base
{
    [SerializeField] private GameObject onDeathvfx;
    [SerializeField] private LayerMask whatIsGround;
    private Skill_Stuntman stuntManager;

    public void SetupStunt(Skill_Stuntman stuntManager)
    {
        this.stuntManager = stuntManager;

        Invoke(nameof(HandleDeath), stuntManager.GetStuntDuration());
    }

    private void Update()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
        StopHorizontalMovement();
    }

   public void HandleDeath()
    {
        Instantiate(onDeathvfx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if (hit.collider != null)
            rb.velocity = new Vector2(0, rb.velocity.y);
    }
}
