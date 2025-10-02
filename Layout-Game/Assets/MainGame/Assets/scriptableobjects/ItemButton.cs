using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public ItemData item;                 // assign per button
    public MenuDisplay targetMenu;        // assign the matching menu

    private void Start()
    {
        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() =>
            {
                if (targetMenu != null && item != null)
                    targetMenu.ShowItem(item);
            });
        }
    }
}
