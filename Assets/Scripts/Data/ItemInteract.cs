using UnityEngine;

public class ItemInteract : Interactable, IDataPersistance
{
    [SerializeField] private string itemId;
    [ContextMenu("Generate guid for itemId")]
    private void GenerateGuid()
    {
        itemId = System.Guid.NewGuid().ToString();
    }

    public Item item;
    public int count = 1;
    private bool collected = false;

    #region Interact Logic
    public override void Interact(Character character)
    {
        if (GameManager.instance.inventoryContainer != null)
        {
            collected = true;
            GameManager.instance.inventoryContainer.Add(item, count);
        }
        else
        {
            Debug.LogWarning("No inventory container attached to the game manager");
        }

        gameObject.SetActive(false);
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = item.icon;
    }

    #endregion

    #region Save&Load

    public void LoadData(GameData data)
    {
        // Get saved state of the item
        data.itemsCollected.TryGetValue(itemId, out collected);
        if (collected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        // remove the previos state of the item
        if (data.itemsCollected.ContainsKey(itemId))
        {
            data.itemsCollected.Remove(itemId);
        }

        // updata with current state of the item
        data.itemsCollected.Add(itemId, collected);
    }

    #endregion
}
