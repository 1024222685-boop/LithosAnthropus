using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_BlackSmith : Object_NPC,IInteractable
{
    private Animator anim;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isBlacksmith", true);
    }

    public void Interact()
    {
        Debug.Log("Open craft or storage");
    }

}
