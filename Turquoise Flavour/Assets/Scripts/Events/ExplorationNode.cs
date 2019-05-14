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
    protected ExplorationNodeData m_nodeData;
    public int m_nodeId;

    // Start is called before the first frame update
    void Start()
    {
        m_nodeData = Overworld.GetExplorationNodeDataFromEventType(m_eventType);
        if (m_nodeData != null)
        {
            Sprite sprite = m_nodeData.availableSprite;
            if (m_isCompleted)
            {
                sprite = m_nodeData.completedSprite;
            }
            GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    public void CompleteNode()
    {
        m_isCompleted = true;
    }
}