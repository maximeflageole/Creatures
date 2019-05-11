using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurquoiseUI : MonoBehaviour
{
    public void SaveGame()
    {
        GameMaster.GetInstance().SaveGame();
    }

    public void ChangeCreature()
    {
        GameMaster.GetInstance().ChangeCreature();
    }
}
