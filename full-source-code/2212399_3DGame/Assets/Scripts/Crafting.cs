using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour
{
    public GameObject craftingscreenUI, toolsScreenUI;
    public List<string> inventoryItemList = new List<string>();
    //Category Buttons
    Button toolsBTN;
    //Craft Buttons
    Button craftAxeBTN;
    //Requirement
    Text AxeReq1, AxeReq2;
    public bool isOpen;
    //Blueprints
    public Blueprints AxeBLP = new Blueprints("Axe", "Stone", 3, "Stick", 3, 2);

    public static Crafting Instance { get; set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isOpen = false;

        toolsBTN = craftingscreenUI.transform.Find("ToolsBtn").GetComponent<Button>();
        // ...existing code...
        toolsBTN.onClick.AddListener(delegate { openToolsCategory(); });

        //Axe
        var axeGO = toolsScreenUI.transform.Find("Axe");
        // ...existing code...
        AxeReq1 = axeGO.transform.Find("req1").GetComponent<Text>();
        AxeReq2 = axeGO.transform.Find("req2").GetComponent<Text>();
        craftAxeBTN = axeGO.transform.Find("Button").GetComponent<Button>();
        if (craftAxeBTN != null)
            craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }
    private void openToolsCategory()
    {
        craftingscreenUI.SetActive(false);
        toolsScreenUI.SetActive(true);
        // Ensure UI reflects current inventory when switching panels
        RefreshNeededItems();
    }

    void CraftAnyItem(Blueprints blueprint)
    {
        if (!CanCraft(blueprint))
        {
            RefreshNeededItems();
            return;
        }
        InventorySystem.Instance.AddToInventory(blueprint.itemName);
        if (blueprint.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprint.Req1, blueprint.Req1Amount);
        }
        else if (blueprint.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprint.Req1, blueprint.Req1Amount);
            InventorySystem.Instance.RemoveItem(blueprint.Req2, blueprint.Req2Amount);
        }
        StartCoroutine(calculate());
        RefreshNeededItems();
    }

    private bool CanCraft(Blueprints blueprint)
    {
        int req1Count = 0;
        int req2Count = 0;
        var items = InventorySystem.Instance.itemList;
        foreach (var name in items)
        {
            if (name == blueprint.Req1) req1Count++;
            if (blueprint.numOfRequirements == 2 && name == blueprint.Req2) req2Count++;
        }
        if (blueprint.numOfRequirements == 1)
            return req1Count >= blueprint.Req1Amount;
        else
            return req1Count >= blueprint.Req1Amount && req2Count >= blueprint.Req2Amount;
    }

    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(0.025f);
        InventorySystem.Instance.RecalculateList();
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            craftingscreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingscreenUI.SetActive(false);
            toolsScreenUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)
                Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }
    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        inventoryItemList = InventorySystem.Instance.itemList;
        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        //Axe
        AxeReq1.text = "3 Stones [" + stone_count + "]";
        AxeReq2.text = "3 Sticks [" + stick_count + "]";

        if (stone_count >= 3 && stick_count >= 3)
            craftAxeBTN.gameObject.SetActive(true);
        else craftAxeBTN.gameObject.SetActive(false);

    }
}
