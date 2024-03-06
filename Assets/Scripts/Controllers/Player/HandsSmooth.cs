using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolveGames
{
    public class HandsSmooth : MonoBehaviour
    {
        PlayerManager player;

        [Header("HandsSmooth")]
        [SerializeField, Range(1, 10)] float smooth = 4f;
        [SerializeField, Range(0.001f, 1)] float amount = 0.03f;
        [SerializeField, Range(0.001f, 1)] float maxAmount = 0.04f;

        [Header("Rotation")]
        [SerializeField, Range(1, 10)] float RotationSmooth = 4.0f;
        [SerializeField, Range(0.1f, 10)] float RotationAmount = 1.0f;
        [SerializeField, Range(0.1f, 10)] float MaxRotationAmount = 5.0f;
        [SerializeField, Range(0.1f, 10)] float RotationMovementMultipler = 1.0f;

        [Header("CroughRotation")]
        [SerializeField] bool EnabledCroughRotation = false;
        [SerializeField, Range(0.1f, 20)] float RotationCroughSmooth = 15.0f;
        [SerializeField, Range(5f, 50)] float RotationCroughMultipler = 18.0f;

        float CroughRotation;
        Vector3 InstallPosition;
        Quaternion InstallRotation;

        private void Awake()
        {
            player = GetComponentInParent<PlayerManager>();
        }


        private void Start()
        {
            InstallPosition = transform.localPosition;
            InstallRotation = transform.localRotation;
        }
        private void Update()
        {

            float InputX = -player.playerLocomotionManager.m_fLookX;
            float InputY = -player.playerLocomotionManager.m_fLookY;
            float horizontal = -player.playerLocomotionManager.horizontal;
            float vertical =  player.playerLocomotionManager.vertical;

            float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
            float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);

            Vector3 finalPosition = new Vector3(moveX, moveY + -player.characterController.velocity.y / 60, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + InstallPosition, Time.deltaTime * smooth);

            float TiltX = Mathf.Clamp(InputX * RotationAmount, -MaxRotationAmount, MaxRotationAmount);
            float TiltY = Mathf.Clamp(InputY * RotationSmooth, -MaxRotationAmount, MaxRotationAmount);
            if (EnabledCroughRotation && player.m_E_PlayerMoveState == Define.E_PlayerMoveState.Crouch) 
                CroughRotation = Mathf.Lerp(CroughRotation, RotationCroughMultipler, RotationCroughSmooth * Time.deltaTime);
            else 
                CroughRotation = Mathf.Lerp(CroughRotation, 0f, RotationCroughSmooth * Time.deltaTime);
            Vector3 vector = new Vector3(Mathf.Max(vertical * 0.4f, 0) * RotationMovementMultipler, 0, horizontal * RotationMovementMultipler);
            Vector3 finalRotation = new Vector3(-TiltY, 0, TiltX + CroughRotation) + vector;



            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(finalRotation) * InstallRotation, Time.deltaTime * RotationSmooth);
        }
    }
}