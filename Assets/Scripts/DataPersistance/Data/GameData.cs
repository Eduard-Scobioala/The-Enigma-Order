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
        playerPosition = new Vector3(-0.5f, -5f, 0f);
        currentSceneName = "Prologue";
    }


    public string GetGameLevel()
    {
        return currentSceneName.Replace("Scene", "");
    }
}
