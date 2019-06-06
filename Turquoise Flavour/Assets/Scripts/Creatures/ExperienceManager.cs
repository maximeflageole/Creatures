using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Experience;

public class ExperienceManager : MonoBehaviour
{
    public static int MAXLEVEL = 20;
    public static int GetNextLevelXp(ELevelSpeed levelSpeed, float currentLevel)
    {
        float coefficient = GetXpCoefficient(levelSpeed);
        if (currentLevel == MAXLEVEL)
        {
            return -1;
        }
        return (int)((Mathf.Pow(currentLevel, 2.0f)*(coefficient-currentLevel) + 500)/10);
    }

    public static float GetXpCoefficient(ELevelSpeed levelSpeed)
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
        return coefficient;
    }

    public static void AddExperience(int experience, ref CreatureExperience creatureExperience, Creature creature)
    {
        if (creatureExperience.level < MAXLEVEL)
        {
            int nextLvlXp = GetNextLevelXp(creatureExperience.levelSpeed, creatureExperience.level);
            if (nextLvlXp > creatureExperience.experiencePoints + experience)
            {
                creatureExperience.experiencePoints += experience;
                return;
            }
            //TODO: Level up
            creatureExperience.level++;
            creature.OnLevelUp();
            int rest = experience - (nextLvlXp - creatureExperience.experiencePoints);
            creatureExperience.experiencePoints = 0;
            AddExperience(rest, ref creatureExperience, creature);
        }
    }
}

namespace Experience
{
    [System.Serializable]
    public enum ELevelSpeed
    {
        Fast,
        Normal,
        Slow,
        UltraSlow,
        Count
    }
}

[System.Serializable]
public struct CreatureExperience
{
    public int level;
    public int experiencePoints;
    public ELevelSpeed levelSpeed;
}