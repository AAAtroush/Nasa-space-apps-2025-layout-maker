using UnityEngine;
using UnityEditor;
using System;

public class UIComponentCopier : EditorWindow
{
    public GameObject reference;
    public string tags = "";

    [MenuItem("Tools/Copy All UI Components By Tag")]
    public static void ShowWindow()
    {
        GetWindow<UIComponentCopier>("UI Component Copier");
    }

    private void OnGUI()
    {
        GUILayout.Label("Copy ALL Components from Reference to Tagged Objects", EditorStyles.boldLabel);

        reference = (GameObject)EditorGUILayout.ObjectField("Reference Object", reference, typeof(GameObject), true);
        tags = EditorGUILayout.TextField("Target Tags (comma-separated)", tags);

        if (GUILayout.Button("Apply To Tagged Objects"))
        {
            ApplyComponents();
        }
    }

    private void ApplyComponents()
    {
        if (reference == null)
        {
            Debug.LogWarning("No reference object assigned!");
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
                CopyAllComponents(reference, target);
            }
        }

        Debug.Log("✅ Applied all components from reference to tagged objects.");
    }

    private void CopyAllComponents(GameObject source, GameObject destination)
    {
        Component[] sourceComponents = source.GetComponents<Component>();

        foreach (Component srcComp in sourceComponents)
        {
            if (srcComp == null) continue;

            Type type = srcComp.GetType();
            Component destComp = destination.GetComponent(type);

            if (destComp == null)
            {
                destComp = destination.AddComponent(type);
            }

            // Copy serialized values
            EditorUtility.CopySerialized(srcComp, destComp);
        }
    }
}
