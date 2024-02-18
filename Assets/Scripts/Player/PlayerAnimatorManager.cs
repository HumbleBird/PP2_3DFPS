using EvolveGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerAnimatorManager : MonoBehaviour
{
    PlayerManager player;

    int vertical;
    int horizontal;

    private  void Awake()
    {
        player = GetComponent<PlayerManager>();

        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
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

    public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRoate = false, bool mirrorAnim = false, bool canRoll = false)
    {
        player.animator.applyRootMotion = isInteracting;
        player.animator.SetBool("canRotate", canRoate);
        player.animator.SetBool("isInteracting", isInteracting);
        player.animator.SetBool("isMirrored", mirrorAnim);
        player.animator.CrossFade(targetAnim, 0.2f);
    }
}
