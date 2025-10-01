using System;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField]
    private GameObject gridVisualization;

    [SerializeField]
    private Vector2Int gridSize = new Vector2Int(60, 30);

    private GridData roomData, stationData;

    [SerializeField] private PreviewSystem preview;
    [SerializeField] private GameManager gameManager;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] private ObjectPlacer objectPlacer;

    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        roomData = new GridData(gameManager, gridSize);
        stationData = new GridData(gameManager, gridSize);
    }

    // NEW: Public method to access roomData
    public GridData GetRoomData()
    {
        return roomData;
    }

    // NEW: Public method to update room type
    public void SetRoomType(int roomID, string roomType)
    {
        roomData.SetRoomType(roomID, roomType);
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID,
                                           grid,
                                           preview,
                                           database,
                                           roomData,
                                           stationData,
                                           objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    // NEW: Start station placement within a specific room
    public void StartStationPlacement(int stationID, int roomID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new StationPlacementState(stationID, roomID,
                                           grid,
                                           preview,
                                           database,
                                           roomData,
                                           stationData,
                                           objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);
    }

    public void StopPlacement()
    {
        if (buildingState == null)
        {
            return;
        }
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    void Update()
    {
        if (buildingState == null)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        if (gridPosition != lastDetectedPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }
    }

    // NEW: Get all available rooms for station placement
    public List<int> GetAvailableRooms()
    {
        return roomData.GetAllRoomIDs();
    }

    public string GetRoomType(int roomID)
    {
        return roomData.GetRoomType(roomID);
    }
}