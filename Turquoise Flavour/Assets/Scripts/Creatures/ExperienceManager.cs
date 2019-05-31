using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    public static int GetNextLevelXp(ELevelSpeed levelSpeed, float currentLevel)
    {
        float coefficient = 0.0f;
        switch (levelSpeed)
        {
            case ELevelSpeed.Fast:
                coefficient = 50.0f;
                break;
            case ELevelSpeed.Normal:
                coefficient = 70.0f;
                break;
            case ELevelSpeed.Slow:
                coefficient = 90.0f;
                break;
            case ELevelSpeed.UltraSlow:
                coefficient = 110.0f;
                break;
            default:
                break;
        }
        return Mathf.RoundToInt(coefficient * Mathf.Pow(1.04f, currentLevel+1));
    }
}

[System.Serializable]
public enum ELevelSpeed
{
    Fast,
    Normal,
    Slow,
    UltraSlow,
    Count
}
