using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationScreen : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_linePrefab;
    [SerializeField]
    protected List<ExplorationNode> m_explorationNodes;

    void Start()
    {
        foreach (var node in GetComponentsInChildren<ExplorationNode>())
        {
            m_explorationNodes.Add(node);
        }
        foreach (var node in m_explorationNodes)
        {
            foreach (var connectedNode in node.GetConnectedNodes())
            {
                GameObject line = Instantiate(m_linePrefab);
                LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, node.transform.position);
                lineRenderer.SetPosition(1, connectedNode.transform.position);
            }
        }
    }

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
