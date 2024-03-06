using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class InputHandler : MonoBehaviour
{
    PlayerControls inputActions;
    public PlayerManager player;

    public float mouseX;
    public float mouseY;

    [Header("Player Movement")]
    public bool m_Crouch_Input;
    public bool m_Prone_Input;
    public bool m_LeftLean_Input;
    public bool m_RightLean_Input;
    public bool m_Sprint_Input;
    public bool m_Walk_Input;
    public bool m_Jump_Input;

    [Header("Plyaer Actions")]
    public bool m_TapFire1_Input;
    public bool m_HoldFire1_Input;
    public bool m_TapFire2_Input;
    public bool m_HoldFire2_Input;
    public bool m_Reload_Input;
    public bool m_Hide_Input;

    [Header("UI")]
    public bool m_ESC_Input;

    Vector2 movementInput;
    Vector2 cameraInput;

    public float mouseWheel;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            inputActions.PlayerMovement.Crouch.performed += i =>   m_Crouch_Input             = true;
            inputActions.PlayerMovement.Crouch.canceled += i =>   m_Crouch_Input             = false;
            inputActions.PlayerMovement.Prone.performed += i =>    m_Prone_Input            = true;
            inputActions.PlayerMovement.LeftLean.performed += i => m_LeftLean_Input           = true;
            inputActions.PlayerMovement.RightLean.performed += i =>m_RightLean_Input            = true;
            inputActions.PlayerMovement.Sprint.performed += i =>   m_Sprint_Input            = true;
            inputActions.PlayerMovement.Sprint.canceled += i => m_Sprint_Input            = false;
            inputActions.PlayerMovement.Walk.performed += i => m_Walk_Input              = true;
            inputActions.PlayerMovement.Walk.canceled += i => m_Walk_Input              = false;
            inputActions.PlayerMovement.Jump.performed += i => m_Jump_Input                = true;

            inputActions.PlayerActions.TapFire1.performed += i =>      m_TapFire1_Input       = true;
            inputActions.PlayerActions.HoldFire1.performed += i =>     m_HoldFire1_Input       = true;
            inputActions.PlayerActions.HoldFire1.canceled += i =>     m_HoldFire1_Input       = false;
            inputActions.PlayerActions.TapFire2.performed += i =>      m_TapFire2_Input       = true;
            inputActions.PlayerActions.HoldFire2.performed += i =>     m_HoldFire2_Input       = true;
            inputActions.PlayerActions.HoldFire2.canceled += i =>     m_HoldFire2_Input       = false;
            inputActions.PlayerActions.Reload.performed += i =>        m_Reload_Input       = true;
            //inputActions.PlayerActions.ChangeWeapon.performed += i => m_ChangeWeapon_Input        = inputActions.ReadValue<Int>();
            inputActions.PlayerActions.Hide.performed += i => m_Hide_Input = true;

            inputActions.UI.ESC.performed += i => m_ESC_Input = true;
        }

        inputActions.Enable();
    }

    public void OnDisable()
    {
        inputActions.Disable();
    }

    public void Update()
    {
        if (player.isDead)
            return;

        if(player.canMove == false)
            return;

        HandleMoveInput();
        HandleCrouchInput();
        HandleJumpInput();

        HandleReloadInput();
        HandleChangeWeaponInput();
        HandleLeftLeanInput();
        HandleRightLeanInput();
        HandleFire1Input();
        HandleFire2Input();
        HandleHideInput();
    }

    private void LateUpdate()
    {
         m_TapFire1_Input = false;
    }

    public void HandleMoveInput()
    {
        player.playerLocomotionManager.horizontal = movementInput.x;
        player.playerLocomotionManager.vertical = movementInput.y;
        player.playerLocomotionManager.moveAmount = Mathf.Clamp01(Mathf.Abs(player.playerLocomotionManager.horizontal) + Mathf.Abs(player.playerLocomotionManager.vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;        
        player.playerLocomotionManager.m_fLookX = cameraInput.x;
        player.playerLocomotionManager.m_fLookY = cameraInput.y;

        if (player.isAiming && player.playerLocomotionManager.moveAmount > 0.5f)
            player.playerLocomotionManager.moveAmount = 0.5f;

        if (player.m_E_PlayerMoveState != E_PlayerMoveState.Crouch || 
            player.m_E_PlayerMoveState != E_PlayerMoveState.Prone  ||
            player.m_E_PlayerMoveState != E_PlayerMoveState.Climbing )
        {
            if (m_Sprint_Input)
                player.m_E_PlayerMoveState = E_PlayerMoveState.Sprint;
            else if (m_Walk_Input)
                player.m_E_PlayerMoveState = E_PlayerMoveState.Walk;
            else if (m_Sprint_Input == false && m_Walk_Input == false)
                player.m_E_PlayerMoveState = E_PlayerMoveState.Run;
        }
    }

    public void HandleCrouchInput()
    {
        if (m_Crouch_Input)
        {
            player.m_E_PlayerMoveState = E_PlayerMoveState.Crouch;
        }
        else if (m_Crouch_Input == false)
        {
            if(player.playerLocomotionManager.CheckCanStand())
            {
                player.playerLocomotionManager.HandleStand();
            }

        }

    }

    public void HandleJumpInput()
    {
        if(m_Jump_Input)
        {
            m_Jump_Input = false;

            // 위에 벽이 있는지 체크
            player.playerLocomotionManager.HandleJump();
        }
    }


    public void HandleReloadInput()
    {
        if(m_Reload_Input)
        {
            m_Reload_Input = false;

            if(player.playerWeaponManager.m_CurrentWeapon != null)
            {
                player.playerWeaponManager.m_CurrentWeapon.WeaponRACtion();
            }
        }
    }

    public void HandleChangeWeaponInput()
    {

    }

    public void HandleLeftLeanInput()
    {

    }

    public void HandleRightLeanInput()
    {

    }

    public void HandleFire1Input()
    {
        if(m_TapFire1_Input || m_HoldFire1_Input)
        {
           // m_TapFire1_Input = false;

            if(player.playerWeaponManager.m_CurrentWeapon != null)
            {
                player.playerWeaponManager.m_CurrentWeapon.WeaponPrimaryAction();
            }
        }
    }

    public void HandleFire2Input()
    {
        if(m_HoldFire2_Input)
        {
            player.isAiming = true;
        }
        else
        {
            player.isAiming = false;
        }
    }

    public void HandleHideInput()
    {

    }
}
