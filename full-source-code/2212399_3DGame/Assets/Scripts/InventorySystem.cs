using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    public GameObject inventoryScreenUI, ItemInfoUI;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    private GameObject itemToAdd, whatSlotToEquip;
    public bool isOpen;
    public GameObject pickupAlert;
    public Text pickupName;
    public Image pickupImage;
    private Coroutine pickupHideRoutine;
    private Coroutine recalcRoutine;
    void TriggerPopup(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName+" added";
        pickupImage.sprite = itemSprite;
        if (pickupHideRoutine != null)
            StopCoroutine(pickupHideRoutine);
        pickupHideRoutine = StartCoroutine(HidePickupAfterDelay(5f));
    }
    private IEnumerator HidePickupAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (pickupAlert != null)
            pickupAlert.SetActive(false);
        pickupHideRoutine = null;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        PopulateSlotList();
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            if (!Crafting.Instance.isOpen)
                Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void AddToInventory(string item)
    {
        whatSlotToEquip = FindNextEmptySlot();
        itemToAdd = (GameObject)Instantiate(Resources.Load<GameObject>(item), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);
    itemList.Add(item);
    TriggerPopup(item, itemToAdd.GetComponent<Image>().sprite);
    ScheduleRecalculate();
    }

    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
            if (slot.transform.childCount == 0)
                return slot;
        return new GameObject();
    }

    public bool CheckFull()
    {
        int counter = 0;
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
                counter += 1;
        }
        if (counter == 21) return true;
        else return false;
    }

    public void RemoveItem(string itemToRemove, int amount)
    {
        int counter = amount;
        for (var i = slotList.Count - 1; i >= 0; i--)
            if (slotList[i].transform.childCount > 0)
            {
                if (slotList[i].transform.GetChild(0).name == itemToRemove + "(Clone)" && counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        ScheduleRecalculate();

    }
    public void RecalculateList()
    {
        itemList.Clear();
        foreach (GameObject slot in slotList)
        {
            if (slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                itemList.Add(result);
            }
        }
    }

    // Ensures we recalc after Unity has processed Destroy/Instantiate at end of frame
    private void ScheduleRecalculate()
    {
        if (recalcRoutine != null)
            StopCoroutine(recalcRoutine);
        recalcRoutine = StartCoroutine(RecalculateAfterFrame());
    }
    private IEnumerator RecalculateAfterFrame()
    {
        // Wait one frame so destroyed/instantiated inventory item objects are reflected in hierarchy
        yield return null;
        RecalculateList();
        if (Crafting.Instance != null)
            Crafting.Instance.RefreshNeededItems();
        recalcRoutine = null;
    }
}