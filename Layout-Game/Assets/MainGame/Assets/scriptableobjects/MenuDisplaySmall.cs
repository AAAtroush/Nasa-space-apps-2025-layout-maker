using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuDisplaySmall : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text attributesText;
    public TMP_Text priceText;
    public TMP_Text gameexText;

    [Header("Hover Settings")]
    public int buttonIndex; // 0-BASED! 0 = first object, 1 = second object, etc.
    public HoverManager hoverManager;

    [Header("Item Data")]
    public ItemData itemData; // Assign your ScriptableObject here in Inspector

    private ItemInspectManager inspectManager;

    private void Awake()
    {
        inspectManager = FindFirstObjectByType<ItemInspectManager>();
        
        if (hoverManager == null)
            hoverManager = GetComponentInParent<HoverManager>();
    }

    private void Start()
    {
        // Auto-populate with assigned ItemData when game starts
        if (itemData != null)
        {
            ShowItem(itemData);
        }
        else
        {
            Debug.LogWarning($"No ItemData assigned to {gameObject.name}");
        }
    }

    // Hover functionality - shows/hides GameObjects only
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverManager?.ShowObjectsForIndex(buttonIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverManager?.HideAllObjects();
    }

    // Call this method when button is CLICKED to show the item data
    public void OnButtonClick()
    {
        if (itemData != null)
        {
            ShowItem(itemData);
        }
    }

    // Populates UI with ScriptableObject data
    public void ShowItem(ItemData data)
    {
        if (data == null) 
        {
            Debug.LogWarning("No ItemData provided to ShowItem!");
            return;
        }

        Debug.Log($"Displaying item: {data.itemname}");

        // Populate UI from ScriptableObject
        if (nameText != null) nameText.text = data.itemname ?? "";
        if (attributesText != null) attributesText.text = data.attributes ?? "";
        if (gameexText != null) gameexText.text = data.gameex ?? "";
        if (priceText != null) priceText.text = data.price ?? "";

        // Trigger inspect system
        if (inspectManager != null)
        {
            inspectManager.ShowItem(data);
        }
    }

    public void Clear()
    {
        if (nameText != null) nameText.text = "";
        if (attributesText != null) attributesText.text = "";
        if (gameexText != null) gameexText.text = "";
        if (priceText != null) priceText.text = "";

        if (inspectManager != null)
            inspectManager.Clear();
    }
}