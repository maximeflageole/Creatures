using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exploration;

public class ExplorationNode : MonoBehaviour
{
    public EEventType m_eventType;
    [SerializeField]
    protected bool m_isCompleted;
    [SerializeField]
    protected bool m_isAvailable;
    [SerializeField]
    protected bool m_isDiscovered;
    [SerializeField]
    protected ExplorationNodeData m_nodeData;
    public int m_nodeId;
    [SerializeField]
    protected List<ExplorationNode> m_connectedNodes = new List<ExplorationNode>();
    public List<ExplorationNode> GetConnectedNodes() { return m_connectedNodes; }

    // Start is called before the first frame update
    void Awake()
    {
        m_nodeData = Overworld.GetExplorationNodeDataFromEventType(m_eventType);
        if (m_isAvailable)
        {
            MakeNodeAvailable();
        }
    }

    public void Update()
    {
        Sprite sprite = null;
        if (m_nodeData != null)
        {
            if (m_isCompleted)
            {
                sprite = m_nodeData.completedSprite;
            }
            else if (m_isAvailable)
            {
                sprite = m_nodeData.availableSprite;
            }
            else if (m_isDiscovered)
            {
                sprite = m_nodeData.discoveredSprite;
            }
        }
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void CompleteNode()
    {
        m_isCompleted = true;
        foreach(var node in m_connectedNodes)
        {
            node.MakeNodeAvailable();
        }
    }

    public void DiscoverNode()
    {
        m_isDiscovered = true;
    }

    public void MakeNodeAvailable()
    {
        m_isAvailable = true;
        foreach (var node in m_connectedNodes)
        {
            node.DiscoverNode();
        }
    }

    public bool GetIsCompleted()
    {
        return m_isCompleted;
    }

    public bool IsAvailable()
    {
        return m_isAvailable;
    }
}