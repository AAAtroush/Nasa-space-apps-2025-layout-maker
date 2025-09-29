using UnityEngine;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IDropHandler
{
    public int row;
    public int col;
    public GridManagerALT gridManager;

    [HideInInspector] public DraggableItem occupiedBy; // <-- NEW

    public void OnDrop(PointerEventData eventData)
    {
        DraggableItem item = eventData.pointerDrag.GetComponent<DraggableItem>();

        if (gridManager.CanPlaceItem(item, row, col))
        {
            gridManager.PlaceItem(item, row, col);
        }
    }
}
