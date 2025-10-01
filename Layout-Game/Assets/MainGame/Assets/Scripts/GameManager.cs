using TMPro;
using UnityEngine;
using System.Collections.Generic;

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

    // Station placement tracking
    private List<int> availableRooms = new List<int>();
    private int currentRoomIndex = 0;
    private PlacementSystem placementSystem;

    void Start()
    {
        placementSystem = FindObjectOfType<PlacementSystem>();
        UpdatePhaseUI();
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
            roomTagManager.StopTaggingMode();
            currentPhase = GamePhase.StationPlacement;

            // Get available rooms from placement system
            availableRooms = placementSystem.GetAvailableRooms();
            if (availableRooms.Count > 0)
            {
                currentRoomIndex = 0;

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
            UpdatePhaseUI();
        }
    }

    public void PreviousRoom()
    {
        if (availableRooms.Count > 1)
        {
            currentRoomIndex = (currentRoomIndex - 1 + availableRooms.Count) % availableRooms.Count;
            UpdatePhaseUI();
        }
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