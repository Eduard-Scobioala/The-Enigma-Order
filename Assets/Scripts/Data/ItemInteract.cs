using UnityEngine;

public class ItemInteract : Interactable
{
    public Item item;
    public int count = 1;

    public override void Interact(Character character)
    {
        if (GameManager.instance.inventoryContainer != null) {
            GameManager.instance.inventoryContainer.Add(item, count);
        }
        else
        {
            Debug.LogWarning("No inventory container attached to the game manager");
        }

        Destroy(gameObject);
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;

        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = item.icon;
    }
}
