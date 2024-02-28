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

    public abstract void WeaponPrimaryAction(); // ���콺 ���� Ŭ��
    public abstract void WeaponSecondAction(); // ���콺 ������ Ŭ��
    public virtual void WeaponRACtion() { }
}
