using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField]
    protected List<Card> m_cards;

    [SerializeField]
    protected List<Card> m_fullDeckCards;
}
