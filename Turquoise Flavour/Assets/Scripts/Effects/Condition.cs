using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class Condition : MonoBehaviour
{
    [SerializeField]
    protected int m_stacks;
    public int GetStacks() { return m_stacks; }
    public void IncrementStacks(int increment) { m_stacks += increment; }
    public void RemoveAllStacks() { m_stacks = 0; }

    [SerializeField]
    protected ConditionData m_conditionData;
    public ConditionData GetData() { return m_conditionData; }

    public void OnCreate(ConditionData conditionData, int stacks)
    {
        m_conditionData = conditionData;
        m_stacks = stacks;
    }
}