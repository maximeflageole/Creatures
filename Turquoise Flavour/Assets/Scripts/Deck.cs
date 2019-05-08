using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Deck : MonoBehaviour
{
    public List<Cards.ECard> m_cards;

    public void SaveGame()
    {
        SaveSystem.SaveGame();
    }

    public void LoadGame()
    {
        m_cards.Clear();
        print("DeckLoading");
        SaveData data = SaveSystem.LoadGame();
        if (data != null)
        {
            /*
            foreach (var cardStr in data.deck)
            {
                Cards.ECard card = Cards.ECard.Count;
                if (System.Enum.TryParse(cardStr, true, out card))
                {
                    m_cards.Add(card);
                }
            }
            */
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