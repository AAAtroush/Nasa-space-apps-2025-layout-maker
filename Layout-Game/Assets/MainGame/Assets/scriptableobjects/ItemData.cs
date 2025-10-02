using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemname;     // in-game name
    public string irlname;      // real-life name
    
    [TextArea(3, 6)]
    public string attributes;   // attributes text (multi-line)

    [TextArea(3, 6)]
    public string gameex;       // game explanation

    [TextArea(3, 6)]
    public string irlex;        // real-life explanation

    public string link;         // external link

    [Header("UI Image")]
    public Sprite image;

    [Header("3D Inspection")]
    public GameObject prefab;   // prefab to spawn in the 3D viewer
    public float cameraDistance = 2f; 
    public Vector3 overrideScale = Vector3.one * 100f;
}
