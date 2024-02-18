using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

// �÷��̾��� �ѱ� ����
// ���� ������ �� �Ѿ� ��, ��Ÿ�� ���� ����
// �� ���� ������

public class PlayerWeaponManager2 : MonoBehaviour
{
    PlayerManager player;
    private Camera cameraComponent;
    private Transform gunPlaceHolder;

    public Weapon m_CurrentWeapon;
    public List<Weapon> m_PrimaryWeapons; // �� ����
    public List<Weapon> m_SecondWeapons ; // ���� ����
    public List<Weapon> m_MeleeWeapons  ;
    public List<Weapon> m_ThrowWeapons  ;
    private int m_CurrentInt_PrimaryWeapon;
    private int m_CurrentInt_SecondWeapon ;
    private int m_CurrentInt_MeleeWeapon  ;
    private int m_CurrentInt_ThrowWeapon;

    [Header("Bullet properties")]
    [Tooltip("Preset value to tell with how many bullets will our waepon spawn aside.")]
    public float bulletsIHave = 20;
    [Tooltip("Preset value to tell with how much bullets will our waepon spawn inside rifle.")]
    public float bulletsInTheGun = 5;
    [Tooltip("Preset value to tell how much bullets can one magazine carry.")]
    public float amountOfBulletsPerLoad = 5;



    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        cameraComponent = GetComponentInChildren<CameraHandler>().cam;
    }

    private void Update()
    {
    }

    private void UpdateWeaponImgage()
    {
        // ���� ü���� �� ������ ����
    }

    private void ChangeWeapon()
    {
        HideWeapon();

        // Ű���� ���ڿ� ���� ���� ����
        // 1 �ֹ���
        // 2 ��������
        // 3 ��������
        // 4 ��ô����
    }

    private void HideWeapon()
    {
        if(player.isWeaponHiding)
        {
            player.playerAnimatorManager.PlayTargetAnimation("Hide", true);
        }
    }


}
