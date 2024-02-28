using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isInteractingBool = "isInteracting";
    public bool isInteractingStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerManager character = animator.GetComponent<PlayerManager>();

        animator.SetBool(isInteractingBool, isInteractingStatus);
    }
}
