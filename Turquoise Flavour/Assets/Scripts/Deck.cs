using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck : MonoBehaviour
{
    public List<Cards.ECard> m_cards = new List<Cards.ECard>();

    public bool AddCard(Cards.ECard card)
    {
        m_cards.Add(card);
        return true;
    }
}