using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    /*
    public static void SaveGame(Deck deck)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerDeck";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();
        foreach (var card in deck.m_cards)
        {
            data.deck.Add(card.ToString());
        }

        formatter.Serialize(stream, data);
        stream.Close();
    }
    */

    public static bool SaveGame()
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
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("End saving");
        return true;
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
                return null;
                Debug.Log("End loading");
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
}