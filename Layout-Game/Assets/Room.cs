using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Room : MonoBehaviour
{
    public Vector2Int gridPos;
    public List<Room> neighbours = new List<Room>();

    public enum RoomTag { Empty, Corridor, Treasure, Boss, Start }
    public RoomTag tag = RoomTag.Empty;

    private Color baseColor = Color.white;  // the “true” color of the room/group
    private Renderer rend;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            baseColor = rend.material.color;
    }

    public void SetColor(Color c)
    {
        baseColor = c;            // store as base
        if (rend == null) rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = c;
    }

    public Color GetColor() => baseColor;

    public void Highlight(Color c)
    {
        if (rend == null) rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = c;
    }

    public void Unhighlight()
    {
        if (rend == null) rend = GetComponent<Renderer>();
        if (rend != null)
            rend.material.color = baseColor;  // restore the current base color
    }

    private void OnMouseDown()
    {
        Debug.Log($"Clicked on room at {gridPos}");
    }
}
