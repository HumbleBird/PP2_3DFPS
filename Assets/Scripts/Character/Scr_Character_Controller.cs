using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static scr_Models;

public class Scr_Character_Controller : MonoBehaviour
{
    CharacterController characterController;
    private DefaultInput defaultInput;
    public Vector2 input_Movement;
    public Vector2 input_View;

    private Vector3 newCameraRotation;
    private Vector3 newCharacterRotation;

    [Header("References")]
    public Transform cameraHolder;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;

    [Header("Gravity")]
    public float grabityAmount = 0.1f;
    public float grabityMin = -3;
    private float playerGravity;

    public Vector3 jumpingForce;
    private Vector3 jumpingForceVelocity;


    [Header("Player Stance")]
    public PlayerStance playerStance;
    public float playerStanceSmoothing;
    public float cameraStandeHeight;
    public float cameraCrouchHeight;
    public float cameraProneHeight;

    public float cameraHeight;
    public float cameraHeightVelocity;

    private void Awake()
    {
        defaultInput = new DefaultInput();

        defaultInput.Characer.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Characer.View.performed += e => input_View = e.ReadValue<Vector2>();
        defaultInput.Characer.Jump.performed += e => Jump();

        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
        newCharacterRotation = transform.localRotation.eulerAngles;

        characterController = GetComponent<CharacterController>();

        cameraHeight = cameraHolder.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateView();
        CalculateMovement();
        CaculateJump();
        CalculateCameraHeight();
    }

    private void CalculateCameraHeight()
    {
        float stanceHeight = cameraStandeHeight;

        if(playerStance == PlayerStance.Crouch)
        {
            stanceHeight = cameraCrouchHeight;
        }
        else if (playerStance == PlayerStance.Prone)
        {
            stanceHeight = cameraProneHeight;
        }

        cameraHeight = Mathf.SmoothDamp(cameraHolder.localPosition.y, stanceHeight, ref cameraHeightVelocity, playerStanceSmoothing);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraHeight, cameraHolder.localPosition.z);
    }

    private void CalculateView()
    {
        newCharacterRotation.y += playerSettings.ViewYSensitivity * (playerSettings.ViewXInverted ? -input_View.x : input_View.x) * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(newCharacterRotation);

        newCameraRotation.x += playerSettings.ViewYSensitivity * (playerSettings.ViewYInverted ? input_View.y  : -input_View.y) * Time.deltaTime;
        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateMovement()
    {
        var verticalSpeed = playerSettings.WalkingForwardSpeed * input_Movement.y * Time.deltaTime;
        var horizontalSpeed = playerSettings.WalkingStrafeSpeed * input_Movement.x * Time.deltaTime;

        var newMovementSpeed = new Vector3(horizontalSpeed, 0, verticalSpeed);
        newMovementSpeed = transform.TransformDirection(newMovementSpeed);

        if(playerGravity > grabityMin)
        {
            playerGravity -= grabityAmount * Time.deltaTime;
        }

        if(playerGravity < -0.1f && characterController.isGrounded)
        {
            playerGravity = -0.1f;
        }


        newMovementSpeed.y += playerGravity;
        newMovementSpeed += jumpingForce * Time.deltaTime;

        characterController.Move(newMovementSpeed);
    }

    private void CaculateJump()
    {
        jumpingForce = Vector3.SmoothDamp(jumpingForce, Vector3.zero, ref jumpingForceVelocity, playerSettings.JumpingFalloff);
    }

    private void Jump()
    {
        if(!characterController.isGrounded)
        {
            return;
        }

        // Jump
        jumpingForce = Vector3.up * playerSettings.JumpingHeight;
        playerGravity = 0;

    }
}
