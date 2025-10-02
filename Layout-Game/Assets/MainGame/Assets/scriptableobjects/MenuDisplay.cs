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
    public Image itemImage;
    public Button linkButton;

    private ItemInspectManager inspectManager;

    private void Awake()
    {
        // Grab the one ItemInspectManager in the scene
        inspectManager = FindFirstObjectByType<ItemInspectManager>();
    }

    /// <summary>
    /// Fill the UI from an ItemData ScriptableObject.
    /// Also trigger the 3D inspect system.
    /// </summary>
    public void ShowItem(ItemData data)
    {
        if (data == null) return;

        // --- UI text ---
        nameText.text = data.itemname;
        irlnameText.text = data.irlname;
        attributesText.text = data.attributes;
        gameexText.text = data.gameex;
        irlexText.text = data.irlex;

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

    // optional: clear the UI
    public void Clear()
    {
        nameText.text = "";
        irlnameText.text = "";
        attributesText.text = "";
        gameexText.text = "";
        irlexText.text = "";
        itemImage.sprite = null;
        itemImage.enabled = false;
        linkButton.onClick.RemoveAllListeners();
        linkButton.interactable = false;

        if (inspectManager != null)
            inspectManager.Clear();
    }
}
