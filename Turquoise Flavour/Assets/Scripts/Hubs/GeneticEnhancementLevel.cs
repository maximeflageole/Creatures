using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneticEnhancementLevel : MonoBehaviour
{
    public Color m_selectedColor;
    public Color m_neutralColor;
    public int m_level;
    public Image m_leftImage;
    public Image m_rightImage;
    public Image m_leftPanel;
    public Image m_rightPanel;
    public TextMeshProUGUI m_leftText;
    public TextMeshProUGUI m_rightText;

    public void OnInstantiate(sEvolutionLevel level)
    {
        m_leftImage.sprite = level.nodeLeft.sprite;
        m_leftText.text = level.nodeLeft.description;
        m_rightImage.sprite = level.nodeRight.sprite;
        m_rightText.text = level.nodeRight.description;
    }

    public void OnClick(bool left)
    {
        GetComponentInParent<GeneticEnhancementPanel>().OnSelectEvolution(m_level, left);
        if (left)
        {
            m_leftPanel.color = m_selectedColor;
            m_rightPanel.color = m_neutralColor;
        }
        else
        {
            m_rightPanel.color = m_selectedColor;
            m_leftPanel.color = m_neutralColor;
        }
    }
}
