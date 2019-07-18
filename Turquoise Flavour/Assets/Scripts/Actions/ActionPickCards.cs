using System.Collections;
using System.Collections.Generic;
using Turquoise;
using UnityEngine;

public class ActionPickCards : Action
{
    List<Card> m_drawPile = new List<Card>();
    List<ECard> m_foundCards = new List<ECard>();
    int m_cardsToPick;
    int m_cardToPickFrom;

    // Start is called before the first frame update
    public override void StartAction()
    {
        base.StartAction();
        CardEffects.GetInstance().m_isSelectingCards = true;

        List<Card> cardList = CardEffects.GetInstance().GetDrawPile().Peek(m_cardToPickFrom);
        for (int j = 0; j < cardList.Count; j++)
        {
            m_foundCards.Add(cardList[j].GetCardData().cardEnumValue);
        }
        GameMaster.GetInstance().m_cardPileUI.DisplayCardPile(m_foundCards, false, m_cardsToPick);

        Debug.Log("Action Pick Cards started");
    }

    public void SetAction(int cardsToPick, int cardsToPickFrom)
    {
        m_cardsToPick = cardsToPick;
        m_cardToPickFrom = cardsToPickFrom;
    }

    // Update is called once per frame
    public override void UpdateAction()
    {
        m_actionEnded = !CardEffects.GetInstance().m_isSelectingCards;
        if (m_actionEnded)
            Debug.Log("Action Pick Cards ended");
    }
}
