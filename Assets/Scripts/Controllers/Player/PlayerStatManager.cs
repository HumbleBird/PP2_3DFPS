using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerStatManager : MonoBehaviour
{
    PlayerManager player;

    public int m_iMaxHp;
    public int m_iCurrentHP;
    public int m_iAttackDamage;
    public int m_iDefense;

    private void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        m_iCurrentHP = m_iMaxHp;
    }

    public void HealthBarUIUpdate(int damage)
    {
        // TODO
    }
}
