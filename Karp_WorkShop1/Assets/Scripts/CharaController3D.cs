using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharaController3D : MonoBehaviour
{
    private const float defaultGravity = 9.81f;

    [Header("Input")]
    public KeyCode upKey;
    public KeyCode forwardKey, backwardKey;
    public KeyCode rightKey, leftKey;

    [Header("Parameters")]
    public float horiSpeed = 10;
    public float jumpHeight = 10;
    public AnimationCurve groundAcc;
    public AnimationCurve jumpCurve;
    public int jumpNbr = 2;
    [Range(0, 10)] public float jumpReleaseAcc = 10;
    [Range(-1f, 1f)] public float accthreshold = 0.3f;
    public float rotSpeed = 100;
    public UnityEvent jump;

    [Header("Ref")]
    [SerializeField] Transform self;
    [SerializeField] Rigidbody selfBody;
    [SerializeField] Transform camTransform;
    [SerializeField] CharaRaycaster2D rayCaster;

    [Header("Info")]
    public Vector3 currentMove = Vector2.zero;
    [HideInInspector] public Vector3 lastDir = Vector2.zero;
    public float horiTimeStamp = 0f;
    public float rotation = 0f;
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
        rotation = 0;

        HorizontalMove();
        VerticalMove();
    }

    private void HorizontalMove()
    {
        // calculer forward
        Vector3 forward = Vector3.Scale((self.position - camTransform.position), new Vector3(1, 0, 1)).normalized;

        //Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x).normalized;

        Debug.DrawRay(self.position, forward * 5, Color.blue);
        Debug.DrawRay(self.position, right * 5, Color.red);

        //Process Input
        if (Input.GetKey(leftKey))
        {
            rotation--;
            currentMove -= right;

        }

        if (Input.GetKey(rightKey))
        {
            rotation++;
            currentMove += right;

        }

        self.Rotate(Vector3.up * rotSpeed * rotation * Time.deltaTime);

        if (Input.GetKey(forwardKey))
        {
            currentMove += forward;
        }

        if (Input.GetKey(backwardKey))
        {
            currentMove -= forward;
        }

        if(currentMove == Vector3.zero)
        {
            horiTimeStamp = Time.time;
            return;
        }

        currentMove = currentMove.normalized;
        Debug.DrawRay(self.position, currentMove * 5, Color.green);

        //Process Acc
        if (/*currentMove != lastDir || */ Vector3.Dot(currentMove, lastDir) < accthreshold)
        {
            horiTimeStamp = Time.time;
        }
        float accStep = Time.time - horiTimeStamp;
        float curAcc = groundAcc.Evaluate(accStep);

        //Translation
        hMove = currentMove.x * curAcc * horiSpeed;
        self.Translate(currentMove * curAcc * horiSpeed * Time.deltaTime, Space.World);

        //Save last fr value
        lastDir = currentMove;
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.position.y < self.transform.position.y)
        {
            isGrounded = true;
        }
    }

    /*
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
            curJumpDur += Time.deltaTime * (Input.GetKey(upKey) ? 1 : jumpReleaseAcc);
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


    }*/
    private void VerticalMove()
    {
        if (Input.GetKeyDown(upKey))
        {
            selfBody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            isGrounded = false;
            jump?.Invoke();
        }
    }


}
