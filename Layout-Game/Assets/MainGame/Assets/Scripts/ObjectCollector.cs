using UnityEngine;
using System.Collections.Generic;

public class ObjectCollector : MonoBehaviour
{
    [Header("Name Prefixes to Search For")]
    public string[] namePrefixes = new string[]
    {
        "xCommunication",
        "xControl", 
        "xLiving",
        "xKitchen",
        "xMedical",
        "xPower",
        "xThermal",
        "xWaste",
        "xWater-Air",
        "xStorage"
    };

    private string[] collectedObjectNames;
    private bool needsUpdate = true;
    [SerializeField] public ApiIntegration apiIntegration;

    private void FixedUpdate(){
            UpdateObjectNamesArray();
    }

    private void UpdateObjectNamesArray()
    {
        List<string> objectNames = new List<string>();
        
        // Get all GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        
        foreach (GameObject obj in allObjects)
        {
            // Check if object name starts with any of our prefixes
            foreach (string prefix in namePrefixes)
            {
                if (obj.name.StartsWith(prefix))
                {
                    objectNames.Add(obj.name);
                }
            }
        }
        
        collectedObjectNames = objectNames.ToArray();
        
        // Debug log to verify collection
        Debug.Log($"Collected {collectedObjectNames.Length} objects with specified prefixes");
        foreach (string name in collectedObjectNames)
        {
            Debug.Log($"Found the element: {name}");
        }
    }

    // Call this method when your button is pressed
    public void OnButtonPressed()
    {
        // Ensure we have the latest data
        UpdateObjectNamesArray();

        apiIntegration.listOfElements = collectedObjectNames;
    }

    // Call this if scene objects change and you need to refresh
    public void MarkForUpdate()
    {
        needsUpdate = true;
    }
}