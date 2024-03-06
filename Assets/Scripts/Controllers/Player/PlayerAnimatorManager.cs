using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.TextCore.Text;

public class PlayerAnimatorManager : MonoBehaviour
{
    PlayerManager player;

    int vertical;
    int horizontal;

    [Header("Rig Builder")]
    public Transform LeftHandIKTarget  ;
    public Transform RightHandIKTarget ;
    public Transform LeftElbowIKTarget ;
    public Transform RightElbowIKTarget;

    public MultiAimConstraint SpineConstraint;
    public MultiAimConstraint UpperChestConstraint;
    public MultiAimConstraint HeadConstraint;
    public MultiAimConstraint RightShoulderConstraint;

    public bool handIKWeightReset = false;

    [Range(0, 1f)]
    public float HandIKAmount = 1f;
    [Range(0, 1f)]
    public float ElbowIKAmount = 1f;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
    }

    private void Update()
    {
        // Animator Parameters Value Change
        UpdateAnimatorValues(player.playerLocomotionManager.moveAmount, player.playerLocomotionManager.horizontal);
    }

    public void UpdateAnimatorValues(float verticalAmount, float horizontalAmount)
    {
        #region Vertical

        float v = 0;
        if (verticalAmount > 0 && verticalAmount < 0.55f)
        {
            v = 0.5f;
        }
        else if (verticalAmount > 0.55f)
        {
            v = 1;
        }
        else if (verticalAmount < 0 && verticalAmount > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalAmount < -0.55f)
        {
            v = -1;
        }
        else
        {
            v = 0;
        }

        #endregion
        #region Horizontal

        float h = 0;
        if (horizontalAmount > 0 && horizontalAmount < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalAmount > 0.55f)
        {
            h = 1;
        }
        else if (horizontalAmount < 0 && horizontalAmount > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalAmount < -0.55f)
        {
            h = -1;
        }
        else
        {
            h = 0;
        }

        #endregion

        switch (player.m_E_PlayerMoveState)
        {
            case Define.E_PlayerMoveState.Walk:
                //v /= 2;
                break;
            case Define.E_PlayerMoveState.Run:
                break;
            case Define.E_PlayerMoveState.Sprint:
                v = 2;
                break;
        }

        h = horizontalAmount;

        player.animator.SetFloat("Vertical", v, 0.1f, Time.deltaTime);
        player.animator.SetFloat("Horizontal", h, 0.1f, Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool mirrorAnim = false)
    {
        //player.animator.applyRootMotion = isInteracting;
        player.animator.SetBool("isInteracting", isInteracting);
        player.animator.SetBool("isMirrored", mirrorAnim);
        player.animator.CrossFade(targetAnim, 0.2f);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (player.isInteracting)
            return;

        if (player.playerWeaponManager.m_CurrentWeapon == null)
            return;

        if(LeftHandIKTarget != null)
        {
            player.animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            player.animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, HandIKAmount);
            player.animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIKTarget.position);
            player.animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIKTarget.rotation);
        }
        if(RightHandIKTarget != null)
        {
            player.animator.SetIKRotationWeight(AvatarIKGoal.RightHand, HandIKAmount);
            player.animator.SetIKPositionWeight(AvatarIKGoal.RightHand, HandIKAmount);
            player.animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandIKTarget.position);
            player.animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandIKTarget.rotation);
        }
        if(LeftElbowIKTarget != null)
        {
            player.animator.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftElbowIKTarget.position);
            player.animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, ElbowIKAmount);
        }

        if(RightElbowIKTarget != null)
        {
            player.animator.SetIKHintPosition(AvatarIKHint.RightElbow, RightElbowIKTarget.position);
            player.animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, ElbowIKAmount);
        }
    }

    public void SetWeaponIKTransform(Transform weapon)
    {
        Transform[] allChildren = weapon.GetComponentsInChildren<Transform>();
        LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftElbow");
        RightHandIKTarget = allChildren.FirstOrDefault(child => child.name == "RightElbow");
        LeftElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "LeftHand");
        RightElbowIKTarget = allChildren.FirstOrDefault(child => child.name == "RightHand");

    }

    public void EraseHandIKForWeapon()
    {
        LeftHandIKTarget = null;
        RightHandIKTarget = null;
        LeftElbowIKTarget = null;
        RightElbowIKTarget = null;
    }
}
