using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [Min(1)] public int width = 8;
    [Min(1)] public int height = 6;
    [Min(0.1f)] public float cellSize = 2f;
    public Vector2 origin = Vector2.zero;

    [Header("Prefabs")]
    public GameObject roomPrefab; // must have a Room script

    [Header("Debug")]
    public bool generateOnPlay = true;

    private Room[,] rooms;
    private DSU dsu;
    public Dictionary<int, Color> groupColors = new Dictionary<int, Color>();


    // RoomTag colors
    private Dictionary<Room.RoomTag, Color> tagColors = new Dictionary<Room.RoomTag, Color>()

    {
        { Room.RoomTag.Empty, Color.white },
        { Room.RoomTag.Corridor, Color.gray },
        { Room.RoomTag.Treasure, Color.yellow },
        { Room.RoomTag.Boss, Color.red },
        { Room.RoomTag.Start, Color.green }
    };
    public void UpdateGroupColor(Room anyRoomInGroup)
    {
        int leader = DSU.Find(ToIndex(anyRoomInGroup.gridPos));

        // Collect all rooms in the group
        List<Room> groupRooms = new List<Room>();
        foreach (Room r in GetAllRooms())
        {
            if (DSU.Find(ToIndex(r.gridPos)) == leader)
                groupRooms.Add(r);
        }

        // Decide color based on group size
        Color newColor = Color.white; // default

        if (groupRooms.Count == 2)
        {
            newColor = new Color(Random.value, Random.value, Random.value); // random
        }
        else if (groupRooms.Count == 1)
        {
            newColor = Color.white;
        }
        else
        {
            // >2: keep previous color if exists
            if (groupColors.TryGetValue(leader, out Color prev))
                newColor = prev;
        }

        // Apply color and save it
        groupColors[leader] = newColor;
        foreach (Room r in groupRooms)
        {
            r.SetColor(newColor);
        }
    }


    private void Start()
    {
        if (Application.isPlaying && generateOnPlay)
            Generate();
    }

    public void SetRoomColor(Room room, Color color)
    {
        int leader = dsu.Find(ToIndex(room.gridPos));
        groupColors[leader] = color; // save the group color

        foreach (Room r in rooms)
        {
            if (dsu.Find(ToIndex(r.gridPos)) == leader)
                r.SetColor(color);
        }
    }


    public void Generate()
    {
        ClearImmediate();

        rooms = new Room[width, height];
        dsu = new DSU(width * height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = CellCenter(new Vector2Int(x, y));
                GameObject obj;

                if (roomPrefab != null)
                    obj = Instantiate(roomPrefab, pos, Quaternion.identity, transform);
                else
                {
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.AddComponent<BoxCollider>();
                    obj.transform.SetParent(transform, false);
                    obj.transform.position = pos;
                }

                Room room = obj.GetComponent<Room>();
                if (room == null) room = obj.AddComponent<Room>();

                room.gridPos = new Vector2Int(x, y);
                room.SetColor(tagColors[room.tag]);
                rooms[x, y] = room;

                obj.name = $"Room_{x}_{y}";
                obj.transform.localScale = new Vector3(cellSize * 0.95f, 0.2f, cellSize * 0.95f);
            }
        }

        // Setup neighbours
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Room r = rooms[x, y];
                r.neighbours.Clear();

                TryAddNeighbour(r, x + 1, y);
                TryAddNeighbour(r, x - 1, y);
                TryAddNeighbour(r, x, y + 1);
                TryAddNeighbour(r, x, y - 1);
            }
        }
    }

    private void TryAddNeighbour(Room r, int nx, int ny)
    {
        if (nx < 0 || ny < 0 || nx >= width || ny >= height) return;
        r.neighbours.Add(rooms[nx, ny]);
    }

    // DSU index conversion
    public int ToIndex(Vector2Int pos) => pos.x * height + pos.y;

    public void Merge(Room a, Room b)
    {
        if (!AreGroupsNeighbors(a, b)) return;

        int leaderA = dsu.Find(ToIndex(a.gridPos));
        int leaderB = dsu.Find(ToIndex(b.gridPos));
        dsu.Union(leaderA, leaderB);
        int newLeader = dsu.Find(leaderA);

        // Let the new group decide its color automatically
        UpdateGroupColor(a);
    }


    private bool AreGroupsNeighbors(Room a, Room b)
    {
        int leaderA = dsu.Find(ToIndex(a.gridPos));
        int leaderB = dsu.Find(ToIndex(b.gridPos));

        foreach (Room r in rooms)
        {
            if (dsu.Find(ToIndex(r.gridPos)) != leaderA) continue;

            foreach (Room n in r.neighbours)
            {
                if (dsu.Find(ToIndex(n.gridPos)) == leaderB)
                    return true;
            }
        }
        return false;
    }

    public void SetRoomTag(Room r, Room.RoomTag newTag)
    {
        r.tag = newTag;
        r.SetColor(tagColors[newTag]);
    }

    public Vector3 CellCenter(Vector2Int pos)
    {
        return new Vector3(origin.x + pos.x * cellSize, 0f, origin.y + pos.y * cellSize);
    }

    public void ClearImmediate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);
            return;
        }
#endif
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);
    }
    public DSU DSU => dsu;
    public Room[] GetAllRooms()
    {
        List<Room> list = new List<Room>();
        foreach (Room r in rooms) list.Add(r);
        return list.ToArray();
    }

    public int ToIndexPublic(Vector2Int pos) => ToIndex(pos);
}

// --- DSU (Disjoint Set Union) ---
public class DSU
{
    private int[] parent;
    private int[] size;

    public DSU(int n)
    {
        parent = new int[n];
        size = new int[n];
        for (int i = 0; i < n; i++)
        {
            parent[i] = i;
            size[i] = 1;
        }
    }

    public int Find(int x)
    {
        if (parent[x] != x) parent[x] = Find(parent[x]);
        return parent[x];
    }

    public void Union(int a, int b)
    {
        a = Find(a);
        b = Find(b);
        if (a == b) return;

        if (size[a] < size[b]) (a, b) = (b, a);
        parent[b] = a;
        size[a] += size[b];
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var gm = (GridManager)target;
        EditorGUILayout.Space();
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Generate Grid"))
            {
                Undo.RegisterFullObjectHierarchyUndo(gm.gameObject, "Generate Grid");
                gm.Generate();
                EditorUtility.SetDirty(gm);
            }
            if (GUILayout.Button("Clear"))
            {
                Undo.RegisterFullObjectHierarchyUndo(gm.gameObject, "Clear Grid");
                gm.ClearImmediate();
                EditorUtility.SetDirty(gm);
            }
        }
    }
}
#endif
