//by EvolveGames
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerManager : MonoBehaviour
{
    [Header("PlayerController")]
    public CharacterController characterController;
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerWeaponManager playerWeaponManager;
    public PlayerAnimatorManager playerAnimatorManager;

    public CameraHandler cameraHandler;

    public Animator animator;
    public InputHandler inputHandler;

    [Header("Flag")]
    public bool isInteracting = false;
    public bool canMove = true;
    public bool CanRunning = true;
    public bool isRunning = false;
    public bool CanClimbing = true;
    public bool isClimbing = false;
    public bool Moving = true;
    public bool isCrouching = false;
    public bool CanHideDistanceWall = true;
    public bool WallDistance;
    [SerializeField] public bool isGrounded { get { return characterController.isGrounded; } }
    public bool isAiming = false;
    public bool isWeaponHiding = false;
    public bool isReloading = false;
    public bool isTwoHandingWeapon;

    [SerializeField, Range(0.1f, 5)] float HideDistance = 1.5f;
    [SerializeField] int LayerMaskInt = 1;


    Vector3 InstallCameraMovement;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        inputHandler = GetComponent<InputHandler>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerWeaponManager = GetComponent<PlayerWeaponManager>();
        cameraHandler = GetComponentInChildren<CameraHandler>();
        playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InstallCameraMovement = cameraHandler.transform.localPosition;

        Init();
    }

    private void Init()
    {
        playerWeaponManager.LoadTwoHandIKTargtets(true);
    }

    void Update()
    {
        RaycastHit ObjectCheck;

        // Check wall
        if (WallDistance != Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt) && CanHideDistanceWall)
        {
            WallDistance = Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt);
            //playerWeaponManager.ani.SetBool("Hide", WallDistance);
            playerWeaponManager.DefiniteHide = WallDistance;
        }

        // Animator Bool
        isInteracting = animator.GetBool("isInteracting");

        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isAiming", isAiming);

    }

    private void LateUpdate()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        playerAnimatorManager.CheckHandIKWeight(playerWeaponManager.rightHandIKTarget, playerWeaponManager.leftHandIKTarget, isTwoHandingWeapon);
    }


}
