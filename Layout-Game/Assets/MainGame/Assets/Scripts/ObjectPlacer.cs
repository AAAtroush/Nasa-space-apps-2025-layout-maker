using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();
    [SerializeField]
    private GameManager gameManager;

    public int PlaceObject(GameObject prefab, Vector3 position, bool isRoom = false, int roomID = -1)
    {
        if (gameManager.numberOfRooms == 10 && isRoom)
        {
            return -1;
        }
        if (gameManager.isRoom && isRoom)
        {
            gameManager.numberOfRooms++;
        }

        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;

        if (isRoom)
        {
            RoomController roomController = newObject.GetComponent<RoomController>();
            if (roomController == null)
            {
                roomController = newObject.AddComponent<RoomController>();
            }
            // UPDATED: Pass the room ID - THIS IS CRITICAL
            roomController.InitializeAsPlacedRoom(roomID);

        }

        placedGameObjects.Add(newObject);
        return placedGameObjects.Count - 1;
    }
    public GameObject GetPlacedObject(int index)
    {
        if (index >= 0 && index < placedGameObjects.Count)
        {
            return placedGameObjects[index];
        }
        return null;
    }

    // Overload for backward compatibility
    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        return PlaceObject(prefab, position, gameManager.isRoom, -1);
    }
}