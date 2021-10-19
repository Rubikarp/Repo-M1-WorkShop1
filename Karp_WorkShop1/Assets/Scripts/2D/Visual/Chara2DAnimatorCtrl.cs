using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chara2DAnimatorCtrl : MonoBehaviour
{
    public Animator animator;
    public CharaController2D chara;

    void Update()
    {
        animator.SetFloat("hAbsSpeed", Mathf.Abs(chara.hMove));
        animator.SetFloat("vSpeed", chara.vMove);
        animator.SetBool("isGrounded", chara.isGrounded);

        if (Input.GetKeyDown(chara.upKey) && chara.remainingJump > 0)
        {
            animator.SetTrigger("jump");
        }

        //TODO
        //trigger fall
    }
}
