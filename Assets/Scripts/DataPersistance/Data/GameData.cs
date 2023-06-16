using UnityEngine;

[System.Serializable]
public class GameData
{
    public SerializableDictionary<string, bool> itemsCollected;
    public Vector3 playerPosition;
    public string currentSceneName;

    // Default values for the Game when there is no data to load
    public GameData()
    {
        itemsCollected = new SerializableDictionary<string, bool>();
        playerPosition = Vector3.zero;
        currentSceneName = "Prologue";
    }


    public string GetGameLevel()
    {
        return currentSceneName.Replace("Scene", "");
    }
}
