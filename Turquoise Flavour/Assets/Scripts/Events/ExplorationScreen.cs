using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationScreen : MonoBehaviour
{
    [SerializeField]
    protected GameObject m_exploratorPrefab;
    [SerializeField]
    protected GameObject m_explorator;
    [SerializeField]
    protected Vector3 m_exploratorNodeOffset;
    [SerializeField]
    protected GameObject m_nodePrefab;
    [SerializeField]
    protected GameObject m_linePrefab;
    [SerializeField]
    protected List<ExplorationNode> m_explorationNodes;
    public List<ExplorationNode> GetExplorationNodes() { return m_explorationNodes; }
    [SerializeField]
    protected Color m_availableColor;
    [SerializeField]
    protected Color m_unavailableColor;
    [SerializeField]
    protected Color m_undiscoveredColor;
    [SerializeField]
    protected MapData m_mapData;
    public MapData GetMapData() { return m_mapData; }

    void Start()
    {

    }

    public void Init()
    {
        int i = 0;
        foreach (var node in m_mapData.explorationNodes)
        {
            Vector3 vec = new Vector3(node.location.x, node.location.y, 0);
            GameObject gameObject = Instantiate(m_nodePrefab, vec, Quaternion.identity, transform);
            ExplorationNode explorationNode = gameObject.GetComponent<ExplorationNode>();
            explorationNode.m_nodeId = i;
            explorationNode.SetData(node.explorationNode);
            m_explorationNodes.Add(explorationNode);
            i++;
        }
        foreach (var node in m_explorationNodes)
        {
            node.InitializeConnections();
            node.ConnectNode();
            foreach (var connectedNode in node.GetConnectedNodes())
            {
                if (connectedNode.m_isConnected)
                {
                    continue;
                }
                GameObject line = Instantiate(m_linePrefab, transform);
                LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, node.transform.position);
                lineRenderer.SetPosition(1, connectedNode.transform.position);
                if (node.GetIsAvailable() && connectedNode.GetIsAvailable())
                {
                    lineRenderer.startColor = m_availableColor;
                    lineRenderer.endColor = m_availableColor;
                }
                else if ((node.GetIsDiscovered() && connectedNode.GetIsDiscovered()) && (node.GetIsAvailable() || connectedNode.GetIsAvailable()))
                {
                    lineRenderer.startColor = m_unavailableColor;
                    lineRenderer.endColor = m_unavailableColor;
                }
                else
                {
                    lineRenderer.startColor = m_undiscoveredColor;
                    lineRenderer.endColor = m_undiscoveredColor;
                }
            }
        }
        m_explorator = Instantiate(m_exploratorPrefab, transform);
        MoveExplorator(m_explorationNodes[0]);
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
                    if (m_explorator.GetComponent<Explorator>().IsNodeSelected(node))
                    {
                        GameMaster.GetInstance().StartEvent(node);
                        hit.collider.enabled = false;
                    }
                    else
                    {
                        MoveExplorator(node);
                        m_explorator.GetComponent<Explorator>().SelectNode(node);
                    }
                }
            }
        }
    }

    void MoveExplorator(ExplorationNode node)
    {
        m_explorator.transform.position = node.transform.position + m_exploratorNodeOffset;
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
