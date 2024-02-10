using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [Header("MOVEMENT FX")]
    [SerializeField] PlayerManager Player;
    [SerializeField] InputHandler inputHandler;

    [SerializeField, Range(0.05f, 2)] float RotationAmount = 0.2f;
    [SerializeField, Range(1f, 20)] float RotationSmooth = 6f;
    [Header("Movement")]
    [SerializeField] bool CanMovementFX = true;
    [SerializeField, Range(0.1f, 2)] float MovementAmount = 0.5f;

    public Camera cam;

    Quaternion InstallRotation;
    Vector3 MovementVector;
    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
        Player = GetComponentInParent<PlayerManager>();
        inputHandler = GetComponentInParent<InputHandler>();
        InstallRotation = transform.localRotation;
    }

    private void Update()
    {
        //HandleRotation();
    }

    private void HandleRotation()
    {
        float movementX = (Player.playerLocomotionManager.moveDirection.x * RotationAmount);
        float movementZ = (-Player.playerLocomotionManager.moveDirection.z * RotationAmount);
        MovementVector = new Vector3(CanMovementFX ? movementX + Player.characterController.velocity.y * MovementAmount : movementX, 0, movementZ);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(MovementVector + InstallRotation.eulerAngles), Time.deltaTime * RotationSmooth);
    }
}
