using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara3DAnimCtrl : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    public KarpController3D chara;
    public InputHandler input;

    void Update()
    {
        animator.SetFloat("turn", input.StickDir.x);
        animator.SetFloat("speed", input.StickDir.magnitude);

        animator.SetBool("isSliding", input.isPlanning);
        animator.SetFloat("vertVelocity", rb.velocity.y);
    }
    public void CallJump()
    {
        animator.SetTrigger("Jump");
    }

    public void CallLanding()
    {
        animator.SetTrigger("Landing");
    }
}
