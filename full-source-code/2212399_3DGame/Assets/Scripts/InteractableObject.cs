using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;

    public string GetItemName()
    {
        return ItemName;
    }
    public void Pickup()
    {
        // Block pickup when any UI screen is open
        if (InventorySystem.Instance.isOpen || Crafting.Instance.isOpen)
            return;

        if (!InventorySystem.Instance.CheckFull())
        {
            InventorySystem.Instance.AddToInventory(ItemName);
            Destroy(gameObject);
        } else{
            // Inventory full: silently ignore pickup
        }
    }
}
