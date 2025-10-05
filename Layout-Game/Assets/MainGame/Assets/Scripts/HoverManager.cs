using System.Collections.Generic;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    [Header("All Controlled Objects")]
    public List<GameObject> allObjects = new List<GameObject>();
    
    void Start()
    {
        // Hide all objects initially
        HideAllObjects();
    }

    public GameObject elementMohmFa45;
    
    public void ShowObjectsForIndex(int index)
    {
        // Hide all objects first
        HideAllObjects();
        
        // Validate index and show only the specified one
        if (index >= 0 && index < allObjects.Count && allObjects[index] != null)
        {
            allObjects[index].SetActive(true);
            elementMohmFa45.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Index {index} is out of range!");
        }
    }
    
    public void HideAllObjects()
    {
        foreach (GameObject obj in allObjects)
        {
            if (obj != null)
                obj.SetActive(false);

            elementMohmFa45.SetActive(false);
        }
    }
}