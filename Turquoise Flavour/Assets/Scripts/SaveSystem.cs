using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame()
    {
        Debug.Log("Start saving");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerDeck";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();
        Player player = Player.GetPlayerInstance();
        foreach (Creature creature in player.GetCreatures())
        {
            data.creaturesSave.Add(creature.GetSaveableCreature());
        }
        foreach (var completedNode in GameMaster.GetInstance().m_completedNodes)
        {
            data.completedNodes.Add(completedNode);
        }
        foreach (var item in InventoryManager.GetInstance().GetInventoryItems())
        {
            data.inventoryItems.Add(item);
        }
        foreach (var explorator in ExploratorManager.GetInstance().m_unlockedExplorators)
        {
            data.unlockedExplorators.Add(explorator);
        }
        data.currentExplorator = Player.GetPlayerInstance().GetCurrentExploratorEnum();
        foreach (var stat in StatisticsManager.GetInstance().GetStatistics())
        {
            data.gameStatistics.Add(stat);
        }
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("End saving");
    }

    public static void ResetSave()
    {
        Debug.Log("Start Reset");
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerDeck";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("End Reset");
    }

    public static SaveData LoadGame()
    {
        Debug.Log("Start loading");
        string path = Application.persistentDataPath + "/playerDeck";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            if (stream.Length == 0)
            {
                stream.Close();
                Debug.Log("End loading");
                return null;
            }

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            Debug.Log("End loading");

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            Debug.Log("End loading");
            return null;
        }
    }
}

[System.Serializable]
public class SaveData
{
    public List<CreatureSaveable> creaturesSave = new List<CreatureSaveable>();
    public List<int> completedNodes = new List<int>();
    public List<TupleItemInventory> inventoryItems= new List<TupleItemInventory>();
    public List<Turquoise.EExplorator> unlockedExplorators = new List<Turquoise.EExplorator>();
    public Turquoise.EExplorator currentExplorator = Turquoise.EExplorator.Count;
    public List<Turquoise.SStatTuple> gameStatistics = new List<Turquoise.SStatTuple>();
}