using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    protected EAIActions m_nextAiAction = EAIActions.None;
    [SerializeField]
    protected SpriteRenderer m_nextAiActionSpriteRenderer;
    [SerializeField]
    protected AIActions m_aiActions;
    [SerializeField]
    protected int m_actionNumber;

    public void GetNextAction()
    {
        m_nextAiAction = m_aiActions.combatActions[m_actionNumber % m_aiActions.combatActions.Count];
        m_actionNumber++;
    }

    public void Update()
    {
        if (m_nextAiActionSpriteRenderer != null)
        {
            m_nextAiActionSpriteRenderer.sprite = AIManager.GetInstance().GetActionSprite(m_nextAiAction);
        }
    }

    public void ExecuteAction()
    {
        CardData cardData = new CardData();
        foreach (var actionToCard in m_aiActions.actionToEffects)
        {
            if (actionToCard.action == m_nextAiAction)
            {
                cardData = actionToCard.cardData;
            }
        }

        CardEffects.GetInstance().ExecuteEnemyCard(cardData);
    }

    public void BeginTurn()
    {
        ExecuteAction();
    }
}
