using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class Weapon : MonoBehaviour
{
    public PlayerManager player;
    public Animator animator;
    public CameraHandler cameraHandler;
    public ItemStyle m_EItemStyle;

    public virtual void Awake()
    {
        player = GetComponentInParent<PlayerManager>();
        cameraHandler = Camera.main.GetComponentInParent<CameraHandler>();
    }

    public abstract void WeaponPrimaryAction(); // 마우스 왼쪽 클릭
    public abstract void WeaponSecondAction(); // 마우스 오른족 클릭
    public virtual void WeaponRACtion() { }
}
