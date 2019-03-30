using System.Collections;
using System.Collections.Generic;
using Turquoise;
using UnityEngine;

namespace Turquoise
{
    public enum ETeams
    {
        Player,
        Enemy,
        Wild,
        Ally,
        None
    }
}

public class Creature : MonoBehaviour
{
    [SerializeField]
    protected ETeams m_team = ETeams.None;
    [SerializeField]
    protected Creatures.CreaturesType m_primaryType;
    [SerializeField]
    protected float m_health = 100.0f;
    [SerializeField]
    protected float m_maxHealth = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
