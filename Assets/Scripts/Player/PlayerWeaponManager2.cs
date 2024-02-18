using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

// 플레이어의 총기 보유
// 총의 움직임 줌 총알 슛, 쿨타임 등을 관리
// 총 관련 아이콘

public class PlayerWeaponManager2 : MonoBehaviour
{
    PlayerManager player;
    private Camera cameraComponent;
    private Transform gunPlaceHolder;

    public Weapon m_CurrentWeapon;
    public List<Weapon> m_PrimaryWeapons; // 주 무기
    public List<Weapon> m_SecondWeapons ; // 보조 무기
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
        // 무기 체인지 시 아이콘 변경
    }

    private void ChangeWeapon()
    {
        HideWeapon();

        // 키보드 숫자에 따른 무기 변경
        // 1 주무기
        // 2 보조무기
        // 3 근접무기
        // 4 투척무기
    }

    private void HideWeapon()
    {
        if(player.isWeaponHiding)
        {
            player.playerAnimatorManager.PlayTargetAnimation("Hide", true);
        }
    }


}
