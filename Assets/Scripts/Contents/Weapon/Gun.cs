using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class Gun : Weapon
{
    public GunStyles currentStyle;

    // TODO 총기 데미지, 무게, 공속

    [Header("Bullet properties")]
    public short bulletsIHave = 20;
    public short bulletsInTheGun = 5;
    public short amountOfBulletsPerLoad = 5;
    public Transform bulletSpawnPlace;
    public GameObject bulletPrefab;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    [Header("Fire Property")]
    [Tooltip("Rounds per second if weapon is set to automatic rafal.")]
    public float roundsPerSecond;
    private float waitTillNextFire;

    [Header("Muzzle")]
    [Tooltip("Array of muzzel flashes, randmly one will appear after each bullet.")]
    public GameObject[] muzzelFlash;
    [Tooltip("Place on the gun where muzzel flash will appear.")]
    public GameObject muzzelSpawn;
    private GameObject holdFlash;
    private GameObject holdSmoke;

    [Header("Recoil")]
    public Transform recoilPivot;
    private Vector3 currentRotation;
    private Vector3 targetRotatipn;

    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    [SerializeField] private float aumRecoilX;
    [SerializeField] private float aumRecoilY;
    [SerializeField] private float aumRecoilZ;

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    [Header("Sounds")]
    public AudioClip audioClip_Shoot;
    public AudioClip audioClip_Reload;

    [Header("Sight")]
    public Transform sightTarget;
    public float sightOffset;
    public float aimingInTime;
    public GameObject weaponSwayObject;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;

    [Header("Aim")]
    public Transform m_AimingTransform;
    public Vector3 m_AimPositon;
    public Vector3 m_NormalPosition;
    public float m_fAimNormalChangeSpeed = 1f;

    [Header("Damage")]
    public int m_iDamage = 10;

    public override void Awake()
    {
        base.Awake();
    }

    public void Update()
    {
        waitTillNextFire -= roundsPerSecond * Time.deltaTime;

        CalculateRecoil();
        //HandleWeaponSway();
        HandleAiming();
    }

    public override void WeaponPrimaryAction()
    {
        ShootMethod();
    }

    private void ShootMethod()
    {
        if (player.isInteracting)
            return;

        if (player.m_E_PlayerMoveState == E_PlayerMoveState.Sprint ||
            player.m_E_PlayerMoveState == E_PlayerMoveState.Crouch ||
            player.m_E_PlayerMoveState == E_PlayerMoveState.Prone ||
            player.m_E_PlayerMoveState == E_PlayerMoveState.Climbing)
            return;

        if (waitTillNextFire <= 0)
        {

            if (bulletsInTheGun > 0)
            {
                // Bullet Spawn
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPlace.position, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.m_Rigidbody.AddForce(bulletSpawnPlace.forward.normalized * bulletVelocity, ForceMode.Impulse);
                bulletScript.LifeTime = bulletPrefabLifeTime;
                bulletScript.Owner = player;
                bulletScript.gun = this;

                // Muzzle
                int randomNumberForMuzzelFlash = Random.Range(0, 5);
                holdFlash = Instantiate(muzzelFlash[randomNumberForMuzzelFlash], muzzelSpawn.transform.position /*- muzzelPosition*/, muzzelSpawn.transform.rotation * Quaternion.Euler(0, 0, 90)) as GameObject;
                holdFlash.transform.parent = muzzelSpawn.transform;

                Managers.Sound.Play(audioClip_Shoot);

                RecoilFire();

                player.playerAnimatorManager.PlayTargetAnimation("Rifle_Fire", false);

                waitTillNextFire = 1;
                bulletsInTheGun -= 1;
            }

            else
            {
                Reload();
            }

        }

    }

    public void CalculateRecoil()
    {
        // Recoil
        targetRotatipn = Vector3.Lerp(targetRotatipn, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotatipn, snappiness * Time.deltaTime);
        recoilPivot.transform.localRotation = Quaternion.Euler(currentRotation);
    }

    public void RecoilFire()
    {
        if (player.isAiming)
        {
            targetRotatipn += new Vector3(aumRecoilX, Random.Range(-aumRecoilY, aumRecoilY), Random.Range(-aumRecoilZ, aumRecoilZ));
        }
        else
        {
            targetRotatipn += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        }
    }

    public override void WeaponSecondAction()
    {
    }

    public void HandleWeaponSway()
    {
        var targetPosition = transform.position;



        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingInTime);
        weaponSwayObject.transform.position = weaponSwayPosition;
    }

    public void HandleAiming()
    {
        if (player.isAiming && player.isInteracting == false)
        {
            //targetPosition = player.cameraHandler.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + player.cameraHandler.transform.forward * sightOffset;
            m_AimingTransform.localPosition = Vector3.Lerp(m_AimingTransform.localPosition, m_AimPositon, Time.deltaTime * m_fAimNormalChangeSpeed);
        }
        else
        {
            m_AimingTransform.localPosition = Vector3.Lerp(m_AimingTransform.localPosition, m_NormalPosition, Time.deltaTime * m_fAimNormalChangeSpeed);
        }
    }

    public override void WeaponRACtion()
    {
        base.WeaponRACtion();

        Reload();
    }

    public void Reload()
    {
        // 총의 종류에 따라 Reload 다르게 하기

        // Animation
        player.playerAnimatorManager.PlayTargetAnimation("Rifle_Reload", true);

        player.playerAnimatorManager.EraseHandIKForWeapon();

        Managers.Sound.Play(audioClip_Reload);
    }

    public void ReloadComplete()
    {
        // 장전해야 될 총알 갯수
        short reloadBulletCount = (short)(amountOfBulletsPerLoad - bulletsInTheGun);

        // 장전해야될 총알 갯수보다 소유 총알 갯수가 많다면 그대로 빼기
        if(bulletsIHave >= reloadBulletCount)
        {
            bulletsIHave -= reloadBulletCount;
            bulletsInTheGun = amountOfBulletsPerLoad;
        }
        // 장전할 총알이 별로 없다면 없는 만큼만
        else
        {
            bulletsInTheGun += bulletsIHave;
            bulletsIHave = 0;
        }
    }


}
