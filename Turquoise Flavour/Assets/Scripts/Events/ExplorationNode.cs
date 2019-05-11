﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Exploration;

public class ExplorationNode : MonoBehaviour
{
    [SerializeField]
    protected EEventType m_eventType;
    [SerializeField]
    protected bool m_isCompleted;
    [SerializeField]
    protected ExplorationNodeData nodeData;

    // Start is called before the first frame update
    void Start()
    {
        nodeData = Overworld.GetExplorationNodeDataFromEventType(m_eventType);
        if (nodeData != null)
        {
            GetComponent<SpriteRenderer>().sprite = nodeData.availableSprite;
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
                if (node != null)
                {
                    GameMaster.GetInstance().ChangeEvent(node.m_eventType);
                    hit.collider.enabled = false;
                }
            }
        }
    }
}