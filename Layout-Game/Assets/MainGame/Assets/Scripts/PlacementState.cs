using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData roomData;
    GridData stationData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO database,
                          GridData roomData,
                          GridData stationData,
                          ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.roomData = roomData;
        this.stationData = stationData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectsData[selectedObjectIndex].Prefab,
                database.objectsData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No object with ID {iD}");
        }

    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID <= 17 ? roomData : stationData;
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    // In PlacementState.cs - Update the AddObjectAt call:
    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            return;
        }

        int roomID = -1;
        int index = -1;


        if (database.objectsData[selectedObjectIndex].ID <= 17) // If it's a room
        {
            // FIRST: Add to roomData to get the room ID
            roomID = roomData.AddObjectAt(gridPosition,
                database.objectsData[selectedObjectIndex].Size,
                database.objectsData[selectedObjectIndex].ID,
                -1, // Temporary index
                true);

            // SECOND: Place the object with the correct room ID
            index = objectPlacer.PlaceObject(
                database.objectsData[selectedObjectIndex].Prefab,
                grid.CellToWorld(gridPosition),
                true, // isRoom = true
                roomID); // Pass the room ID

            // THIRD: Update roomData with the correct object index
            roomData.UpdatePlacedObjectIndex(roomID, index);
        }
        else // It's a station
        {
            index = objectPlacer.PlaceObject(
                database.objectsData[selectedObjectIndex].Prefab,
                grid.CellToWorld(gridPosition),
                false); // isRoom = false

            stationData.AddObjectAt(gridPosition,
                database.objectsData[selectedObjectIndex].Size,
                database.objectsData[selectedObjectIndex].ID,
                index);
        }

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}