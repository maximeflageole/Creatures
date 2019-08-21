using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapData", menuName = "Map data")]
public class MapData : ScriptableObject
{
    public List<NodeDataAndLocation> explorationNodes;
    public List<NodeLink> nodeLinks;

    public List<int> GetConnectedNodes(int id)
    {
        List<int> connectedNodes = new List<int>();
        foreach (var link in nodeLinks)
        {
            if (link.firstNodeId == link.secondNodeId)
            {
                Debug.Log("2 connected nodes have the same id");
                continue;
            }
            if (link.firstNodeId == id)
            {
                connectedNodes.Add(link.secondNodeId);
            }
            else if (link.secondNodeId == id)
            {
                connectedNodes.Add(link.firstNodeId);
            }
        }
        return connectedNodes;
    }

    public ExplorationNodeData GetNodeFromId(int i)
    {
        if (i < explorationNodes.Count)
        {
            return explorationNodes[i].explorationNode;
        }
        return null;
    }
}

[System.Serializable]
public struct NodeDataAndLocation
{
    public ExplorationNodeData explorationNode;
    public Vector2 location;
}

[System.Serializable]
public struct NodeLink
{
    public int firstNodeId;
    public int secondNodeId;
}