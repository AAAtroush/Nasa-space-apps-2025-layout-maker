using UnityEngine;
using UnityEditor;

public class RectTransformCopier : EditorWindow
{
    public RectTransform reference;
    public string tags = "";

    [MenuItem("Tools/Copy RectTransform By Tag")]
    public static void ShowWindow()
    {
        GetWindow<RectTransformCopier>("RectTransform Copier");
    }

    private void OnGUI()
    {
        GUILayout.Label("Copy RectTransform from Reference to Tagged Objects", EditorStyles.boldLabel);

        reference = (RectTransform)EditorGUILayout.ObjectField("Reference RectTransform", reference, typeof(RectTransform), true);
        tags = EditorGUILayout.TextField("Target Tags (comma-separated)", tags);

        if (GUILayout.Button("Apply RectTransform"))
        {
            ApplyRectTransform();
        }
    }

    private void ApplyRectTransform()
    {
        if (reference == null)
        {
            Debug.LogWarning("No reference RectTransform assigned!");
            return;
        }

        string[] splitTags = tags.Split(',');
        foreach (string rawTag in splitTags)
        {
            string trimmedTag = rawTag.Trim();
            if (string.IsNullOrEmpty(trimmedTag)) continue;

            GameObject[] targets = GameObject.FindGameObjectsWithTag(trimmedTag);
            if (targets.Length == 0)
            {
                Debug.LogWarning("No objects found with tag: " + trimmedTag);
                continue;
            }

            foreach (GameObject target in targets)
            {
                RectTransform rt = target.GetComponent<RectTransform>();
                if (rt != null)
                {
                    Undo.RecordObject(rt, "Copy RectTransform");

                    rt.anchorMin = reference.anchorMin;
                    rt.anchorMax = reference.anchorMax;
                    rt.pivot = reference.pivot;
                    rt.anchoredPosition = reference.anchoredPosition;
                    rt.sizeDelta = reference.sizeDelta;
                    rt.localRotation = reference.localRotation;
                    rt.localScale = reference.localScale;
                }
            }
        }

        Debug.Log("Applied RectTransform to tagged objects.");
    }
}
