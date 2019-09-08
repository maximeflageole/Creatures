using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreaturesActionUI : MonoBehaviour
{
    public Turquoise.ECreatureInteraction m_interaction;

    public void OnClick()
    {
        GetComponentInParent<ActionPanelUI>().OnClickCreatureAction(m_interaction);
    }
}
