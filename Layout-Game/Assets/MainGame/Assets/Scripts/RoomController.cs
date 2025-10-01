using UnityEngine;
using System.Collections.Generic;

public class RoomController : MonoBehaviour
{
    private List<Renderer> roomRenderers = new List<Renderer>();
    private List<Material[]> roomMaterials = new List<Material[]>();
    private bool isPlacedRoom = false;

    public string CurrentTag { get; private set; } = "Untagged";
    public Color CurrentColor { get; private set; }
    public int RoomID { get; private set; } = -1; // NEW: Store room ID

    // NEW: Debug method to show room info

    // NEW: Initialize with room ID
    public void InitializeAsPlacedRoom(int roomID)
    {
        if (isPlacedRoom) return;

        isPlacedRoom = true;
        RoomID = roomID; // Store the room ID

        roomRenderers.AddRange(GetComponentsInChildren<Renderer>());


        foreach (Renderer renderer in roomRenderers)
        {
            Material[] originalMats = renderer.materials;
            Material[] newMats = new Material[originalMats.Length];

            for (int i = 0; i < originalMats.Length; i++)
            {
                newMats[i] = new Material(originalMats[i]);
            }

            renderer.materials = newMats;
            roomMaterials.Add(newMats);
        }
    }

    // UPDATED: Set room tag and update GridData
    public void SetRoomTag(string tagName, Color color)
    {
        if (!isPlacedRoom)
        {
            return;
        }

        CurrentTag = tagName;
        CurrentColor = color;
        gameObject.tag = tagName;

        // NEW: Update GridData with the room type
        UpdateGridDataRoomType(tagName);

        foreach (Material[] materials in roomMaterials)
        {
            foreach (Material material in materials)
            {
                if (material != null)
                {
                    material.color = color;
                }
            }
        }
    }

    // NEW: Update GridData with room type
    private void UpdateGridDataRoomType(string roomType)
    {
        // Find PlacementSystem and update room type
        PlacementSystem placementSystem = FindObjectOfType<PlacementSystem>();
        if (placementSystem != null && RoomID != -1)
        {
            // This assumes you have a way to access roomData from PlacementSystem
            // You might need to add a public method to PlacementSystem or GridData

            // For now, we'll use a direct approach - you might need to adjust this
            GridData roomData = placementSystem.GetRoomData(); // You'll need to add this method
            if (roomData != null)
            {
                roomData.SetRoomType(RoomID, roomType);
            }
        }
    }

    // Keep this for backward compatibility


    private void OnDestroy()
    {
        // Clean up materials
        foreach (Material[] materials in roomMaterials)
        {
            foreach (Material material in materials)
            {
                if (material != null)
                {
                    Destroy(material);
                }
            }
        }
    }
}