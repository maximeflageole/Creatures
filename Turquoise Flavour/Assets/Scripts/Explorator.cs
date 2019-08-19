using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explorator : MonoBehaviour
{
    [SerializeField]
    protected ExplorationNode m_currentNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectNode(ExplorationNode node)
    {
        m_currentNode = node;
    }

    public bool IsNodeSelected(ExplorationNode node)
    {
        return m_currentNode == node;
    }
}
