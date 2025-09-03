using System.Collections.Generic;
using UnityEngine;

public class RoomSelector : MonoBehaviour
{
    public GridManager gridManager;
    public KeyCode mergeKey = KeyCode.Space;

    private HashSet<Room> selectedRooms = new HashSet<Room>();

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Room r = hit.collider.GetComponent<Room>();
                if (r != null)
                    ToggleRoomSelection(r);
            }
        }

        if (Input.GetKeyDown(mergeKey) && selectedRooms.Count >= 2)
        {
            MergeSelectedGroups();
            ClearSelection();
        }
    }

    private void ToggleRoomSelection(Room r)
    {
        if (selectedRooms.Contains(r))
        {
            selectedRooms.Remove(r);
            RestoreRoomGroupColor(r);
        }
        else
        {
            selectedRooms.Add(r);
            HighlightGroup(r, new Color32(143, 143, 143, 150));
        }
    }

    private void HighlightGroup(Room r, Color highlightColor)
    {
        int leader = gridManager.DSU.Find(gridManager.ToIndexPublic(r.gridPos));

        foreach (Room room in gridManager.GetAllRooms())
        {
            if (gridManager.DSU.Find(gridManager.ToIndexPublic(room.gridPos)) == leader)
                room.Highlight(highlightColor);
        }
    }

    private void RestoreRoomGroupColor(Room r)
    {
        int leader = gridManager.DSU.Find(gridManager.ToIndexPublic(r.gridPos));
        foreach (Room room in gridManager.GetAllRooms())
        {
            if (gridManager.DSU.Find(gridManager.ToIndexPublic(room.gridPos)) == leader)
            {
                if (gridManager.groupColors.TryGetValue(leader, out Color c))
                    room.SetColor(c);
                else
                    room.Unhighlight();
            }
        }
    }

    private void MergeSelectedGroups()
    {
        if (selectedRooms.Count == 0) return;

        HashSet<Room> visited = new HashSet<Room>();
        Room start = null;

        // pick any room from the selection as start
        foreach (Room r in selectedRooms) { start = r; break; }

        if (start == null) return;

        DFSMerge(start, visited);

        // after DFS merge, group colors will be updated automatically
    }

    // DFS recursive merge
    private void DFSMerge(Room current, HashSet<Room> visited)
    {
        visited.Add(current);

        foreach (Room n in current.neighbours)
        {
            if (!selectedRooms.Contains(n)) continue; // only merge rooms that were selected
            if (visited.Contains(n)) continue;

            // merge current and neighbor
            gridManager.Merge(current, n);

            DFSMerge(n, visited);
        }
    }


    public bool AreGroupsNeighbors(Room a, Room b)
    {
        int leaderA = gridManager.DSU.Find(gridManager.ToIndexPublic(a.gridPos));
        int leaderB = gridManager.DSU.Find(gridManager.ToIndexPublic(b.gridPos));

        foreach (Room r in gridManager.GetAllRooms())
        {
            if (gridManager.DSU.Find(gridManager.ToIndexPublic(r.gridPos)) != leaderA) continue;

            foreach (Room n in r.neighbours)
            {
                if (gridManager.DSU.Find(gridManager.ToIndexPublic(n.gridPos)) == leaderB)
                    return true;
            }
        }
        return false;
    }

    private void ClearSelection()
    {
        foreach (Room r in selectedRooms)
            RestoreRoomGroupColor(r);

        selectedRooms.Clear();
    }
}
