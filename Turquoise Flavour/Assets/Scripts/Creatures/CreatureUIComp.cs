using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatureUIComp : MonoBehaviour
{
    public TextMeshPro m_manaTextMesh;
    public TextMesh m_healthText;
    public TextMesh m_armorText;
    public TextMesh m_ConditionsText;
    public TextMesh m_levelText;
    public TextMesh m_experienceText;
    public Turquoise.ETeams m_team;

    public void UpdateUI(int health, int maxHealth, int armor, int mana, int baseMana, int level, int experience, int nextLvlExp)
    {
        if (m_healthText != null)
        {
            m_healthText.text = health.ToString() + " / " + maxHealth.ToString();
        }
        if (m_armorText != null)
        {
            m_armorText.gameObject.SetActive(armor != 0);
            m_armorText.text = armor.ToString();
        }
        if (m_manaTextMesh != null)
        {
            m_manaTextMesh.text = mana + "/" + baseMana;
        }
        if (m_levelText != null)
        {
            m_levelText.text = "Lvl: " + level;
        }
        if (m_experienceText != null)
        {
            m_experienceText.text = "Exp: " + experience + " / " + nextLvlExp;
        }
    }
}
