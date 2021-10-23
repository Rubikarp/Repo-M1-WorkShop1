using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KarpController3D : MonoBehaviour
{
    private const float defaultGravity = 9.81f;

    [Header("References")]
    [SerializeField] Transform self;
    [SerializeField] Rigidbody selfBody;
    [Space(5)]
    [SerializeField] Transform camTransform;
    [Space(5)]
    [SerializeField] InputHandler input;
    [SerializeField] CharaRaycaster3D rayCaster;

    [Header("Parameters")]
    public float runSpeed = 10;
    public AnimationCurve accCurve;
    [Space(5)]
    public int jumpCounter = 1;
    public float jumpForce = 10;
    public float jumpSecondaryForce = 1;
    public float jumpCooldown = 0.4f;
    [Space(5)]
    public float rotateSpeed = 0.3f;
    
    [Header("Debug")]
    public bool drawDebug = false;

    [Header("Stats Movement")]
    public Vector2 moveDir = Vector2.zero;
    public Vector3 currentMove = Vector2.zero;
    [HideInInspector] public Vector3 lastDir = Vector2.zero;
    public float accTime = 0f;
    [Range(-1,1)] public float accThreshold = 0.3f;
    
    [Space(10)]
    public bool isGrounded = false;
    public bool canJump = true;
    public int remainingJump;
    [Space(10)]
    public UnityEvent touchFloor;

    void Start()
    {
        canJump = true;
        isGrounded = false;
        remainingJump = jumpCounter;

        touchFloor.AddListener(OnTouchFloor);
    }

    void Update()
    {
        currentMove = Vector2.zero;

        HorizontalMove();
        AirPlanning();
    }

    private void HorizontalMove()
    {
        // calculer forward = cam2Avatar projetté sur le plan XZ
        Vector3 forward = Vector3.Scale((self.position - camTransform.position), new Vector3(1, 0, 1)).normalized;
        //Calcul Right = orthogonal de forward et vec3.up
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

        if (drawDebug)
        {
            Debug.DrawRay(self.position, forward * 5, Color.red);
            Debug.DrawRay(self.position, right * 5, Color.blue);
        }

        //Process Input
        Vector2 StickDir = input.StickDir.normalized;
        currentMove = (StickDir.x * right) + (StickDir.y * forward);

        //ROTATION
        self.rotation = Quaternion.Slerp(self.rotation, Quaternion.LookRotation(currentMove == Vector3.zero? self.forward : currentMove, Vector3.up), rotateSpeed * Time.deltaTime);

        //MOVE

        if (currentMove == Vector3.zero)
        {
            accTime = 0f;
            return;
        }
        else
        {
            currentMove = currentMove.normalized;
        }

        if (drawDebug)
        {
            Debug.DrawRay(self.position, currentMove * 5, Color.green);
        }


        //Process Acc
        if (Vector3.Dot(currentMove, lastDir) > accThreshold)
        {
            accTime = Time.time;
        }
        accTime += Time.deltaTime;
        float currentAcc = accCurve.Evaluate(accTime);

        //Translation
        self.Translate(currentMove * currentAcc * runSpeed * Time.deltaTime, Space.World);
        
        //Save last fr value
        lastDir = currentMove;
    }
    private void AirPlanning()
    {
        if (input.isPlanning)
        {
            selfBody.velocity += -Physics.gravity * 0.5f * Time.deltaTime;
        }

        if (rayCaster.CastCapsule(Ray3Dir.Down) && canJump)
        {
            touchFloor?.Invoke();
            canJump = false;
            Invoke("JumpCD", jumpCooldown);
        }
    }

    private void OnTouchFloor()
    {
        isGrounded = true;
        remainingJump = jumpCounter;
    }

    public void OnJump()
    {
        if(remainingJump > 0 && canJump)
        {
            selfBody.AddForce(Vector3.up  * jumpForce, ForceMode.Impulse);
            remainingJump--;

            isGrounded = false;
            canJump = false;
            Invoke("JumpCD", jumpCooldown);
        }
        else if(canJump)
        {
            selfBody.velocity = Vector3.up * jumpSecondaryForce;

            isGrounded = false;
            canJump = false;
            Invoke("JumpCD", jumpCooldown);
        }

    }

    public void JumpCD()
    {
        canJump = true;
    }
}
