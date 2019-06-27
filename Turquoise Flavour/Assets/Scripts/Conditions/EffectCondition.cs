using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Turquoise;

public class EffectCondition : MonoBehaviour
{
    [SerializeField]
    protected int m_quantity;
    [SerializeField]
    protected EConditionType m_condition;
    [SerializeField]
    protected GameObject m_target;

    public virtual bool VerifyCondition(GameObject m_target)
    {
        return false;
    }
}

namespace Turquoise
{
    public enum EConditionType
    {
        Bleeds,
        Count
    }
}