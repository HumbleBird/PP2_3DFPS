using UnityEngine;
using System.Collections;
using static Define;

public class Bullet : MonoBehaviour 
{
	public float LifeTime;
	public Rigidbody m_Rigidbody;

	public float m_fBulletDistance;

    public LayerMask targetLayer;
	private TrailRenderer m_BulletTrail;
	private ParticleSystem m_ParticleSystem;

    public PlayerManager Owner;
    public Gun gun;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        Destroy(gameObject, LifeTime); // TODO Pool

    }


    void Update () 
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerManager enemyManager = collision.gameObject.GetComponent<PlayerManager>();
            if (enemyManager == Owner)
                return;

            // 무기 콜라이더가 어디 부분에서 처음 부딪치는지 탐지
            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            float angleHitFrom = Vector3.SignedAngle(Owner.transform.forward, enemyManager.transform.forward, Vector3.up);

            string whereHit = collision.gameObject.tag;

            // 딜 넣기
            TakeDamageEffect takeDamageEffect = new TakeDamageEffect();
            // 어디를 때렸는지 확인
            if(whereHit != "Untagged")
                takeDamageEffect.m_E_hitDetection = (E_HitDetection)System.Enum.Parse(typeof(E_HitDetection), whereHit); // 나중에 렉이 걸리는지 확일 할 것.
            takeDamageEffect.m_DamagerPlayer = Owner;
            takeDamageEffect.m_SacrificePlayer = enemyManager;
            takeDamageEffect.contactPoint = contactPoint;
            takeDamageEffect.angleHitFrom = angleHitFrom;
            takeDamageEffect.m_iDamage = gun.m_iDamage;
            takeDamageEffect.DamageEffect();

            Destroy(gameObject); // TODO Pool
        }
    }

}
