using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionShuffling : Action
{
    public override void StartAction()
    {
        base.StartAction();
        Debug.Log("Action Shuffling started");
    }

    // Update is called once per frame
    public override void UpdateAction()
    {
        m_actionEnded = CardEffects.GetInstance().IsDiscardPileEmpty();
        if(m_actionEnded)
        Debug.Log("Action shuffling ended");
    }
}
