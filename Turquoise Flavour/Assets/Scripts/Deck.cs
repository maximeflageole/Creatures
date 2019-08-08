using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck : MonoBehaviour
{
    public List<Turquoise.ECard> m_cards = new List<Turquoise.ECard>();

    public bool AddCard(Turquoise.ECard card)
    {
        m_cards.Add(card);
        return true;
    }
    public bool RemoveCard(Turquoise.ECard card)
    {
        if (m_cards.Contains(card))
        {
            m_cards.Remove(card);
            return true;
        }
        return false;
    }
}