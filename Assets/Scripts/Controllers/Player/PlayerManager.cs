//by EvolveGames
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    [Header("PlayerController")]
    public CharacterController characterController;
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerWeaponManager playerWeaponManager;
    public PlayerAnimatorManager playerAnimatorManager;
    public PlayerStatManager playerStatManager;

    public Animator animator;

    [Header("Flag")]
    public bool isInteracting = false; // 총 장전, 수류탄 던지기 등의 특정 수행 행동
    public bool isGrounded;
    public bool isDead = false;
    public bool CanHideDistanceWall = true;
    public bool WallDistance;

    // Move
    public bool canMove = true;
    public bool CanClimbing = true;
    public bool CanStand = true;

    // Weapon
    public bool isAiming = false;
    public bool isTwoHandingWeapon;

    [SerializeField, Range(0.1f, 5)] float HideDistance = 1.5f;
    [SerializeField] int LayerMaskInt = 1;

    private E_PlayerMoveState ePlayerMoveState;

    public E_PlayerMoveState m_E_PlayerMoveState
    {
        set
        {
            if (value == ePlayerMoveState)
                return;

            ePlayerMoveState = value;
        }
        get { return ePlayerMoveState;}
    }

    // Wall & Ladder
    //private float m_fCheckLadder

    protected virtual void Awake()
    {
        characterController = GetComponent<CharacterController>();

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerWeaponManager = GetComponent<PlayerWeaponManager>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        playerStatManager = GetComponent<PlayerStatManager>(); 
        
        animator = GetComponent<Animator>();

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        //RaycastHit ObjectCheck;

        // Check wall
        //if (WallDistance != Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt) && CanHideDistanceWall)
        //{
        //    WallDistance = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt);
        //    //playerWeaponManager.ani.SetBool("Hide", WallDistance);
        //    playerWeaponManager.DefiniteHide = WallDistance;
        //}

        // Animator Bool
        isInteracting = animator.GetBool("isInteracting");

        if(ePlayerMoveState == E_PlayerMoveState.Crouch)
            animator.SetBool("isCrouching", true);
        else
            animator.SetBool("isCrouching", false);

        animator.SetBool("isAiming", isAiming);
        animator.SetBool("IsDead", isDead);
    }

    public virtual void OnDead()
    {
        isDead = true;
        canMove = false;

        // RegDoll
    }

    public virtual void LifeInit()
    {
        playerStatManager.Init();
        FlagInit();
        playerWeaponManager.Init();
        playerLocomotionManager.Init();
    }

    public void FlagInit()
    {
        isInteracting = false; // 총 장전, 수류탄 던지기 등의 특정 수행 행동
        isDead = false;
        CanHideDistanceWall = true;


        canMove = true;
        CanClimbing = true;
        CanStand = true;


        isAiming = false;
        isTwoHandingWeapon = false;
    }
}
