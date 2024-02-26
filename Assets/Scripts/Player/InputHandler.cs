using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    PlayerControls inputActions;
    public PlayerManager player;

    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    [Header("Player Movement")]
    public bool m_Crouch_Input;
    public bool m_Prone_Input;
    public bool m_LeftLean_Input;
    public bool m_RightLean_Input;
    public bool m_Sprint_Input;
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
            inputActions.PlayerMovement.Sprint.canceled += i =>   m_Sprint_Input            = false;
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
        HandleMoveInput();
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
        horizontal = movementInput.x;
        vertical = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        mouseX = cameraInput.x;
        mouseY = cameraInput.y;

        if (player.isAiming && moveAmount > 0.5f)
            moveAmount = 0.5f;

    }

    public void HandleReloadInput()
    {

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
        if (!player.isGrounded ||
            player.isRunning)
        {
            m_HoldFire2_Input = false;
            return;
        }

        if (player.playerWeaponManager.m_CurrentWeapon == null)
            return;

        if (m_HoldFire2_Input)
        {
                player.isAiming = true;
            player.playerWeaponManager.m_CurrentWeapon.WeaponSecondAction();
        }
        else
        {
            if (player.isAiming)
            {
                player.isAiming = false;
                //player.cameraHandler.ResetAimCameraRotations();
            }
        }
    }

    public void HandleHideInput()
    {
        if(m_Hide_Input == false && player.isInteracting == false)
        {
            m_Hide_Input = true;
            player.isWeaponHiding = true;
        }
    }
}
