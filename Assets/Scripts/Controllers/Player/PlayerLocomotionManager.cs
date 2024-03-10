using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using static Define;

public class PlayerLocomotionManager : MonoBehaviour
{
    private PlayerManager player;

    [Header("Ground Movement")]
    [SerializeField] float currentMoveSpeed;
    [SerializeField] float walkSpeed = 3.0f;
    [SerializeField] float RunSpeed = 4.0f;
    [SerializeField] float SprintSpeed = 5.0f;
    [SerializeField] float CrouchSpeed = 2.5f;
    [SerializeField] float ProneSpeed = 2.0f;
    [SerializeField] float ClimbingSpeed = 1.0f;

    [Header("Gravity Settings")]
    public float inAirTimer;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity = -7.5f;
    [SerializeField] protected float fallStartYVelocity = -7;
    [SerializeField] protected float gravityForce = -0.8f;
    [SerializeField] float groundCheckSphereRadius = 0.17f;
    [SerializeField] protected float m_FallingDamageCanTime = 0.5f;
    [SerializeField] protected float m_FallingDeadTime = 1f;
    protected bool fallingVelocity = false;
    [SerializeField] private float JumpPower;

    [Header("Collider Height")]
    [SerializeField] float StandHeight = 1.8f;
    [SerializeField] Vector3 Stand_CharacterCenter = new Vector3(0, 0.94f, 0);
    [SerializeField] float CroughHeight = 1.33f;
    [SerializeField] Vector3 Crouch_CharacterCenter = new Vector3(0, 0.66f, 0);


    public Vector3 moveDirection = Vector3.zero;
    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float m_fLookX;
    public float m_fLookY;
    public float m_fRotationSpeed;

    public LayerMask groundLayer = 1 << 0;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        player.isGrounded = Physics.CheckSphere(transform.position, groundCheckSphereRadius, groundLayer);
        player.animator.SetBool("isGrounded", player.isGrounded);
        player.animator.SetFloat("inAirTimer", inAirTimer);

        HandleMovement();
        HandleRotation();
        HandleCrouch();
        HandleGroundCheck();
    }

    public void Init()
    {
        StandHeight = player.characterController.height;

    }

    private void HandleMovement()
    {
        if (player.isDead)
            return;

        if (player.canMove == false)
            return;

        if (player.isGrounded == false)
            return;

        // Crouch 상태에서는 Running 을 못 한다.
        // Player의 이동 상태는
        // 걷기 (alt 누름 && Stand 상태)
        // 뛰기 (그냥 W 누르기)
        // 전력 질주 (Shift 누름 상태 + w 누름)
        // Crouch (ctrl 앉기), 뛰기 안 됨
        // 눕기 (x 상태), 뛰기 안 됨

        switch (player.m_E_PlayerMoveState)
        {
            case E_PlayerMoveState.Walk:
                currentMoveSpeed = walkSpeed;
                break;
            case E_PlayerMoveState.Run:
                currentMoveSpeed = RunSpeed;
                break;
            case E_PlayerMoveState.Sprint:
                currentMoveSpeed = SprintSpeed;
                break;
            case E_PlayerMoveState.Crouch:
                currentMoveSpeed = CrouchSpeed;
                break;
            case E_PlayerMoveState.Prone:
                currentMoveSpeed = ProneSpeed;
                break;
            case E_PlayerMoveState.Climbing:
                currentMoveSpeed = ClimbingSpeed;
                break;
        }

        // Movement Direction
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        MyPlayerManager myPlayer = player as MyPlayerManager;
        if (myPlayer != null)
        {
            moveDirection = myPlayer.cameraHandler.transform.forward * myPlayer.playerLocomotionManager.vertical;
            moveDirection += myPlayer.cameraHandler.transform.right * myPlayer.playerLocomotionManager.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            player.characterController.Move(moveDirection * Time.deltaTime * currentMoveSpeed);
        }
    }

    private void HandleRotation()
    {
        MyPlayerManager myPlayer = player as MyPlayerManager;
        if(myPlayer != null)
            transform.rotation *= Quaternion.Euler(0, myPlayer.cameraHandler.Lookhorizontal * myPlayer.cameraHandler.lookSpeed, 0);

        //Vector3 targetDir = Vector3.zero;

        //MyPlayerManager myPlayer = player as MyPlayerManager;

        //targetDir = myPlayer.cameraHandler.cam.transform.forward * myPlayer.playerLocomotionManager.vertical;
        //targetDir += myPlayer.cameraHandler.cam.transform.right * myPlayer.playerLocomotionManager.horizontal;
        //targetDir.Normalize();
        //targetDir.y = 0;

        //if (targetDir == Vector3.zero)
        //    targetDir = transform.forward;

        //float rs = m_fRotationSpeed;

        //Quaternion tr = Quaternion.LookRotation(targetDir);
        //Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * Time.deltaTime);

        //transform.rotation = targetRotation;
    }

    private void HandleCrouch()
    {
        if (player.m_E_PlayerMoveState == E_PlayerMoveState.Crouch)
        {
            float Height = Mathf.Lerp(player.characterController.height, CroughHeight, 5 * Time.deltaTime);
            player.characterController.height = Height;
            Vector3 center = Vector3.Lerp(player.characterController.center, Crouch_CharacterCenter, Time.deltaTime);
            player.characterController.center = center;
        }
    }

    public void HandleStand()
    {
        if (player.characterController.height != StandHeight)
        {
            float Height = Mathf.Lerp(player.characterController.height, StandHeight, 6 * Time.deltaTime);
            player.characterController.height = Height;
            Vector3 center = Vector3.Lerp(player.characterController.center, Stand_CharacterCenter, Time.deltaTime);
            player.characterController.center = center;
        }
    }

    public bool CheckCanStand()
    {
        RaycastHit CroughCheck;

        Vector3 playerHeadPosition = transform.position;
        playerHeadPosition.y += player.characterController.height;

        // 머리 위 장애물이 있는지 검사
        if (!Physics.Raycast(playerHeadPosition, transform.TransformDirection(Vector3.up), out CroughCheck, 0.8f, 1))
        {
            return true;
        }
        else
            return false;
    }

    public void HandleJump()
    {
        if (player.isDead)
            return;

        if (player.canMove == false)
            return;

        // Jump
        if (player.isGrounded && player.m_E_PlayerMoveState != E_PlayerMoveState.Climbing)
        {
            moveDirection.y = JumpPower;

            //transform.position = Vector3.Slerp(transform.position, transform.position + moveDirection, Time.deltaTime);
            player.characterController.Move(moveDirection);

            // Animation

            player.playerAnimatorManager.PlayTargetAnimation("Jump", false);
        }
    }

    public void HandleGroundCheck()
    {
        if (player.isDead)
            return;

        // 땅에 착지
        if (player.isGrounded)
        {
            if (inAirTimer > 0)
            {
                // Damage Check
                if (inAirTimer <= m_FallingDeadTime && inAirTimer >= m_FallingDamageCanTime)
                {
                    // Stat
                    //player.characterStatsManager.currentHealth -= Mathf.RoundToInt(inAirTimer * 5);

                    // UI Refresh
                    //player.characterStatsManager.HealthBarUIUpdate(Mathf.RoundToInt(Mathf.RoundToInt(inAirTimer * 5)));

                    // Sound
                    //Managers.Sound.SoundPlayFromCharacter(gameObject, "Character/Common/Fall_Land", player.characterSoundFXManager.m_AudioSource);

                    inAirTimer = 0;
                    fallingVelocity = false;
                    yVelocity.y = groundedYVelocity;
                }
            }

            inAirTimer = 0;

            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocity = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        // 공중에 뜨는 중
        else if (player.m_E_PlayerMoveState != E_PlayerMoveState.Climbing)
        {
            if (!fallingVelocity)
            {
                fallingVelocity = true;
                yVelocity.y = fallStartYVelocity;
            }

            inAirTimer += Time.deltaTime;
            yVelocity.y += gravityForce * Time.deltaTime;

            //FallingDeadCheck();

            //transform.position = Vector3.Slerp(transform.position, transform.position + yVelocity, Time.deltaTime);
            player.characterController.Move(yVelocity * Time.deltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, groundCheckSphereRadius);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == groundLayer)
            Debug.Log(collision.gameObject.name);
    }
}
