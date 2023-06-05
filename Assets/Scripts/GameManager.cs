using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;
    public ItemContainer inventoryContainer;
    public DnDController dndController;

    private void Awake()
    {
        instance = this;
    }
}
