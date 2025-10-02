using UnityEngine;

public class ItemInspectManager : MonoBehaviour
{
    public Camera inspectCamera;         // drag your special camera here
    public RenderTexture renderTexture;  // assign a RenderTexture for RawImage
    public Transform spawnPoint;         // empty child where prefabs will spawn

    private GameObject currentInstance;

    // Show the prefab from ItemData
    public void ShowItem(ItemData data)
    {
        Clear(); // clear any old model

        if (data.prefab != null)
        {
            currentInstance = Instantiate(data.prefab, spawnPoint.position, Quaternion.identity, spawnPoint);

            // adjust camera distance if ItemData has value
            if (inspectCamera != null)
            {
                inspectCamera.targetTexture = renderTexture;
                inspectCamera.transform.position = spawnPoint.position - inspectCamera.transform.forward * data.cameraDistance;
            }
        }
    }

    // Clear any existing instance
    public void Clear()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }
    }
}
