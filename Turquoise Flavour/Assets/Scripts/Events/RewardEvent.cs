using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardEvent : TurquoiseEvent
{
    [SerializeField]
    protected List<CardData> cardsData;
    [SerializeField]
    protected List<Card> cardRewards;

    void Update()
    {
        CheckMouseClickCard();
    }

    void Start()
    {
        if (cardsData.Count != cardRewards.Count)
        {
            print("Not as many card data as card prefabs");
            return;
        }
        for (int i = 0; i < cardsData.Count; i++)
        {
            cardRewards[i].SetCardData(cardsData[i]);
        }
    }
    // This function is called to check if the mouse has clicked on a card
    void CheckMouseClickCard()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Card cardHit = hit.collider.GetComponent<Card>();
                if (cardHit == null)
                {
                    return;
                }
                AddCardToDeck(cardHit);
            }
        }
    }

    protected void AddCardToDeck(Card card)
    {
        Player.GetPlayerInstance().AddCardToCreatureDeck(card.GetCardData().cardEnumValue);
        DisableCards();
        GameMaster.GetInstance().EndCurrentEvent(true);
    }

    protected void DisableCards()
    {
        foreach (var card in cardRewards)
        {
            card.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
