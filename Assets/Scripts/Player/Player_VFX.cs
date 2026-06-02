using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    public void CreateEffectOf(GameObject effct, Transform target)
    {
        Instantiate(effct, target.position, Quaternion.identity);
    }
}
