using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] GameObject panel;

    private void Update()
    {
        if (GameManager.instance.canOpenInventory && Input.GetKeyDown(KeyCode.I))
        {
            panel.SetActive(!panel.activeInHierarchy);
        }
    }
}
