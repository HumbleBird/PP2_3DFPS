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
    [SerializeField] float gravity = 20.0f;
    [SerializeField] float timeToRunning = 2.0f;
    [HideInInspector] public float Lookvertical;
    [HideInInspector] public float Lookhorizontal;
    [SerializeField, Range(0.5f, 10)] float lookSpeed = 2.0f;
    [SerializeField, Range(10, 120)] float lookXLimit = 80.0f;
    [SerializeField] float RunningFOV = 65.0f;
    [SerializeField] float SpeedToFOV = 4.0f;
    float InstallFOV;

    [SerializeField] float StandHeight = 1.8f;
    [SerializeField] Vector3 Stand_CharacterCenter = new Vector3(0, 0.94f, 0);
    [SerializeField] float CroughHeight = 1.33f;
    [SerializeField] Vector3 Crouch_CharacterCenter = new Vector3(0, 0.66f, 0);

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
        StandHeight = player.characterController.height;
        InstallFOV = player.cameraHandler.cam.fieldOfView;

    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit CroughCheck;

        HandleMovement();
        HandleRotation();

        if (player.inputHandler.m_Crouch_Input)
        {
            player.isCrouching = true;

            float Height = Mathf.Lerp(player.characterController.height, CroughHeight, 5 * Time.deltaTime);
            player.characterController.height = Height;
            Vector3 center = Vector3.Lerp(player.characterController.center, Crouch_CharacterCenter, Time.deltaTime);
            player.characterController.center = center;

            // Walk Speed;
            WalkingValue = Mathf.Lerp(WalkingValue, CroughSpeed, 6 * Time.deltaTime);
        }
        else if (!Physics.Raycast(player.cameraHandler.cam.transform.position, transform.TransformDirection(Vector3.up), out CroughCheck, 0.8f, 1))
        {
            if (player.characterController.height != StandHeight)
            {
                player.isCrouching = false;

                float Height = Mathf.Lerp(player.characterController.height, StandHeight, 6 * Time.deltaTime);
                player.characterController.height = Height;
                Vector3 center = Vector3.Lerp(player.characterController.center, Stand_CharacterCenter, Time.deltaTime);
                player.characterController.center = center;

                // Walk Speed;
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
        player.isRunning = !player.isCrouching ? player.CanRunning ? player.inputHandler.m_Sprint_Input : false : false;
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

        // Animator Parameters Value Change
        player.playerAnimatorManager.UpdateAnimatorValues(player.inputHandler.moveAmount, player.inputHandler.horizontal, player.isRunning);
    }

    private void HandleRotation()
    {
        if (player.canMove)
        {
            Lookvertical = -player.inputHandler.mouseY;
            Lookhorizontal = player.inputHandler.mouseX;

            rotationX += Lookvertical * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            player.cameraHandler.cam.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Lookhorizontal * lookSpeed, 0);

            if (player.isRunning && player.Moving) 
                player.cameraHandler.cam.fieldOfView = Mathf.Lerp(player.cameraHandler.cam.fieldOfView, RunningFOV, SpeedToFOV * Time.deltaTime);
            else 
                player.cameraHandler.cam.fieldOfView = Mathf.Lerp(player.cameraHandler.cam.fieldOfView, InstallFOV, SpeedToFOV * Time.deltaTime);
        }
    }
}
