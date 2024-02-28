using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayerWeaponManager : MonoBehaviour
{
    PlayerManager player;

    [Header("Ref Transform")]
    public Transform m_WeaponHolder;
    public Transform m_WeaponPivot;

    [Header("Weapon")]
    public Weapon m_CurrentWeapon;
    public List<Weapon> m_PrimaryWeapons; // 주 무기
    public List<Weapon> m_SecondWeapons; // 보조 무기
    public List<Weapon> m_MeleeWeapons;
    public List<Weapon> m_ThrowWeapons;
    private int m_CurrentInt_PrimaryWeapon;
    private int m_CurrentInt_SecondWeapon;
    private int m_CurrentInt_MeleeWeapon;
    private int m_CurrentInt_ThrowWeapon;




    [Header("Hand IK Targets")]
    public RightHandIKTarget rightHandIKTarget;
    public LeftHandIKTarget leftHandIKTarget;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {

    }

    public virtual void LoadTwoHandIKTargtets(bool isTwoHandingWeapon)
    {
        leftHandIKTarget = transform.GetComponentInChildren<LeftHandIKTarget>();
        rightHandIKTarget = transform.GetComponentInChildren<RightHandIKTarget>();

        if (leftHandIKTarget == null || rightHandIKTarget == null)
            return;

        player.playerAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
    }

    public void ReloadComplete()
    {
        LoadTwoHandIKTargtets(true);
        ((Gun)m_CurrentWeapon).ReloadComplete();
    }

    #region TEMP


    [Header("Item Change")]
    [SerializeField] public Animator ani;
    [SerializeField] Image ItemCanvasLogo;
    [SerializeField] bool LoopItems = true;
    [SerializeField, Tooltip("You can add your new item here.")] GameObject[] Items;
    [SerializeField, Tooltip("These logos must have the same order as the items.")] Sprite[] ItemLogos;

    [SerializeField] int ItemIdInt;
    int MaxItems;
    int ChangeItemInt;
    [HideInInspector] public bool DefiniteHide;
    bool ItemChangeLogo;

    private Camera cameraComponent;
    private Transform gunPlaceHolder;

    private void Start2()
    {
        //Color OpacityColor = ItemCanvasLogo.color;
        //OpacityColor.a = 0;
        //ItemCanvasLogo.color = OpacityColor;
        //ItemChangeLogo = false;
        //DefiniteHide = false;
        //ChangeItemInt = ItemIdInt;
        //ItemCanvasLogo.sprite = ItemLogos[ItemIdInt];
        //MaxItems = Items.Length - 1;
        //StartCoroutine(ItemChangeObject());
    }

    void FixedUpdate2()
    {
        //if (ItemChangeLogo)
        //{
        //    Color OpacityColor = ItemCanvasLogo.color;
        //    OpacityColor.a = Mathf.Lerp(OpacityColor.a, 0, 20 * Time.deltaTime);
        //    ItemCanvasLogo.color = OpacityColor;
        //}
        //else
        //{
        //    Color OpacityColor = ItemCanvasLogo.color;
        //    OpacityColor.a = Mathf.Lerp(OpacityColor.a, 1, 6 * Time.deltaTime);
        //    ItemCanvasLogo.color = OpacityColor;
        //}
    }

    private void ItemChange()
    {
        if (player.inputHandler.mouseWheel > 0.1f)
        {
            player.inputHandler.mouseWheel = 0;
            ItemIdInt++;

        }

        if (player.inputHandler.mouseWheel < -0.1f)
        {
            player.inputHandler.mouseWheel = 0;
            ItemIdInt--;
        }

        if (player.inputHandler.m_Hide_Input)
        {
            player.inputHandler.m_Hide_Input = false;

            if (ani.GetBool("Hide"))
                Hide(false);
            else
                Hide(true);
        }

        if (ItemIdInt < 0) ItemIdInt = LoopItems ? MaxItems : 0;
        if (ItemIdInt > MaxItems) ItemIdInt = LoopItems ? 0 : MaxItems;


        if (ItemIdInt != ChangeItemInt)
        {
            ChangeItemInt = ItemIdInt;
            StartCoroutine(ItemChangeObject());
        }
    }

    public void Hide(bool Hide)
    {
        DefiniteHide = Hide;
        //ani.SetBool("Hide", Hide);
    }

    IEnumerator ItemChangeObject()
    {
        if (!DefiniteHide) ani.SetBool("Hide", true);
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < (MaxItems + 1); i++)
        {
            Items[i].SetActive(false);
        }
        Items[ItemIdInt].SetActive(true);
        if (!ItemChangeLogo) StartCoroutine(ItemLogoChange());

        if (!DefiniteHide) ani.SetBool("Hide", false);
    }

    IEnumerator ItemLogoChange()
    {
        ItemChangeLogo = true;
        yield return new WaitForSeconds(0.5f);
        ItemCanvasLogo.sprite = ItemLogos[ItemIdInt];
        yield return new WaitForSeconds(0.1f);
        ItemChangeLogo = false;
    }

    #endregion



}
