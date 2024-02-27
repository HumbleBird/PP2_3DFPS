using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class Gun : Weapon
{
    public GunStyles currentStyle;

    [Header("Bullet properties")]
    public float bulletsIHave = 20;
    public float bulletsInTheGun = 5;
    public float amountOfBulletsPerLoad = 5;
    public Transform bulletSpawnPlace;
    public GameObject bullet;
    private Transform gunPlaceHolder;

    [Header("Fire Property")]
    [Tooltip("Rounds per second if weapon is set to automatic rafal.")]
    public float roundsPerSecond;
    private float waitTillNextFire;

    [HideInInspector]
    public Vector3 currentGunPosition;

    [Header("Gun Positioning")]
    [Tooltip("Vector 3 position from player SETUP for NON AIMING values")]
    public Vector3 restPlacePosition;
    [Tooltip("Vector 3 position from player SETUP for AIMING values")]
    public Vector3 aimPlacePosition;
    [Tooltip("Time that takes for gun to get into aiming stance.")]
    public float gunAimTime = 0.1f;


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


    [Header("Crosshair properties")]
    public Texture horizontal_crosshair, vertical_crosshair;
    public Vector2 top_pos_crosshair, bottom_pos_crosshair, left_pos_crosshair, right_pos_crosshair;
    public Vector2 size_crosshair_vertical = new Vector2(1, 1), size_crosshair_horizontal = new Vector2(1, 1);
    [HideInInspector]
    public Vector2 expandValues_crosshair;
    private float fadeout_value = 1;

    [Header("Sounds")]
    public AudioSource shoot_sound_source, reloadSound_source;
    public static AudioSource hitMarker;

    [Header("Reference")]
    public CameraHandler m_CameraHandler;
    public GameObject weaponSwayObject;

    [Header("Sight")]
    public Transform sightTarget;
    public float sightOffset;
    public float aimingInTime;
    private Vector3 weaponSwayPosition;
    private Vector3 weaponSwayPositionVelocity;

    public override void Awake()
    {
        player = GetComponentInParent<PlayerManager>();
    }



    public void Update()
    {
        waitTillNextFire -= roundsPerSecond * Time.deltaTime;

        CalculateRecoil();
        CalculateAimingIn();
    }

    public override void WeaponPrimaryAction()
    {
       // if (currentStyle == GunStyles.nonautomatic)
        {
            if (player.inputHandler.m_TapFire1_Input)
            {
                ShootMethod();
            }
        }
        //if (currentStyle == GunStyles.automatic)
        {
            if (player.inputHandler.m_HoldFire1_Input)
            {
                ShootMethod();
            }
        }
    }

    private void ShootMethod()
    {
        if (waitTillNextFire <= 0 && !player.isReloading && player.isRunning == false)
        {

            if (bulletsInTheGun > 0)
            {

                int randomNumberForMuzzelFlash = Random.Range(0, 5);
                Instantiate(bullet, bulletSpawnPlace.transform.position, bulletSpawnPlace.transform.rotation);

                holdFlash = Instantiate(muzzelFlash[randomNumberForMuzzelFlash], muzzelSpawn.transform.position /*- muzzelPosition*/, muzzelSpawn.transform.rotation * Quaternion.Euler(0, 0, 90)) as GameObject;
                holdFlash.transform.parent = muzzelSpawn.transform;
                if (shoot_sound_source)
                    shoot_sound_source.Play();
                else
                    print("Missing 'Shoot Sound Source'.");

                RecoilFire();

                waitTillNextFire = 1;
                bulletsInTheGun -= 1;
            }

            else
            {

                Debug.Log("¿Â¿¸¡ﬂ");
                //if(!aiming)
                //StartCoroutine(Reload_Animation());
                //if(emptyClip_sound_source)
                //	emptyClip_sound_source.Play();
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
        throw new System.NotImplementedException();
    }

    public void CalculateAimingIn()
    {
        var targetPosition = transform.position;

        if(player.isAiming)
        {
            targetPosition = player.cameraHandler.transform.position + (weaponSwayObject.transform.position - sightTarget.position) + player.cameraHandler.transform.forward * sightOffset;
        }

        weaponSwayPosition = weaponSwayObject.transform.position;
        weaponSwayPosition = Vector3.SmoothDamp(weaponSwayPosition, targetPosition, ref weaponSwayPositionVelocity, aimingInTime);
        weaponSwayObject.transform.position = weaponSwayPosition;
    }
}
