using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour
{
    [SerializeField]
    protected int m_duration;
    public int GetDuration() { return m_duration; }

    [SerializeField]
    protected int m_intensity;
    public int GetIntensity() { return m_intensity; }
}

public class Buff : Condition
{
    [SerializeField]
    protected EBuffType m_buffType;

    public EBuffType GetBuffType() { return m_buffType; } 
}

public class Debuff : Condition
{
    [SerializeField]
    protected EDebuffType m_debuffType;

    public EDebuffType GetDebuffType() { return m_debuffType; }
}

public enum EBuffType
{
    Damage,
    Armor,
    Random,
    None
}

public enum EDebuffType
{
    Damage,
    Armor,
    Random,
    None
}