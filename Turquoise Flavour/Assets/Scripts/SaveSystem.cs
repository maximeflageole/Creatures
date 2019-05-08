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

    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/playerDeck";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData();
        Player player = Player.GetPlayerInstance();
        foreach (Creature creature in player.GetCreatures())
        {
            data.creaturesSave.Add(creature.GetSaveableCreature());
        }

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static SaveData LoadGame()
    {
        string path = Application.persistentDataPath + "/playerDeck";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            if (stream.Length == 0)
            {
                return null;
            }

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}

[System.Serializable]
public class SaveData
{
    public List<CreatureSaveable> creaturesSave = new List<CreatureSaveable>();
}