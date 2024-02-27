using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour
{
    private Animator _animator;

    public bool IkActive = true;
    public RightHandIKTarget RightHandTarget;
    public RightHandIKTarget LeftHandTarget;

    // Start is called before the first frame update
    void Awake()
    {
		_animator = GetComponent<Animator>();

    }

    //a callback for calculating IK
    void OnAnimatorIKs(int layerIndex)
    {
        if (_animator)
        {

            float v = IkActive ? 1 : 0;

            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, v);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, v);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, v);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, v);

            if (IkActive)
            {
                // Set the right hand target position and rotation, if one has been assigned
                _animator.SetIKPosition(AvatarIKGoal.RightHand, RightHandTarget.transform.position);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandTarget.transform.rotation);
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.transform.position);
                _animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTarget.transform.rotation);
            }
        }
    }
}
