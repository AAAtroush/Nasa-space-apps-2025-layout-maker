using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Assign all your menus here")]
    public GameObject[] menus;

    public void OpenMenu(int index)
    {
        // Disable all
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].SetActive(false);
        }

        // Enable chosen one
        if (index >= 0 && index < menus.Length)
        {
            menus[index].SetActive(true);
        }
    }
}
