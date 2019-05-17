using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creatures;

public class CreaturePickEvent : TurquoiseEvent
{
    [SerializeField]
    protected List<ECreature> creatures;
    [SerializeField]
    protected List<SelectableCreature> selectableCreatures;

    void Update()
    {
        CheckMouseClickCreature();
    }

    void Start()
    {
        if (creatures.Count == 0)
        {
            print("Missing creatures data for creature pick");
            return;
        }
        if (creatures.Count > selectableCreatures.Count)
        {
            print("More creatures data than the UI can support!");
            return;
        }
        for (int i = 0; i < creatures.Count; i++)
        {
            selectableCreatures[i].Initialize(creatures[i]);
        }
    }
    // This function is called to check if the mouse has clicked on a card
    void CheckMouseClickCreature()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                SelectableCreature creatureHit = hit.collider.GetComponent<SelectableCreature>();
                if (creatureHit == null)
                {
                    return;
                }
                AddCreature(creatureHit.m_creatureData);
            }
        }
    }

    protected void AddCreature(CreatureData creatureData, int level = 1)
    {
        Player.GetPlayerInstance().CaptureCreature(creatureData, level);
        DisableEvent();
        GameMaster.GetInstance().EndCurrentEvent(true);
    }

    protected void DisableEvent()
    {
        foreach (var creature in selectableCreatures)
        {
            creature.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
