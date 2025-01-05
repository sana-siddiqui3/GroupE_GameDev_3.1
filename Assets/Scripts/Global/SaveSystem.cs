using System.IO;
using UnityEngine;

public static class SaveSystem
{
    // This path points to the device’s persistent data folder
    private static string saveFilePath = Application.persistentDataPath + "/saveData.json";

    // Save the data to a JSON file
    public static void Save(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Game saved to " + saveFilePath);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to save data: " + ex.Message);
        }
    }

    // Load the data from a JSON file
    public static SaveData Load()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No save file found at " + saveFilePath);
            return null;
        }

        try
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Game loaded from " + saveFilePath);
            return data;
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to load data: " + ex.Message);
            return null;
        }
    }
}
