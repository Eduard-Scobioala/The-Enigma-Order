using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public SerializableDictionary<string, bool> itemsCollected;

    // Default values for the Game when there is no data to load
    public GameData()
    {
        playerPosition = Vector3.zero;
        itemsCollected = new SerializableDictionary<string, bool>();
    }
}
