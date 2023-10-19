using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float sprintSpeed = 10.0f; // 冲刺时的速度
    public float rotationSpeed = 700.0f;
    public float gravity = -9.81f;
    public float sprintDuration = 1.0f; // 冲刺的持续时间
    public Transform respawnPoint; // 指向场景中重生点的变量
    public LayerMask trapMask; // 用于识别陷阱的层

    private Vector3 velocity;
    private bool isSprinting = false;
    private float sprintTimer = 0.0f;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        HandleSprint();
        CheckForTraps();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationSpeed, 0.1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            float currentSpeed = isSprinting ? sprintSpeed : movementSpeed;
            characterController.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }

        // Handle gravity
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleSprint()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSprinting)
        {
            isSprinting = true;
            sprintTimer = sprintDuration;
        }

        if (isSprinting)
        {
            sprintTimer -= Time.deltaTime;
            if (sprintTimer <= 0)
            {
                isSprinting = false;
            }
        }
    }

    private void CheckForTraps()
    {
    if (Physics.CheckSphere(transform.position, characterController.radius, trapMask))
    {
        Debug.Log("Trap Detected!"); // 这行代码会在检测到陷阱时输出信息
        Respawn();
    }
    }

    private void Respawn()
    {
    transform.position = respawnPoint.position;
    velocity = Vector3.zero; // 重置玩家速度
    }

    
}


