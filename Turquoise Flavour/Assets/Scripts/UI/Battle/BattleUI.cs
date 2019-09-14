using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public TextMeshProUGUI m_textMeshPro;
    public AnimationCurve m_anim;
    public AnimationCurve m_alphaAnim;
    public float m_animDuration;
    protected float m_animCurrentDuration;
    public static BattleUI m_instance;

    public void ChangeTurn(string text)
    {
        m_textMeshPro.text = text;
        m_animCurrentDuration = 0;
        m_textMeshPro.rectTransform.localScale.Set(1, 1, 1);
    }

    public void Start()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_animCurrentDuration > m_animDuration)
        {
            return;
        }
        else
        {
            float value = 1 + m_anim.Evaluate(m_animCurrentDuration / m_animDuration);
            m_textMeshPro.rectTransform.localScale = new Vector3(value, value, value);
            m_textMeshPro.alpha = m_alphaAnim.Evaluate(m_animCurrentDuration / m_animDuration);
        }
        m_animCurrentDuration += Time.deltaTime;
    }
}
