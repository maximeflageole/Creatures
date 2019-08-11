using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

[CreateAssetMenu(fileName = "New Ai Actions", menuName = "Ai Actions")]
public class AIActions : ScriptableObject
{
    public List<sActionToCard> actionToEffects = new List<sActionToCard>();
    public List<EAIActions> combatActions = new List<EAIActions>();
}

namespace Turquoise
{
    [System.Serializable]
    public struct sActionToCard
    {
        public EAIActions action;
        public CardData cardData;
    }
}