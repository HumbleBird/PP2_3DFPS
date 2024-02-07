using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class PlayerLocomotionManager : MonoBehaviour
{
    public PlayerManager player;

    [SerializeField, Range(1, 10)] float walkingSpeed = 3.0f;
    [Range(0.1f, 5)] public float CroughSpeed = 1.0f;
    [SerializeField, Range(2, 20)] float RuningSpeed = 4.0f;
    [SerializeField, Range(0, 20)] float jumpSpeed = 6.0f;
    [SerializeField]  float installGravity;
    [SerializeField]  float rotationX = 0;
    [SerializeField]  float movementDirectionY;

    [Space(20)]
    [Header("Climbing")]
    [SerializeField, Range(1, 25)] public float Speed = 2f;

    public Vector3 moveDirection = Vector3.zero;
    [SerializeField] protected Vector3 yVelocity;

    [HideInInspector] public float WalkingValue;
    float RunningValue;
    float InstallCroughHeight;
    [SerializeField] float CroughHeight = 1.0f;
    [SerializeField] float gravity = 20.0f;
    [SerializeField] float timeToRunning = 2.0f;
    [HideInInspector] public float Lookvertical;
    [HideInInspector] public float Lookhorizontal;
    [SerializeField, Range(0.5f, 10)] float lookSpeed = 2.0f;
    [SerializeField, Range(10, 120)] float lookXLimit = 80.0f;
    [SerializeField] float RunningFOV = 65.0f;
    [SerializeField] float SpeedToFOV = 4.0f;
    float InstallFOV; 
    

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        RunningValue = RuningSpeed;
        installGravity = gravity;
        WalkingValue = walkingSpeed;
        InstallCroughHeight = player.characterController.height;
        InstallFOV = player.cam.fieldOfView;

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit CroughCheck;

        HandleMovement();
        HandleRotation();

        if (player.inputHandler.m_Crouch_Input)
        {
            player.isCrough = true;
            float Height = Mathf.Lerp(player.characterController.height, CroughHeight, 5 * Time.deltaTime);
            player.characterController.height = Height;
            WalkingValue = Mathf.Lerp(WalkingValue, CroughSpeed, 6 * Time.deltaTime);
        }
        else if (!Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.up), out CroughCheck, 0.8f, 1))
        {
            if (player.characterController.height != InstallCroughHeight)
            {
                player.isCrough = false;
                float Height = Mathf.Lerp(player.characterController.height, InstallCroughHeight, 6 * Time.deltaTime);
                player.characterController.height = Height;
                WalkingValue = Mathf.Lerp(WalkingValue, walkingSpeed, 4 * Time.deltaTime);
            }
        }
    }

    private void HandleMovement()
    {
        // Gravity
        if (!player.characterController.isGrounded && !player.isClimbing)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Movement Direction
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        player.isRunning = !player.isCrough ? player.CanRunning ? player.inputHandler.m_Sprint_Input : false : false;
        player.inputHandler.vertical = player.canMove ? (player.isRunning ? RunningValue : WalkingValue) * player.inputHandler.vertical : 0;
        player.inputHandler.horizontal = player.canMove ? (player.isRunning ? RunningValue : WalkingValue) * player.inputHandler.horizontal : 0;
        if (player.isRunning) RunningValue = Mathf.Lerp(RunningValue, RuningSpeed, timeToRunning * Time.deltaTime);
        else RunningValue = WalkingValue;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * player.inputHandler.vertical) + (right * player.inputHandler.horizontal);

        // Jump
        if (player.inputHandler.m_Jump_Input && player.canMove && player.characterController.isGrounded && !player.isClimbing)
        {
            player.inputHandler.m_Jump_Input = false;
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        player.characterController.Move(moveDirection * Time.deltaTime);
        player.Moving = player.inputHandler.horizontal < 0 || player.inputHandler.vertical < 0 || player.inputHandler.horizontal > 0 || player.inputHandler.vertical > 0 ? true : false;
    }

    private void HandleRotation()
    {
        if (player.canMove)
        {
            Lookvertical = -player.inputHandler.mouseY;
            Lookhorizontal = player.inputHandler.mouseX;

            rotationX += Lookvertical * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            player.Camera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Lookhorizontal * lookSpeed, 0);

            if (player.isRunning && player.Moving) player.cam.fieldOfView = Mathf.Lerp(player.cam.fieldOfView, RunningFOV, SpeedToFOV * Time.deltaTime);
            else player.cam.fieldOfView = Mathf.Lerp(player.cam.fieldOfView, InstallFOV, SpeedToFOV * Time.deltaTime);
        }
    }
}
