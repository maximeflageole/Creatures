using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class Condition : MonoBehaviour
{
    [SerializeField]
    protected int m_intensity;
    public int GetIntensity() { return m_intensity; }
    public void IncrementIntensity(int increment) { m_intensity += increment; }

    [SerializeField]
    protected ConditionData m_conditionData;
    public ConditionData GetData() { return m_conditionData; }

    public void OnCreate(ConditionData conditionData, int intensity)
    {
        m_conditionData = conditionData;
        m_intensity = intensity;
    }
}