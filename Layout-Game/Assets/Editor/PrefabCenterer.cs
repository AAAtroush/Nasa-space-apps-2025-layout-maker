using UnityEngine;
using UnityEditor;

public class PrefabCenterer : EditorWindow
{
    string sourceFolder = "Assets/Prefabs"; 
    string saveFolder = "Assets/Prefabs/Centered"; 

    [MenuItem("Tools/Center Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<PrefabCenterer>("Center Prefabs");
    }

    void OnGUI()
    {
        GUILayout.Label("Center Prefabs Tool", EditorStyles.boldLabel);

        sourceFolder = EditorGUILayout.TextField("Source Folder:", sourceFolder);
        saveFolder = EditorGUILayout.TextField("Save Folder:", saveFolder);

        if (GUILayout.Button("Center Prefabs"))
        {
            CenterPrefabsInFolder();
        }
    }

    void CenterPrefabsInFolder()
    {
        if (!AssetDatabase.IsValidFolder(saveFolder))
        {
            string parent = "Assets";
            foreach (string part in saveFolder.Replace("Assets/", "").Split('/'))
            {
                if (!AssetDatabase.IsValidFolder(parent + "/" + part))
                    AssetDatabase.CreateFolder(parent, part);
                parent += "/" + part;
            }
        }

        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { sourceFolder });
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (!prefab) continue;

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                DestroyImmediate(instance);
                continue;
            }

            Bounds bounds = renderers[0].bounds;
            foreach (Renderer r in renderers) bounds.Encapsulate(r.bounds);
            Vector3 center = bounds.center;

            GameObject parent = new GameObject(prefab.name + "_Centered");
            instance.transform.SetParent(parent.transform);
            instance.transform.position -= center;

            string newPath = saveFolder + "/" + prefab.name + "_Centered.prefab";
            PrefabUtility.SaveAsPrefabAsset(parent, newPath);

            DestroyImmediate(parent);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("✅ Centered prefabs saved to " + saveFolder);
    }
}
