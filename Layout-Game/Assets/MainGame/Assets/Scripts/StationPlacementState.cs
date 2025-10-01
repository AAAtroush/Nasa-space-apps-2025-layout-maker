using System.Collections.Generic;
using UnityEngine;

public class StationPlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    private int stationID;
    private Grid grid;
    private PreviewSystem previewSystem;
    private ObjectsDatabaseSO database;
    private GridData roomData;
    private GridData stationData;
    private ObjectPlacer objectPlacer;
    private int targetRoomID;
    private RoomTagManager roomTagManager;
    private PlacementSystem placementSystem; // ADD THIS

    public StationPlacementState(int iD, int roomID,
                                  Grid grid,
                                  PreviewSystem previewSystem,
                                  ObjectsDatabaseSO database,
                                  GridData roomData,
                                  GridData stationData,
                                  ObjectPlacer objectPlacer)
    {
        stationID = iD;
        targetRoomID = roomID;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.roomData = roomData;
        this.stationData = stationData;
        this.objectPlacer = objectPlacer;
        this.roomTagManager = Object.FindFirstObjectByType<RoomTagManager>();
        this.placementSystem = Object.FindFirstObjectByType<PlacementSystem>(); // ADD THIS

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == stationID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size);
        }
        else
        {
        }
        DebugRoomInfo();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        // Check if position is within the target room
        if (!roomData.IsPositionInRoom(gridPosition, targetRoomID))
        {
            return false;
        }

        // NEW: Check if station type is allowed in this SPECIFIC room type
        if (!IsStationAllowedInTargetRoom())
        {
            return false;
        }

        // Check if station can be placed here (no collisions)
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, database.objectsData[selectedObjectIndex].Size);
        foreach (var pos in positionToOccupy)
        {
            if (!roomData.IsPositionInRoom(pos, targetRoomID) || stationData.CanPlaceObjectAt(pos, Vector2Int.one) == false)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsStationAllowedInTargetRoom()
    {
        if (roomTagManager == null)
        {
            Debug.LogError("RoomTagManager not found!");
            return true;
        }

        // FIXED: Use GridData to get the room type
        string roomType = roomData.GetRoomType(targetRoomID);


        bool canPlace = roomTagManager.CanPlaceStationInRoom(stationID, roomType);
        return canPlace;
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

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            return;
        }

        int index = objectPlacer.PlaceObject(database.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition), false);

        stationData.AddObjectAt(gridPosition,
            database.objectsData[selectedObjectIndex].Size,
            database.objectsData[selectedObjectIndex].ID,
            index,
            false); // isRoom = false for stations

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }

    private void DebugRoomInfo()
    {

        // Get all rooms and their IDs
        RoomController[] allRooms = Object.FindObjectsByType<RoomController>(FindObjectsSortMode.None);

        foreach (RoomController room in allRooms)
        {
            Vector3Int roomGridPos = grid.WorldToCell(room.transform.position);
            int roomIdAtPos = roomData.GetRoomIDAtPosition(roomGridPos);
        }
    }
}