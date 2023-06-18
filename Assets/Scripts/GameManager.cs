using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public ItemContainer inventoryContainer;
    public DnDController dndController;

    public bool characterCanMove = true;
    public bool canOpenInventory = true;

    private void Awake()
    {
        instance = this;
    }
}
