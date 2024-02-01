using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class scr_Models
{
    #region Player

    public enum PlayerStance
    {
        Stande,
        Crouch,
        Prone
    }

    [Serializable]
    public class PlayerSettingsModel
    {
        [Header("View Settings")]
        public float ViewXSensitivity = 10;
        public float ViewYSensitivity = 10;

        public bool ViewXInverted;
        public bool ViewYInverted;

        [Header("Movement - Sprint")]
        public bool SprintingHold;
        public float MovementSmoothing;

        [Header("Movement - Running")]
        public float RunningForwardSpeed;
        public float RunningStrafeSpeed;


        [Header("Movement - Walking")]
        public float WalkingForwardSpeed = 4;
        public float WalkingBackwardSpeed = 2;
        public float WalkingStrafeSpeed = 3;

        [Header("Jumping")]
        public float JumpingHeight = 0.4f;
        public float JumpingFalloff = 0.1f;
        public float FallingSmoothing;

        [Header("Speed Effectors")]
        public float SpeedEffector = 1;
        public float CrouchSpeedEffector;
        public float ProneSpeedEffector;
        public float FallingSpeedEffector;
    }

    [Serializable]
    public class CharacterStance
    {
        public float CameraHeight;
        public CapsuleCollider StanceCollider;
    }

    #endregion

    #region Weapons

    [Serializable]
    public class WeaponSettginsModel
    {
        [Header("Sway")]
        public float SwayAmount;
        public bool SwayYInverted;
        public bool SwayXInverted;
        public float SwaySmoothing;
    }

    #endregion
}
