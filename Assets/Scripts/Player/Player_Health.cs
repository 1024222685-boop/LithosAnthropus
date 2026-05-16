using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Health : Entity_Health
{
    protected override void Die()
    {
        base.Die();

        //GameManager.instance.SetLastPlayerPosition(transform.position);
        GameManager.instance.RestartScene();
    }
}
