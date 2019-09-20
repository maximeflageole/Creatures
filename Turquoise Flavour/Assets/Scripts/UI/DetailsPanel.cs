using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailsPanel : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI m_textMesh;

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null)
        {
            Card cardData = hit.transform.gameObject.GetComponentInChildren<Card>();
            CreatureUIComp creatureData = hit.transform.gameObject.GetComponentInChildren<CreatureUIComp>();

            if (cardData != null)
            {
                //Card data found
                m_textMesh.text = "Card found";
            }
            else if (creatureData != null)
            {
                //Creature data found
                m_textMesh.text = "Creature found";
            }
            else
            {
            }
        }
    }
}
