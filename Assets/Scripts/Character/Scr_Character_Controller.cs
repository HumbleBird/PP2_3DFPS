using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static scr_Models;

public class Scr_Character_Controller : MonoBehaviour
{
    private DefaultInput defaultInput;
    public Vector2 input_Movement;
    public Vector2 input_View;

    private Vector3 newCameraRotation;

    [Header("References")]
    public Transform cameraHolder;

    [Header("Settings")]
    public PlayerSettingsModel playerSettings;
    public float viewClampYMin = -70;
    public float viewClampYMax = 80;

    private void Awake()
    {
        defaultInput = new DefaultInput();

        defaultInput.Characer.Movement.performed += e => input_Movement = e.ReadValue<Vector2>();
        defaultInput.Characer.View.performed += e => input_View = e.ReadValue<Vector2>();

        defaultInput.Enable();

        newCameraRotation = cameraHolder.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateView();
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        newCameraRotation.x += playerSettings.ViewYSensitivity * input_View.y * Time.deltaTime;

        newCameraRotation.x = Mathf.Clamp(newCameraRotation.x, viewClampYMin, viewClampYMax);

        cameraHolder.localRotation = Quaternion.Euler(newCameraRotation);
    }

    private void CalculateView()
    {
    }
}
