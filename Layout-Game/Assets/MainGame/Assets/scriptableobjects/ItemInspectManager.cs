using UnityEngine;

public class ItemInspectManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    private GameObject currentInstance;

    [Header("Rotation Settings")]
    public float rotationSpeed = 1f; // tweak sensitivity

    private Vector3 currentRotation = Vector3.zero;

    public void ShowItem(ItemData data)
    {
        Clear();

        if (data != null && data.prefab != null && spawnPoint != null)
        {
            currentInstance = Instantiate(data.prefab, spawnPoint.position, Quaternion.identity, spawnPoint);
            currentInstance.transform.localPosition = Vector3.zero;
            currentInstance.transform.localRotation = Quaternion.identity;
            currentInstance.transform.localScale = data.overrideScale;

            currentRotation = Vector3.zero;
        }
    }

    public void Clear()
    {
        if (currentInstance != null)
        {
            Destroy(currentInstance);
            currentInstance = null;
        }
    }

    private void Update()
    {
        if (currentInstance == null) return;

        // Rotate only while holding left mouse
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // ✅ Properly inverted up/down
            currentRotation.x += mouseY * rotationSpeed;  // flipped sign here
            currentRotation.y -= mouseX * rotationSpeed;  // keep left/right normal

            currentInstance.transform.rotation = Quaternion.Euler(currentRotation);
        }
    }
}
