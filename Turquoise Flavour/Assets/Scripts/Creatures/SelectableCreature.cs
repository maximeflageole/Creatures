using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

public class SelectableCreature : MonoBehaviour
{
    public ECreature m_eCreature;
    public CreatureData m_creatureData;

    public void Initialize(ECreature eCreature)
    {
        m_eCreature = eCreature;
        m_creatureData = GameMaster.GetInstance().m_creatureList.GetCreatureDataFromCreatureName(eCreature);
        GetComponent<SpriteRenderer>().sprite = m_creatureData.sprite;
    }
}
