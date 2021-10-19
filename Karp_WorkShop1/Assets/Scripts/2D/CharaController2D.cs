using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController2D : MonoBehaviour
{
    private const float defaultGravity = 9.81f;

    [Header("Input")]
    public KeyCode upKey;
    public KeyCode rightKey, leftKey;

    [Header("Parameters")]
    public float horiSpeed = 10;
    public float jumpHeight = 10;
    public AnimationCurve groundAcc;
    public AnimationCurve jumpCurve;
    public int jumpNbr = 2;
    [Range(0,10)] public float jumpReleaseAcc = 10;

    [Header("Ref")]
    [SerializeField] Transform self;
    [SerializeField] Transform graph;
    [SerializeField] CharaRaycaster2D rayCaster;

    [Header("Info")]
    [HideInInspector] public Vector2 currentMove = Vector2.zero;
    [HideInInspector] public Vector2 lastDir = Vector2.zero;
    [HideInInspector] public float horiTimeStamp = 0f;
    [HideInInspector] public float curJumpDur = 0f;
    public bool isGrounded = false;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public int remainingJump;
    [HideInInspector] public float hMove;
    [HideInInspector] public float vMove;

    void Start()
    {
        isGrounded = false;
        isJumping = false;
        remainingJump = jumpNbr;
    }

    void Update()
    {
        currentMove = Vector2.zero;

        HorizontalMove();
        VerticalMove();
    }

    private void HorizontalMove()
    {
        //Process Input
        if (Input.GetKey(leftKey))
        {
            currentMove.x--;
            graph.localScale = new Vector3(-Mathf.Abs(graph.localScale.x), graph.localScale.y, graph.localScale.z);
        }

        if (Input.GetKey(rightKey))
        {
            currentMove.x++;
            graph.localScale = new Vector3(Mathf.Abs(graph.localScale.x), graph.localScale.y, graph.localScale.z);
        }

        //Process Acc
        if (currentMove.x != lastDir.x)
        {
            horiTimeStamp = Time.time;
        }
        float accStep = Time.time - horiTimeStamp;
        float curAcc = groundAcc.Evaluate(accStep);

        //Translation
        hMove = currentMove.x * curAcc * horiSpeed;
        self.Translate(currentMove * curAcc * horiSpeed * Time.deltaTime);

        //Save last fr value
        lastDir = currentMove;
    }

    private void VerticalMove()
    {
        if (Input.GetKeyDown(upKey))
        {
            if (remainingJump > 0)
            {
                remainingJump--;
                isJumping = true;
                isGrounded = false;
                curJumpDur = 0f;
            }
        }

        if (isJumping)
        {
            curJumpDur +=  Time.deltaTime * (Input.GetKey(upKey)? 1 : jumpReleaseAcc);
            currentMove.y = defaultGravity * jumpCurve.Evaluate(curJumpDur);
            
            if (curJumpDur > jumpCurve.keys[jumpCurve.keys.Length - 1].time)
            {
                isJumping = false;
            }
        }
        else
        {
            currentMove.y = defaultGravity * -1;
        }

        bool moveUp = currentMove.y > 0;
        if (rayCaster.ThrowRay(moveUp ? RayDir.Up : RayDir.Down))
        {
            if (!moveUp)
            {
                isGrounded = true;
                isJumping = false;
                remainingJump = jumpNbr;
            }
            vMove = 0;
        }
        else
        {
            if (!moveUp)
            {
                isGrounded = false;
            }
            vMove = currentMove.y;
            self?.Translate(Vector2.up * currentMove.y * Time.deltaTime);

        }


    }
}
