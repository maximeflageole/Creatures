using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    protected List<Creature> m_creatures;

    public void Start()
    {
        LoadCreaturesDecks();
        print("PlayerStart");
    }

    protected void LoadCreaturesDecks()
    {
        foreach (var creature in m_creatures)
        {
            print("Creature load decks");
            creature.LoadDeck();
        }
    }
}
