//by EvolveGames
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolveGames
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerManager : MonoBehaviour
    {
        [Header("PlayerController")]
        public CharacterController characterController;
        public PlayerLocomotionManager playerLocomotionManager;
        [SerializeField] public PlayerWeaponManager playerWeaponManager;

        public Animator animator;
        public InputHandler inputHandler;

        [SerializeField] public Transform Camera;

        [Header("Flag")]
        public bool canMove = true;
        public bool CanRunning = true;
        public bool isRunning = false;
        public bool CanClimbing = true;
        public bool isClimbing = false;
        public bool Moving = true;
        public bool isCrough = false;
        public bool CanHideDistanceWall = true;
        public bool WallDistance;
        [SerializeField] public bool isGrounded { get { return characterController.isGrounded; } }
        public bool isAiming = false;

        [SerializeField, Range(0.1f, 5)] float HideDistance = 1.5f;
        [SerializeField] int LayerMaskInt = 1;

        public Camera cam;

        Vector3 InstallCameraMovement;

        void Start()
        {
            characterController = GetComponent<CharacterController>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerWeaponManager = GetComponent<PlayerWeaponManager>();
            cam = GetComponentInChildren<Camera>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            InstallCameraMovement = Camera.localPosition;

        }

        void Update()
        {

            RaycastHit ObjectCheck;


            // Check wall
            if (WallDistance != Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt) && CanHideDistanceWall)
            {
                WallDistance = Physics.Raycast(GetComponentInChildren<Camera>().transform.position, transform.TransformDirection(Vector3.forward), out ObjectCheck, HideDistance, LayerMaskInt);
                playerWeaponManager.ani.SetBool("Hide", WallDistance);
                playerWeaponManager.DefiniteHide = WallDistance;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ladder" && CanClimbing)
            { 
                CanRunning = false;
                isClimbing = true;
                playerLocomotionManager.WalkingValue /= 2;
                playerWeaponManager.Hide(true);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.tag == "Ladder" && CanClimbing)
            {
                playerLocomotionManager.moveDirection = new Vector3(0, inputHandler.vertical * playerLocomotionManager.Speed * (-Camera.localRotation.x / 1.7f), 0);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Ladder" && CanClimbing)
            {
                CanRunning = true;
                isClimbing = false;
                playerLocomotionManager.WalkingValue *= 2;
                playerWeaponManager.ani.SetBool("Hide", false);
                playerWeaponManager.Hide(false);
            }
        }
    }
}