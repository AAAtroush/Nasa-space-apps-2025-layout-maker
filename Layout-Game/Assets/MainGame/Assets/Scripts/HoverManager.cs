using System.Collections.Generic;
using UnityEngine;

public class HoverManager : MonoBehaviour
{
    [Header("All Controlled Objects")]
    public List<GameObject> allObjects = new List<GameObject>();
    
<<<<<<< Updated upstream
=======
    [Header("Current State")]
    [SerializeField] private int currentActiveIndex = -1;
    
>>>>>>> Stashed changes
    void Start()
    {
        // Hide all objects initially
        HideAllObjects();
    }
<<<<<<< Updated upstream

    public GameObject elementMohmFa45;
=======
>>>>>>> Stashed changes
    
    public void ShowObjectsForIndex(int index)
    {
        // Hide all objects first
        HideAllObjects();
        
<<<<<<< Updated upstream
        // Validate index and show only the specified one
        if (index >= 0 && index < allObjects.Count && allObjects[index] != null)
        {
            allObjects[index].SetActive(true);
            elementMohmFa45.SetActive(true);
=======
        // Validate index
        if (index >= 0 && index < allObjects.Count)
        {
            allObjects[index].SetActive(true);
            currentActiveIndex = index;
>>>>>>> Stashed changes
        }
        else
        {
            Debug.LogWarning($"Index {index} is out of range!");
<<<<<<< Updated upstream
=======
            currentActiveIndex = -1;
>>>>>>> Stashed changes
        }
    }
    
    public void HideAllObjects()
    {
        foreach (GameObject obj in allObjects)
        {
            if (obj != null)
                obj.SetActive(false);
<<<<<<< Updated upstream

            elementMohmFa45.SetActive(false);
        }
=======
        }
        currentActiveIndex = -1;
    }
    
    public void ShowOnlyIndex(int index)
    {
        for (int i = 0; i < allObjects.Count; i++)
        {
            if (allObjects[i] != null)
                allObjects[i].SetActive(i == index);
        }
        currentActiveIndex = index;
>>>>>>> Stashed changes
    }
}