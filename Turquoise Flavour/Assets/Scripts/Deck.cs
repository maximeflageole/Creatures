using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Cards.ECard> m_cards = new List<Cards.ECard>();

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        m_cards.Clear();
        print("DeckLoading");
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            foreach (var cardStr in data.deck)
            {
                Cards.ECard card = Cards.ECard.Count;
                if (System.Enum.TryParse(cardStr, true, out card))
                {
                    m_cards.Add(card);
                }
            }
        }
        else Debug.Log("SaveSystem.LoadGame():There is no save file for this deck");
    }

    public bool AddCard(Cards.ECard card)
    {
        m_cards.Add(card);
        SaveGame();
        return true;
    }
}