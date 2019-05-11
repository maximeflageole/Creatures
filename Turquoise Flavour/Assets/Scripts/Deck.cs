using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck : MonoBehaviour
{
    public List<Cards.ECard> m_cards = new List<Cards.ECard>();

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public bool AddCard(Cards.ECard card)
    {
        m_cards.Add(card);
        SaveGame();
        return true;
    }
}