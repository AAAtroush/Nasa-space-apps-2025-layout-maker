using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public int numberOfRooms = 0;
    [SerializeField] public TextMeshProUGUI counterText;
    [SerializeField] public bool isRoom = false;
    [SerializeField] public GameObject toNextStageFromRoom;
    [SerializeField] private RoomTagManager roomTagManager;

    // NEW: Station placement phase
    [SerializeField] public GameObject toNextStageFromTagging;
    [SerializeField] public TextMeshProUGUI phaseText;
    [SerializeField] public TextMeshProUGUI currentRoomText;

    private enum GamePhase { RoomPlacement, RoomTagging, StationPlacement }
    private GamePhase currentPhase = GamePhase.RoomPlacement;

    [SerializeField] GameObject roomControlButtons;

    [SerializeField] GameObject uiForRooms;
    [SerializeField] GameObject uiForObjects;

    [SerializeField] GameObject finish;


    // Station placement tracking
    private List<int> availableRooms = new List<int>();
    private int currentRoomIndex = 0;
    private PlacementSystem placementSystem;


    private Dictionary<int, CinemachineCamera> roomCameras = new Dictionary<int, CinemachineCamera>();
    private CinemachineCamera currentActiveCamera;

    void Start()
    {
        placementSystem = FindObjectOfType<PlacementSystem>();
        UpdatePhaseUI();

        InitializeRoomCameras();
    }

    private void InitializeRoomCameras()
    {
        roomCameras.Clear();

        // Get all placed rooms and find their virtual cameras
        RoomController[] allRooms = FindObjectsByType<RoomController>(FindObjectsSortMode.None);
        foreach (RoomController room in allRooms)
        {
            if (room.RoomID != -1)
            {
                CinemachineCamera vcam = room.GetComponentInChildren<CinemachineCamera>();
                if (vcam != null)
                {
                    roomCameras[room.RoomID] = vcam;
                    // Initially set all cameras to low priority
                    vcam.Priority = 1;
                }
            }
        }
    }

    private void Update()
    {
        counterText.text = $"Rooms: {numberOfRooms}/10";

        if (currentPhase == GamePhase.RoomPlacement && numberOfRooms >= 10)
        {
            toNextStageFromRoom.SetActive(true);
        }
    }

    public void StartRoomTaggingPhase()
    {
        if (currentPhase == GamePhase.RoomPlacement && roomTagManager != null)
        {
            placementSystem.StopPlacement();
            roomTagManager.StartTaggingMode();
            currentPhase = GamePhase.RoomTagging;
            UpdatePhaseUI();
        }
    }

    public void StartStationPlacementPhase()
    {
        if (currentPhase == GamePhase.RoomTagging)
        {
            finish.SetActive(true);
            roomTagManager.StopTaggingMode();
            currentPhase = GamePhase.StationPlacement;

            roomControlButtons.SetActive(true);
            uiForRooms.SetActive(false);
            uiForObjects.SetActive(true);
            // Get available rooms from placement system
            availableRooms = placementSystem.GetAvailableRooms();
            if (availableRooms.Count > 0)
            {
                currentRoomIndex = 0;

                InitializeRoomCameras();
                SwitchToRoomCamera(availableRooms[currentRoomIndex]);
                // DEBUG: Log all available rooms and their types
                foreach (int roomID in availableRooms)
                {
                    string roomType = placementSystem.GetRoomType(roomID);
                }
            }
            else
            {
                Debug.LogError("No available rooms for station placement!");
            }

            UpdatePhaseUI();
            isRoom = false; // Important: Set to false for station placement
        }
    }

    public void NextRoom()
    {
        if (availableRooms.Count > 1)
        {
            currentRoomIndex = (currentRoomIndex + 1) % availableRooms.Count;
            SwitchToRoomCamera(availableRooms[currentRoomIndex]);
            UpdatePhaseUI();
        }
    }

    public void PreviousRoom()
    {
        if (availableRooms.Count > 1)
        {
            currentRoomIndex = (currentRoomIndex - 1 + availableRooms.Count) % availableRooms.Count;
            SwitchToRoomCamera(availableRooms[currentRoomIndex]);
            UpdatePhaseUI();
        }
    }

    private void SwitchToRoomCamera(int roomID)
    {
        // Deactivate current camera
        if (currentActiveCamera != null)
        {
            currentActiveCamera.Priority = 1;
        }

        // Activate new camera
        if (roomCameras.ContainsKey(roomID))
        {
            currentActiveCamera = roomCameras[roomID];
            currentActiveCamera.Priority = 11; // Higher priority to take over
        }
        else
        {
            Debug.LogWarning($"No camera found for room ID: {roomID}");

            // Try to find the camera if it wasn't initialized
            RoomController room = FindRoomByID(roomID);
            if (room != null)
            {
                CinemachineCamera vcam = room.GetComponentInChildren<CinemachineCamera>();
                if (vcam != null)
                {
                    roomCameras[roomID] = vcam;
                    currentActiveCamera = vcam;
                    currentActiveCamera.Priority = 11;
                }
            }
        }
    }

    private RoomController FindRoomByID(int roomID)
    {
        RoomController[] allRooms = FindObjectsByType<RoomController>(FindObjectsSortMode.None);
        foreach (RoomController room in allRooms)
        {
            if (room.RoomID == roomID)
            {
                return room;
            }
        }
        return null;
    }

    private void UpdatePhaseUI()
    {
        phaseText.text = $"Phase: {currentPhase}";

        if (currentPhase == GamePhase.StationPlacement && availableRooms.Count > 0)
        {
            currentRoomText.text = $"Room: {availableRooms[currentRoomIndex] + 1}";
        }
        else
        {
            currentRoomText.text = "";
        }

        toNextStageFromRoom.SetActive(currentPhase == GamePhase.RoomPlacement && numberOfRooms >= 10);
        toNextStageFromTagging.SetActive(currentPhase == GamePhase.RoomTagging);
    }

    public void StartPlacement(int ID)
    {
        if (currentPhase == GamePhase.RoomPlacement)
        {
            placementSystem.StartPlacement(ID);
        }
        else if (currentPhase == GamePhase.StationPlacement && availableRooms.Count > 0)
        {
            placementSystem.StartStationPlacement(ID, availableRooms[currentRoomIndex]);
        }
        else
        {
            Debug.LogError($"Cannot start placement: Phase={currentPhase}, AvailableRooms={availableRooms.Count}");
        }
    }
}