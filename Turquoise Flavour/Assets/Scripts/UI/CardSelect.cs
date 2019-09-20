using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardSelect : MonoBehaviour
{
    [SerializeField]
    protected BoxCollider2D m_boxCollider;
    [SerializeField]
    protected List<Card> m_selectedCards;
    [SerializeField]
    protected int m_amountToSelect;
    [SerializeField]
    protected bool m_isActive = false;
    [SerializeField]
    protected Vector3 m_anchor;
    [SerializeField]
    protected TextMeshPro m_text;
    [SerializeField]
    protected ESelectionType m_selectionType;

    // Update is called once per frame
    void Update()
    {
        SelectButton();
        if (!m_isActive)
        {
            return;
        }
        if (m_selectedCards.Count == 0)
        {
            return;
        }
        foreach (var card in m_selectedCards)
        {
            card.transform.position = m_anchor + new Vector3(2.0f * m_selectedCards.IndexOf(card), 0.0f, 0.0f);
        }
    }

    public void SelectCard(Card card)
    {
        if (m_selectedCards.Contains(card))
        {
            m_selectedCards.Remove(card);
            card.SetCardInSelection(false);
        }
        else if (m_selectedCards.Count < m_amountToSelect)
        {
            if (m_selectedCards.Contains(card))
            {
                return;
            }
            m_selectedCards.Add(card);
            card.SetCardInSelection(true);
        }
    }

    protected void Select()
    {
        if ((m_selectedCards.Count == m_amountToSelect) || CardEffects.GetInstance().CardHandCount() == m_selectedCards.Count)
        {
            CardEffects.GetInstance().SelectCardList(m_selectedCards);
            foreach (var card in m_selectedCards)
            {
                card.SetCardInSelection(false);
            }
            m_selectedCards.Clear();
            gameObject.SetActive(false);
        }
    }

    protected void SelectButton()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.GetComponent<BoxCollider2D>() == m_boxCollider)
        {
            if (Input.GetMouseButtonUp(0))
            {
                switch (m_selectionType)
                {
                    case ESelectionType.Discarding:
                        Debug.Log("Discarding");
                        Select();
                        break;
                    case ESelectionType.Exhausting:
                        Debug.Log("Exhausting");
                        Select();
                        break;
                    case ESelectionType.Other:
                        Debug.Log("Selecting");
                        Select();
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public void StartDiscarding(int amount)
    {
        m_selectionType = ESelectionType.Discarding;
        m_amountToSelect = amount;
        m_selectedCards.Clear();
        if (amount == 1)
        {
            m_text.text = "Discard " + amount + " card";
        }
        else
        {
            m_text.text = "Discard " + amount + " cards";
        }
    }

    public void StartExhausting(int amount)
    {
        m_selectionType = ESelectionType.Exhausting;
        m_amountToSelect = amount;
        m_selectedCards.Clear();
        if (amount == 1)
        {
            m_text.text = "Exhaust " + amount + " card";
        }
        else
        {
            m_text.text = "Exhaust " + amount + " cards";
        }
    }

    public void StartSelecting(int amount)
    {
        m_selectionType = ESelectionType.Other;
        m_amountToSelect = amount;
        m_selectedCards.Clear();
        if (amount == 1)
        {
            m_text.text = "Select " + amount + " card";
        }
        else
        {
            m_text.text = "Select " + amount + " cards";
        }
    }
}

public enum ESelectionType
{
    Discarding,
    Exhausting,
    Other,
    None
}