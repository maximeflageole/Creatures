using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTree : MonoBehaviour
{
    [SerializeField]
    protected List<LevelUpAbility> m_levelUpAbilities;
    public CreatureData m_creatureData;
    public void CreateAbilityTree(CreatureData creatureData)
    {
        for (int i = 0; i < creatureData.abilityTree.levelUpData.Count && i < m_levelUpAbilities.Count; i++)
        {
            LevelUpData data = creatureData.abilityTree.levelUpData[i];
            m_levelUpAbilities[i].m_levelUpText.text = "lvl " + data.level;
            for (int j = 0; j < m_levelUpAbilities[i].m_levelUpButtons.Count; j++)
            {
                Button btn = m_levelUpAbilities[i].m_levelUpButtons[j];
                btn.image.sprite = data.levelAbilities[j].sprite;
            }
        }
    }

    public void Start()
    {
        CreateAbilityTree(m_creatureData);
    }
}