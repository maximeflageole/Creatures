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
    public GameObject m_damageTextPrefab;
    public float m_healthLagDuration;
    protected float m_healthLagCurrentDuration;
    public AnimationCurve m_healthLagAnim;
    public float m_previousHealth;
    protected bool m_calculateHealth = true;

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
        if (m_maskTransform != null && m_calculateHealth)
        {
            CalculateHealthLag(health, maxHealth);
        }
    }

    public void CalculateHealthLag(int health, int maxHealth)
    {
        if (m_healthLagCurrentDuration > m_healthLagDuration)
        {
            m_calculateHealth = false;
            m_healthLagCurrentDuration = 0;
            m_previousHealth = ((float)health / (float)maxHealth);
            return;
        }
        else
        {
            float pos = m_healthLagAnim.Evaluate(m_healthLagCurrentDuration / m_healthLagDuration);
            float healthPercent = ((float)health / (float)maxHealth);
            float calculatedPos = ((1-pos) * m_previousHealth) + (healthPercent * (pos));
            m_maskTransform.localPosition = new Vector3(calculatedPos * m_xMaskValue, m_maskTransform.localPosition.y, m_maskTransform.localPosition.z);
        }
        m_healthLagCurrentDuration += Time.deltaTime;
    }

    public void ReceiveDamage(int amount, Turquoise.EDamageIntensity intensity)
    {
        m_calculateHealth = true;
        DamageUI damageUi = Instantiate(m_damageTextPrefab, transform).GetComponent<DamageUI>();
        damageUi.DisplayDamage(amount, intensity);
        m_healthLagCurrentDuration = 0;
    }
}
