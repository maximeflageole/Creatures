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

    //Damage text
    public GameObject m_damageTextPrefab;
    public float m_healthLagDuration;
    protected float m_healthLagCurrentDuration;
    public AnimationCurve m_healthLagAnim;
    protected float m_previousHealth;
    protected bool m_calculateHealth = true;

    //Attack animations
    public AnimationCurve m_attackCurve;
    public float m_attackDuration;
    protected float m_attackCurrentDuration = 100;
    protected List<Turquoise.ECardGenre> m_animationStack = new List<Turquoise.ECardGenre>();
    protected bool m_playingAnim;
    protected Turquoise.ECardGenre m_currentAnim = Turquoise.ECardGenre.Count;
    protected float m_initialXposition;

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
        bool mirror = m_team == Turquoise.ETeams.Enemy;
        PlayAnimations(mirror);
    }

    void PlayAnimations(bool mirror)
    {
        if (m_animationStack.Count != 0 && m_playingAnim == false)
        {
            m_currentAnim = m_animationStack[0];
            m_animationStack.RemoveAt(0);
            m_playingAnim = true;
            m_attackCurrentDuration = 0;
        }
        if (m_attackCurrentDuration > m_attackDuration)
        {
            m_playingAnim = false;
            m_currentAnim = Turquoise.ECardGenre.Count;
        }
        if (m_playingAnim)
        {
            if (m_currentAnim == Turquoise.ECardGenre.Attack)
            {
                float value = m_attackCurve.Evaluate(m_attackCurrentDuration / m_attackDuration) * 3.0f;
                if (mirror)
                    value *= -1;
                transform.localPosition = new Vector3(m_initialXposition + value, transform.localPosition.y, transform.localPosition.z);
                m_attackCurrentDuration += Time.deltaTime;
            }
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

    public void PlayAnimation(Turquoise.ECardGenre animationType)
    {
        m_animationStack.Add(animationType);
    }

    void Start()
    {
        m_initialXposition = transform.position.x;
    }
}
