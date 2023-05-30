using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    public GameObject player;
    public ItemContainer inventoryContainer;
    public DnDController dndController;
    public DialogueSystem dialogueSystem;
}
