using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActionUI : MonoBehaviour
{
    public Turquoise.EItemInteraction m_interaction;

    public void OnClick()
    {
        GetComponentInParent<ActionPanelUI>().OnClickItemAction(m_interaction);
    }
}
