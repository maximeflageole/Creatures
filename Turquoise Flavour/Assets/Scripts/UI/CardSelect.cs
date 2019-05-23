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
    protected int m_amountToDiscard;
    [SerializeField]
    protected bool m_isActive = false;
    [SerializeField]
    protected Vector3 m_anchor;
    [SerializeField]
    protected TextMeshPro m_text;

    // Update is called once per frame
    void Update()
    {
        DiscardButton();
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
            card.transform.position = m_anchor;
        }
    }

    public void AddCard(Card card)
    {
        if (m_selectedCards.Count < m_amountToDiscard)
        {
            m_selectedCards.Add(card);
            card.transform.position = m_anchor;
        }
    }

    protected void Discard()
    {
        if ((m_selectedCards.Count == m_amountToDiscard) || CardEffects.GetCardEffectsInstance().CardHandCount() == 0)
        {
            Debug.Log("Discarding X card");
            CardEffects.GetCardEffectsInstance().DiscardCardList(m_selectedCards);
            m_selectedCards.Clear();
        }
        gameObject.SetActive(false);
    }

    protected void DiscardButton()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.GetComponent<BoxCollider2D>() == m_boxCollider)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Discarding");
                Discard();
            }
        }
    }

    public void StartDiscarding(int amount)
    {
        m_amountToDiscard = amount;
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
}
