using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuDisplay : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text nameText;
    public TMP_Text irlnameText;
    public TMP_Text attributesText;
    public TMP_Text gameexText;
    public TMP_Text irlexText;
    public TMP_Text priceText;
    public Image itemImage;
    public Button linkButton;

    private ItemInspectManager inspectManager;

    private void Awake()
    {
        inspectManager = FindFirstObjectByType<ItemInspectManager>();
    }

    public void ShowItem(ItemData data)
    {
        if (data == null) return;

        // --- UI text ---
        nameText.text = data.itemname;
        irlnameText.text = data.irlname;
        attributesText.text = data.attributes;
        gameexText.text = data.gameex;
        irlexText.text = data.irlex;
        priceText.text = data.price;   // 👈 NEW

        // --- UI image ---
        if (data.image != null)
        {
            itemImage.sprite = data.image;
            itemImage.enabled = true;
        }
        else
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
        }

        // --- Link button ---
        linkButton.onClick.RemoveAllListeners();
        string link = data.link;
        if (!string.IsNullOrEmpty(link))
        {
            linkButton.interactable = true;
            linkButton.onClick.AddListener(() => Application.OpenURL(link));
        }
        else
        {
            linkButton.interactable = false;
        }

        // --- Trigger the inspect system ---
        if (inspectManager != null)
        {
            Debug.Log("MenuDisplay calling InspectManager for: " + data.itemname);
            inspectManager.ShowItem(data);
        }
        else
        {
            Debug.LogWarning("No ItemInspectManager found in scene!");
        }
    }

    public void Clear()
    {
        nameText.text = "";
        irlnameText.text = "";
        attributesText.text = "";
        gameexText.text = "";
        irlexText.text = "";
        priceText.text = "";   // 👈 NEW
        itemImage.sprite = null;
        itemImage.enabled = false;
        linkButton.onClick.RemoveAllListeners();
        linkButton.interactable = false;

        if (inspectManager != null)
            inspectManager.Clear();
    }
}
