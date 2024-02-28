using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.TextCore.Text;

public class PlayerAnimatorManager : MonoBehaviour
{
    PlayerManager player;

    protected RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandConstraint;
    public TwoBoneIKConstraint rightHandConstraint;

    int vertical;
    int horizontal;

    public bool handIKWeightReset = false;


    private void Awake()
    {
        player = GetComponent<PlayerManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");

        rigBuilder = GetComponentInChildren<RigBuilder>();
    }

    public void UpdateAnimatorValues(float verticalAmount, float horizontalAmount, bool isSprinting)
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

        if (isSprinting)
        {
            v = 2;
            h = horizontalAmount;
        }

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

    public virtual void SetHandIKForWeapon(RightHandIKTarget rightHandTarget, LeftHandIKTarget leftHandTarget, bool isTwoHandingWeapon)
    {
        if (rightHandConstraint == null || leftHandConstraint == null)
            return;

        if (isTwoHandingWeapon)
        {
            if (rightHandTarget != null)
            {
                rightHandConstraint.data.target = rightHandTarget.transform;
                rightHandConstraint.data.targetPositionWeight = 1; // 원한다면 각 무기 별로 할당 가능
                rightHandConstraint.data.targetRotationWeight = 1;
            }

            if (leftHandTarget != null)
            {
                leftHandConstraint.data.target = leftHandTarget.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
        else
        {
            rightHandConstraint.data.target = null;
            leftHandConstraint.data.target = null;
        }

        rigBuilder.Build();
    }

    public virtual void CheckHandIKWeight(RightHandIKTarget rightHandIK, LeftHandIKTarget leftHandIK, bool isTwoHandingWeapon)
    {
        if (player.isInteracting)
            return;

        //if (handIKWeightReset)
        {
            handIKWeightReset = false;

            if (rightHandConstraint.data.target != null)
            {
                rightHandConstraint.data.target = rightHandIK.transform;
                rightHandConstraint.data.targetPositionWeight = 1;
                rightHandConstraint.data.targetRotationWeight = 1;
            }

            if (leftHandConstraint.data.target != null)
            {
                leftHandConstraint.data.target = leftHandIK.transform;
                leftHandConstraint.data.targetPositionWeight = 1;
                leftHandConstraint.data.targetRotationWeight = 1;
            }
        }
    }

    public virtual void EraseHandIKForWeapon()
    {
        handIKWeightReset = true;

        if (rightHandConstraint.data.target != null)
        {
            rightHandConstraint.data.targetPositionWeight = 0;
            rightHandConstraint.data.targetRotationWeight = 0;
        }

        if (leftHandConstraint.data.target != null)
        {
            leftHandConstraint.data.targetPositionWeight = 0;
            leftHandConstraint.data.targetRotationWeight = 0;
        }
    }


}
