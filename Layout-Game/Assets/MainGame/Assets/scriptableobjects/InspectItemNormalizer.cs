using UnityEngine;

public class InspectItemNormalizer : MonoBehaviour
{
    [HideInInspector] public Camera inspectCamera;

    void Start()
    {
        Normalize();
    }

    void Normalize()
    {
        if (inspectCamera == null) return;

        // Collect bounds from renderers
        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);

        // Center model at spawn point
        Vector3 offset = transform.position - bounds.center;
        transform.position += offset;

        // Scale model to fit in camera view
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        float distance = Vector3.Distance(inspectCamera.transform.position, transform.position);
        float viewHeight = 2f * distance * Mathf.Tan(inspectCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);

        float scale = (viewHeight * 0.4f) / maxSize; // 0.4f = fit nicely
        transform.localScale *= scale;

        // Reset rotation
        transform.rotation = Quaternion.identity;
    }
}
