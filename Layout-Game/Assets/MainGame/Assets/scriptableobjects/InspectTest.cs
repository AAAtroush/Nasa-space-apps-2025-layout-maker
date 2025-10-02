using UnityEngine;

public class InspectTest : MonoBehaviour
{
    public Camera inspectCamera;
    public Transform spawnPoint;
    public GameObject testPrefab;

    private GameObject currentItem;

    void Start()
    {
        SpawnTest();
    }

    void SpawnTest()
    {
        if (currentItem != null)
            Destroy(currentItem);

        currentItem = Instantiate(testPrefab, spawnPoint.position, Quaternion.identity);
        currentItem.transform.LookAt(inspectCamera.transform);
    }
}
