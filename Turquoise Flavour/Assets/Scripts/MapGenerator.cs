using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static void Generator(MapData mapData)
    {
        int wildBattlesCount = Random.Range(mapData.m_wildBattleMin, mapData.m_wildBattleMax+1);
        int trainerBattlesCount = Random.Range(mapData.m_trainerBattleMin, mapData.m_trainerBattleMax+1);
        int treasureCount = Random.Range(mapData.m_treasureMin, mapData.m_treasureMax+1);

        Debug.Log("wild encounters: " + wildBattlesCount + " trainer battles: " + trainerBattlesCount + " treasures: " + treasureCount);
    }
}
