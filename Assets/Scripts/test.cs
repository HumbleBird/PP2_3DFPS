using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class test : MonoBehaviour
    {

        #region Old

        [Header("MOVEMENT FX")]
        [SerializeField] PlayerManager player;
        [SerializeField] InputHandler inputHandler;

        [SerializeField, Range(0.05f, 2)] float RotationAmount = 0.2f;
        [SerializeField, Range(1f, 20)] float RotationSmooth = 6f;

        [Header("Movement")]
        [SerializeField] bool CanMovementFX = true;
        [SerializeField, Range(0.1f, 2)] float MovementAmount = 0.5f;

        public Camera cam;

        [HideInInspector]
        public float Lookvertical;
        [HideInInspector]
        public float Lookhorizontal;
        [SerializeField, Range(0.5f, 10)]
        public float lookSpeed = 2.0f;
        [SerializeField, Range(10, 120)]
        public float lookXLimit = 80.0f;
        [SerializeField] float RunningFOV = 65.0f;
        [SerializeField] float SpeedToFOV = 4.0f;
        float InstallFOV;

        [SerializeField] float rotationX = 0;

        Quaternion InstallRotation;
        Vector3 MovementVector;

        private void Start()
        {
            cam = GetComponentInChildren<Camera>();
            player = GetComponentInParent<PlayerManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            InstallRotation = transform.localRotation;
            InstallFOV = cam.fieldOfView;
        }

        private void Update()
        {
            HandleCameraLookValue();
            HandleCameraRotation();
        }

        private void HandleCameraLookValue()
        {
            if (player.canMove == false)
                return;

            Lookhorizontal += inputHandler.mouseX * lookSpeed;
            Lookvertical -= -inputHandler.mouseY * lookSpeed;

            Lookvertical = Mathf.Clamp(Lookvertical, -lookXLimit, lookXLimit);
        }

        private void HandleCameraRotation()
        {
            if (player.canMove == false)
                return;

            cam.transform.localRotation = Quaternion.Euler(Lookvertical, 0, 0);

            if (player.isRunning && player.Moving)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, RunningFOV, SpeedToFOV * Time.deltaTime);
            else
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, InstallFOV, SpeedToFOV * Time.deltaTime);
        }

        #endregion
    }
}
