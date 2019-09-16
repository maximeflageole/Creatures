using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageUI : MonoBehaviour
{
    [SerializeField]
    protected List<DamageColorAndType> m_damageColorsAndTypes;
    [SerializeField]
    protected List<int> m_fontSizes;
    [SerializeField]
    protected TextMeshPro m_textMesh;
    public float m_timeToDisplay;
    protected float m_timeDisplayed;
    public AnimationCurve m_animCurveY;
    public AnimationCurve m_animCurveX;
    public float m_randomXPos;
    public float m_maxHeight;
    public float m_maxXPos;

    public void DisplayDamage(int amount, Turquoise.EDamageIntensity intensity)
    {
        m_randomXPos = Random.Range(-1.0f, 1.0f);
        int fontSize = 0;
        DamageColorAndType damageColorAndType = m_damageColorsAndTypes[0];
        switch (intensity)
        {
            case Turquoise.EDamageIntensity.Normal:
                fontSize = m_fontSizes[0];
                damageColorAndType = m_damageColorsAndTypes[0];
                break;
            case Turquoise.EDamageIntensity.Crit:
                fontSize = m_fontSizes[1];
                damageColorAndType = m_damageColorsAndTypes[1];
                break;
            case Turquoise.EDamageIntensity.Reduced:
                fontSize = m_fontSizes[2];
                damageColorAndType = m_damageColorsAndTypes[2];
                break;
            case Turquoise.EDamageIntensity.SuperEffective:
                fontSize = m_fontSizes[3];
                damageColorAndType = m_damageColorsAndTypes[3];
                break;
            case Turquoise.EDamageIntensity.Heal:
                fontSize = m_fontSizes[4];
                damageColorAndType = m_damageColorsAndTypes[4];
                amount *= -1;
                break;
        }
        m_textMesh.fontSize = fontSize;
        m_textMesh.color = damageColorAndType.color;
        m_textMesh.text = amount.ToString();
    }

    public void Update()
    {
        if (m_timeDisplayed > m_timeToDisplay)
        {
            Destroy(gameObject);
        }
        float evaluationY = m_animCurveY.Evaluate(m_timeDisplayed / m_timeToDisplay);
        float evaluationX = m_animCurveX.Evaluate(m_timeDisplayed / m_timeToDisplay);
        transform.localPosition = new Vector3(evaluationX * m_randomXPos * m_maxXPos, evaluationY * m_maxHeight, transform.localPosition.z);
        m_timeDisplayed += Time.deltaTime;
    }
}

[System.Serializable]
public struct DamageColorAndType
{
    public Color color;
    public Turquoise.EDamageType damageType;
}

namespace Turquoise
{
    public enum EDamageIntensity
    {
        Reduced,
        Normal,
        SuperEffective,
        Crit,
        Heal,
        Count
    }
}