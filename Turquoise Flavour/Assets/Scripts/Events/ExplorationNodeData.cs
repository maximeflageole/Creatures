﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Exploration Node", menuName = "Exploration Node")]
public class ExplorationNodeData : ScriptableObject
{
    public Sprite availableSprite;
    public Sprite completedSprite;
    public Sprite discoveredSprite;
    public string nodeName;
    public Exploration.EEventType eventType;
    public DialogueData dialogueData;
}
