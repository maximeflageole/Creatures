using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsState : TurquoiseState
{
    [SerializeField]
    protected int turnCount;

    public override void UpdateState()
    {

    }

    public override bool VerifyOutConditions()
    {
        //Debug.Log("Card state out conditions are true");
        return false;
    }

    public override void StartState()
    {
        Debug.Log("Start CardsState");
        turnCount = 1;
        CardEffects cardEffects = CardEffects.GetCardEffectsInstance();

        //2: Start Fastest Creature round
        cardEffects.StartFirstTurn();
    }

    public override void EndState()
    {
        Debug.Log("Exits CardsState");
    }
}
