using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Define;

public class TakeDamageEffect : ScriptableObject
{
    [Header("Player Causing Damage")]
    public PlayerManager m_DamagerPlayer; // ������ ������ ĳ���Ͷ��, they are listed here?
    public PlayerManager m_SacrificePlayer; // �ǰ���

    [Header("Damage")]
    int m_iFinalDamage;
    public int m_iDamage;
    public E_HitDetection m_E_hitDetection;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("SFX")]
    public AudioClip m_DamageAudioClip; // ELEMENTAL(Fire, Magic, Darks, Lightning) Damage �� �� �÷���

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint; // ĳ���� �ٵ� ������ �߻� ����

    public void DamageEffect()
    {
        if (m_SacrificePlayer.isDead)
            return;

        // ������ ���
        CalculateDamage();

        // ���⿡ ���� ������ �ִϸ��̼�
        CheckWhichDirectionDamageCameFrom();

        // ������ �ִϸ��̼� ���
        PlayDamageAnimation();

        // UI ������Ʈ
        PlayerHelathBarUpdate();

        // ����
        PlayDamageSoundFX();

        // �� ȿ��
        PlayBloodSplatter();

        // character�� Player���, ī�޶� ����ũ ȿ���� ��.
        //SetCameraShake(player);
    }

    public void CalculateDamage()
    {
        float scope = 0;

        switch (m_E_hitDetection)
        {
            case E_HitDetection.Head:
                scope = 1.5f;
                break;
            case E_HitDetection.Body:
                scope = 1.0f;
                break;
            case E_HitDetection.Arm:
            case E_HitDetection.Leg:
                scope = 0.5f;

                break;
            default:
                scope = 1.0f;
                Debug.Log("������ ������ �������ּ���");
                break;
        }

        m_iFinalDamage = Mathf.FloorToInt((m_iDamage - m_SacrificePlayer.playerStatManager.m_iDefense) * scope);

        if (m_iFinalDamage < 0)
            m_iFinalDamage = 0;


        m_SacrificePlayer.playerStatManager.m_iCurrentHP = Mathf.Max(m_SacrificePlayer.playerStatManager.m_iCurrentHP - m_iFinalDamage, 0);

        if (m_SacrificePlayer.playerStatManager.m_iCurrentHP <= 0)
        {
            m_SacrificePlayer.OnDead();
        }

        Debug.Log(m_SacrificePlayer.playerStatManager.m_iCurrentHP);
    }

    private void CheckWhichDirectionDamageCameFrom()
    {
        if (angleHitFrom >= 145 && angleHitFrom <= 180)
        {
            damageAnimation = "Hit_Frontward";
            return;
        }
        else if (angleHitFrom <= -145 && angleHitFrom >= -180)
        {
            damageAnimation = "Hit_Frontward";
            return;

        }
        else if (angleHitFrom >= -45 && angleHitFrom <= 45)
        {
            damageAnimation = "Hit_Backward";
            return;

        }
        else if (angleHitFrom >= -144 && angleHitFrom <= -45)
        {
            damageAnimation = "Hit_Left";
            return;

        }
        else if (angleHitFrom >= 45 && angleHitFrom <= 144)
        {
            damageAnimation = "Hit_Right";
            return;
        }
    }

    private void PlayDamageAnimation()
    {
        if (m_SacrificePlayer.isDead)
        {
            //player.characterWeaponSlotManager.CloseDamageCollider(); // ���� ����
            m_SacrificePlayer.playerAnimatorManager.PlayTargetAnimation("Dead_01", true); // TODO RegDoll
        }
        else
        {

            // Ȱ��/��Ȱ�� stun lock
            if (playDamageAnimation)
            {
                m_SacrificePlayer.playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);
            }
        }

    }

    private void PlayerHelathBarUpdate()
    {
        m_SacrificePlayer.playerStatManager.HealthBarUIUpdate(m_iFinalDamage);
    }

    private void PlayDamageSoundFX()
    {
        // Character Voice
        Managers.Sound.Play(m_DamageAudioClip);

        // �ǰ� ȿ����

    }

    private void PlayBloodSplatter()
    {
        // ���� �� Ƣ��
    }
}
