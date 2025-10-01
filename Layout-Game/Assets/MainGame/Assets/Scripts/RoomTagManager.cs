using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomTagManager : MonoBehaviour
{
    [System.Serializable]
    public class RoomTag
    {
        public string tagName;
        public Color color;
        public List<int> allowedStationIDs; // NEW: Which stations can be placed here
    }

    [SerializeField]
    private List<RoomTag> availableTags = new List<RoomTag>
    {
        new RoomTag {
            tagName = "Comms",
            color = Color.blue,
            allowedStationIDs = new List<int> { 18,19,20,21,22 } // Communication stations
        },
        new RoomTag {
            tagName = "Control",
            color = Color.red,
            allowedStationIDs = new List<int> { 23, 24, 25 } // Control stations
        },
        new RoomTag {
            tagName = "Kitchen",
            color = Color.yellow,
            allowedStationIDs = new List<int> { 26, 27, 28, 29, 30, 31, 32 } // Kitchen stations
        },
        new RoomTag {
            tagName = "Living",
            color = Color.yellow,
            allowedStationIDs = new List<int> { 33, 34, 35, 36 } // Living stations
        },
        new RoomTag {
            tagName = "Medical",
            color = Color.yellow,
            allowedStationIDs = new List<int> { 37, 38, 39, 40, 41 } // Medical stations
        },
        new RoomTag {
            tagName = "Power",
            color = Color.yellow,
            allowedStationIDs = new List<int> { 42, 43, 44, 45 } // Power stations
        },
        new RoomTag {
            tagName = "Storage",
            color = Color.yellow,
            allowedStationIDs = new List<int> { 46, 47, 48, 49 } // Storage stations
        },
        new RoomTag {
            tagName = "Thermal",
            color = Color.yellow,
            allowedStationIDs = new List<int> { 50, 51, 52 } // Thermal stations
        },
        new RoomTag {
            tagName = "Waste",
            color = Color.yellow,
            allowedStationIDs = new List<int> { 53, 54, 55 } // Waste stations
        },
        new RoomTag {
            tagName = "Water-Air",
            color = Color.yellow,
            allowedStationIDs = new List<int> { 56, 57, 58 } // Water-Air stations
        },

    };

    // NEW: Get allowed stations for a room by its tag
    public List<int> GetAllowedStationsForRoom(string roomTag)
    {
        foreach (var tag in availableTags)
        {
            if (tag.tagName == roomTag)
            {
                return tag.allowedStationIDs;
            }
        }
        return new List<int>(); // No stations allowed if tag not found
    }

    // NEW: Check if a station can be placed in a room
    public bool CanPlaceStationInRoom(int stationID, string roomTag)
    {
        

        var allowedStations = GetAllowedStationsForRoom(roomTag);
        

        return allowedStations.Contains(stationID);
    }
    [SerializeField] private LayerMask roomLayerMask;

    [SerializeField] private GameObject tagSelectionUI;
    [SerializeField] private Transform tagButtonContainer;
    [SerializeField] private Button tagButtonPrefab;

    private Camera mainCamera;
    private RoomController selectedRoom;
    private bool isTaggingMode = false;

    private void Start()
    {
        mainCamera = Camera.main;
        tagSelectionUI.SetActive(false);
        InitializeTagButtons();
    }

    private void InitializeTagButtons()
    {
        foreach (Transform child in tagButtonContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var tag in availableTags)
        {
            Button button = Instantiate(tagButtonPrefab, tagButtonContainer);
            button.image.color = tag.color;
            button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = tag.tagName;
            button.onClick.AddListener(() => AssignTagToSelectedRoom(tag));
        }
    }

    public void StartTaggingMode()
    {
        isTaggingMode = true;
        tagSelectionUI.SetActive(false);
    }

    private void Update()
    {
        if (!isTaggingMode) return;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse clicked in tagging mode");

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f, roomLayerMask);

            foreach (RaycastHit hit in hits)
            {
                // Search the entire hierarchy for RoomController
                RoomController room = hit.collider.GetComponentInParent<RoomController>();
                if (room != null)
                {
                    selectedRoom = room;
                    ShowTagSelectionUI();
                    Debug.Log($"Found room: {room.gameObject.name}, RoomID: {room.RoomID}");
                    break;
                }
            }

            if (selectedRoom == null)
            {
                Debug.Log("No RoomController found in hierarchy of clicked object");

                // Additional debug: List all objects that were hit
                foreach (RaycastHit hit in hits)
                {
                    Debug.Log($"Hit object: {hit.collider.gameObject.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopTaggingMode();
        }
    }
    // New method to search entire hierarchy for RoomController
    private RoomController FindRoomControllerInHierarchy(GameObject startObject)
    {
        Transform current = startObject.transform;

        while (current != null)
        {
            RoomController room = current.GetComponent<RoomController>();
            if (room != null)
            {
                return room;
            }
            current = current.parent;
        }
        return null;
    }

    private void ShowTagSelectionUI()
    {
        tagSelectionUI.SetActive(true);
    }

    private void AssignTagToSelectedRoom(RoomTag tag)
    {

        if (selectedRoom != null)
        {
            // Use SetRoomTag to store both the tag name AND color
            selectedRoom.SetRoomTag(tag.tagName, tag.color);
        }
        else
        {
            Debug.LogError("No room selected!");
        }

        tagSelectionUI.SetActive(false);
    }

    public void StopTaggingMode()
    {
        isTaggingMode = false;
        tagSelectionUI.SetActive(false);
        selectedRoom = null;
    }
}