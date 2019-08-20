using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapData", menuName = "Map data")]
public class MapData : ScriptableObject
{
    public List<NodeDataAndLocation> explorationNodes;
}

[System.Serializable]
public struct NodeDataAndLocation
{
    public ExplorationNodeData explorationNode;
    public Vector2 location;
}