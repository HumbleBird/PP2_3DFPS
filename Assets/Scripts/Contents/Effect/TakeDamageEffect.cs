using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Define;

public class TakeDamageEffect : ScriptableObject
{
    [Header("Player Causing Damage")]
    public PlayerManager m_DamagerPlayer; // 데미지 유발이 캐릭터라면, they are listed here?
    public PlayerManager m_SacrificePlayer; // 피격자

    [Header("Damage")]
    int m_iFinalDamage;
    public int m_iDamage;
    public E_HitDetection m_E_hitDetection;

    [Header("Animation")]
    public bool playDamageAnimation = true;
    public bool manuallySelectDamageAnimation = false;
    public string damageAnimation;

    [Header("SFX")]
    public AudioClip m_DamageAudioClip; // ELEMENTAL(Fire, Magic, Darks, Lightning) Damage 일 떄 플레이

    [Header("Direction Damage Taken From")]
    public float angleHitFrom;
    public Vector3 contactPoint; // 캐릭터 바디 데미지 발생 지점

    public void DamageEffect()
    {
        if (m_SacrificePlayer.isDead)
            return;

        // 데미지 계산
        CalculateDamage();

        // 방향에 따른 데미지 애니메이션
        CheckWhichDirectionDamageCameFrom();

        // 데미지 애니메이션 재생
        PlayDamageAnimation();

        // UI 업데이트
        PlayerHelathBarUpdate();

        // 사운드
        PlayDamageSoundFX();

        // 피 효과
        PlayBloodSplatter();

        // character가 Player라면, 카메라 쉐이크 효과를 줌.
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
                Debug.Log("데미지 부위를 설정해주세요");
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
            //player.characterWeaponSlotManager.CloseDamageCollider(); // 근접 무기
            m_SacrificePlayer.playerAnimatorManager.PlayTargetAnimation("Dead_01", true); // TODO RegDoll
        }
        else
        {

            // 활성/비활성 stun lock
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

        // 피격 효과음

    }

    private void PlayBloodSplatter()
    {
        // 벽에 피 튀김
    }
}
