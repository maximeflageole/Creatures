using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overworld : MonoBehaviour
{
    public static Overworld s_OverworldInstance;
    public List<ExplorationNodeData> m_nodesData;

    public static ExplorationNodeData GetExplorationNodeDataFromEventType(Exploration.EEventType eventType)
    {
        if (s_OverworldInstance == null)
        {
            return null;
        }
        foreach (var nodeData in s_OverworldInstance.m_nodesData)
        {
            if (nodeData.eventType == eventType)
            {
                return nodeData;
            }
        }
        return null;
    }

    public void Awake()
    {
        s_OverworldInstance = this;
    }
}
