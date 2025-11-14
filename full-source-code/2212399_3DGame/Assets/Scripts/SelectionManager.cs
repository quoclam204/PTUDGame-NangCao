using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public GameObject interaction_Info_UI;
    Text interaction_text;
    public Image centerDot;
    public Image handIcon;

    private void Start()
    {
        interaction_text = interaction_Info_UI.GetComponent<Text>();
    }

    void Update()
    {
        // Disable selection and pickup UI while any gameplay UI is open
        if (InventorySystem.Instance.isOpen || Crafting.Instance.isOpen)
        {
            interaction_Info_UI.SetActive(false);
            centerDot.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 6.5f))
        {
            var interactable = hit.transform.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);

                if (interactable.CompareTag("Pickupable"))
                {
                    centerDot.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    interactable.Pickup();
                }
            }
            else
            {
                interaction_Info_UI.SetActive(false);
                centerDot.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            interaction_Info_UI.SetActive(false);
            centerDot.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
        }
    }

}
