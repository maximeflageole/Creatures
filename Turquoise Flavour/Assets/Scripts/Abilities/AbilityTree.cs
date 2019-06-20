using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTree : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_levelUpPrefab;
    [SerializeField]
    protected List<LevelUpAbility> m_levelUpAbilities;
    public CreatureData m_creatureData;
    [SerializeField]
    protected float m_columnSpacing;

    public void CreateAbilityTree(CreatureData creatureData)
    {
        CreateUI(creatureData.abilityTree.abilities.Count);
        for (int i = 0; i < creatureData.abilityTree.abilities.Count && i < m_levelUpAbilities.Count; i++)
        {
            /*
            LevelUpAbilityData data = creatureData.abilityTree.abilities[i];
            m_levelUpAbilities[i].m_levelUpText.text = "lvl " + (i + 1);

            Button btn = m_levelUpAbilities[i].m_levelUpButton;
            btn.image.sprite = data.sprite;
            btn.GetComponentInChildren<Text>().text = data.text;
            */
        }
    }

    public void CreateUI(int count)
    {
        //Make sure previous abilities are removed from UI and memory
        foreach (var ability in m_levelUpAbilities)
        {
            Destroy(ability.gameObject);
        }
        m_levelUpAbilities.Clear();
        for(int i = 0; i < count; i++)
        {
            var ability = Instantiate(m_levelUpPrefab, transform);
            ability.transform.localPosition += new Vector3(m_columnSpacing * i, -15f, 0f);
            m_levelUpAbilities.Add(ability.GetComponent<LevelUpAbility>());
        }
    }

    public void Start()
    {
        //TODO: Remove
        CreateAbilityTree(m_creatureData);
    }
}