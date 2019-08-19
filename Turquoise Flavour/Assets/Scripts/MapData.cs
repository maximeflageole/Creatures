using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MapData", menuName = "Map data")]
public class MapData : ScriptableObject
{
    public int m_wildBattleMin;
    public int m_wildBattleMax;
    public int m_trainerBattleMin;
    public int m_trainerBattleMax;
    public int m_treasureMin;
    public int m_treasureMax;
}
