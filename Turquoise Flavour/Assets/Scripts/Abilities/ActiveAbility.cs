using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbility : MonoBehaviour
{
    [SerializeField]
    protected bool m_used;
    [SerializeField]
    protected ActiveAbilityData m_abilityData;

    public void LoadAbility(ActiveAbilityData data)
    {
        m_abilityData = data;
    }

    public void LoadAbilityUI(ActiveAbilityUI abilityUI)
    {
        abilityUI.LoadAbility(m_abilityData);
    }
}