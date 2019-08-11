using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CreatureUIComp : MonoBehaviour
{
    public TextMeshPro m_manaTextMesh;
    public TextMesh m_healthText;
    public TextMesh m_ConditionsText;
    public TextMesh m_levelText;
    public TextMesh m_experienceText;
    public Turquoise.ETeams m_team;
    public BoonsUI m_boonsUI;
    public float m_xMaskValue;
    public Transform m_maskTransform;

    public void UpdateUI(int health, int maxHealth, int mana, int baseMana, int level, int experience, int nextLvlExp, List<Condition> conditions)
    {
        if (m_healthText != null)
        {
            m_healthText.text = health.ToString() + " / " + maxHealth.ToString();
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
        if (m_boonsUI != null)
        {
            m_boonsUI.UpdateUI(conditions);
        }
        if (m_maskTransform != null)
        {
            float healthPercent = (float)health / (float)maxHealth;
            healthPercent = Mathf.Clamp(healthPercent, 0.0f, 1.0f);
            m_maskTransform.localPosition = new Vector3(m_xMaskValue * healthPercent, m_maskTransform.localPosition.y, m_maskTransform.localPosition.z);
        }
    }
}
