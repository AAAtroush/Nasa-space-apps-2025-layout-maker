using System;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();
    Dictionary<int, List<Vector3Int>> roomOccupiedPositions = new();
    Dictionary<int, string> roomTypes = new(); // NEW: Store room types by ID
    private int nextRoomID = 0;

    private GameManager gameManager;
    private Vector2Int gridSize;

    public GridData(GameManager gm, Vector2Int size)
    {
        gameManager = gm;
        gridSize = size;
    }

    // NEW: Update placed object index for a room
    // NEW: Update placed object index for a room
    public void UpdatePlacedObjectIndex(int roomID, int placedObjectIndex)
    {
        // Create a list of keys to update first to avoid modifying during enumeration
        List<Vector3Int> keysToUpdate = new List<Vector3Int>();

        // First pass: find all keys that need updating
        foreach (var kvp in placedObjects)
        {
            if (kvp.Value.RoomID == roomID)
            {
                keysToUpdate.Add(kvp.Key);
            }
        }

        // Second pass: update the values
        foreach (var key in keysToUpdate)
        {
            if (placedObjects.ContainsKey(key))
            {
                var oldData = placedObjects[key];
                var newData = new PlacementData(
                    oldData.occupiedPositions,
                    oldData.ID,
                    placedObjectIndex,
                    oldData.RoomID
                );
                placedObjects[key] = newData;
            }
        }
    }

    public int AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int ID, int placedObjectIndex, bool isRoom = false)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        int roomID = isRoom ? nextRoomID++ : -1;
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex, roomID);

        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObjects[pos] = data;
        }

        if (isRoom)
        {
            roomOccupiedPositions[roomID] = new List<Vector3Int>(positionToOccupy);
            roomTypes[roomID] = "Untagged"; // Initialize as untagged
        }

        return roomID;
    }

    // NEW: Set room type for a room ID
    public void SetRoomType(int roomID, string roomType)
    {
        if (roomTypes.ContainsKey(roomID))
        {
            roomTypes[roomID] = roomType;

        }
    }

    // NEW: Get room type by room ID
    public string GetRoomType(int roomID)
    {


        if (roomTypes.ContainsKey(roomID))
        {
            string roomType = roomTypes[roomID];

            return roomType;
        }


        return "Unknown";
    }
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        if (gameManager.numberOfRooms == 10 && gameManager.isRoom)
        {
            return false;
        }
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (pos.x < 0 || pos.x >= gridSize.x || pos.z < 0 || pos.z >= gridSize.y)
            {
                return false;
            }
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }

    // NEW: Check if position is inside a specific room
    public bool IsPositionInRoom(Vector3Int gridPosition, int roomID)
    {
        return roomOccupiedPositions.ContainsKey(roomID) &&
               roomOccupiedPositions[roomID].Contains(gridPosition);
    }

    // NEW: Get room ID at position (returns -1 if no room)
    public int GetRoomIDAtPosition(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) && placedObjects[gridPosition].RoomID >= 0)
        {
            return placedObjects[gridPosition].RoomID;
        }
        return -1;
    }

    // NEW: Get all room IDs
    public List<int> GetAllRoomIDs()
    {
        return new List<int>(roomOccupiedPositions.Keys);
    }

}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }
    public int RoomID { get; private set; } // -1 for stations, >=0 for rooms

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex, int roomID = -1)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
        RoomID = roomID;
    }
}