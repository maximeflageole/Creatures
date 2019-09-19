using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPanelUI : MonoBehaviour
{
    [SerializeField]
    protected Transform m_viewportTransform;
    [SerializeField]
    protected List<CardPanelUI> m_child;
    [SerializeField]
    protected GameObject m_cardPanelUIPrefab;

    public void DisplayCards(List<CardData> cardDatas)
    {
        if (gameObject.activeSelf)
        {
            Reset();
            return;
        }
        Reset();
        gameObject.SetActive(true);
        foreach (var data in cardDatas)
        {
            CardPanelUI cardPanel = Instantiate(m_cardPanelUIPrefab, m_viewportTransform).GetComponent<CardPanelUI>();
            cardPanel.AssignCardData(data, true);
            m_child.Add(cardPanel);
        }
    }

    private void Reset()
    {
        gameObject.SetActive(false);
        foreach (var child in m_child)
        {
            Destroy(child.gameObject);
        }
        m_child.Clear();
    }
}
