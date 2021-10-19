using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara3DAnimCtrl : MonoBehaviour
{
    public Animator animator;
    public CharaController3D chara;
    public Rigidbody rb;

    void Update()
    {
        animator.SetBool("isGrounded", chara.isGrounded);
        animator.SetFloat("velY", rb.velocity.y);
        animator.SetFloat("turn", Input.GetAxis("Horizontal"));
        animator.SetFloat("speed", Input.GetAxis("Vertical"));
    }
    public void Jump()
    {
        animator.SetTrigger("Jump");
    }
}
