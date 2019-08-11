using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationScreen : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject go = hit.collider.gameObject;
                ExplorationNode node = go.GetComponent<ExplorationNode>();
                if (node != null && !node.GetIsCompleted() && node.IsAvailable())
                {
                    GameMaster.GetInstance().StartEvent(node);
                    hit.collider.enabled = false;
                }
            }
        }
    }
}

namespace Exploration
{
    public enum EEventType
    {
        CardReward,
        WildEncounter,
        Boss,
        Treasure,
        CreaturePick,
        Count
    }
}
