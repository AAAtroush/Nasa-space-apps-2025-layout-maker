using UnityEngine;

public class GridManagerALT : MonoBehaviour
{
    public int rows;
    public int cols;
    public SlotScript[,] slots;

    [Header("References")]
    public RectTransform gridParent;   // drag your GridPatternImage here
    public RectTransform itemsContainer;

    void Awake()
    {
        slots = new SlotScript[rows, cols];

        // Get all SlotScript components from the grid
        SlotScript[] allSlots = gridParent.GetComponentsInChildren<SlotScript>();

        for (int i = 0; i < allSlots.Length; i++)
        {
            int row = i / cols;
            int col = i % cols;

            allSlots[i].row = row;
            allSlots[i].col = col;
            allSlots[i].gridManager = this;

            slots[row, col] = allSlots[i];
        }
    }

    public bool CanPlaceItem(DraggableItem item, int startRow, int startCol)
    {
        if (startRow + item.height > rows || startCol + item.width > cols)
            return false;

        for (int r = 0; r < item.height; r++)
        {
            for (int c = 0; c < item.width; c++)
            {
                if (slots[startRow + r, startCol + c].occupiedBy != null)
                    return false;
            }
        }

        return true;
    }

    public void PlaceItem(DraggableItem item, int startRow, int startCol)
    {
        // Clear old slots
        foreach (var s in item.occupiedSlots)
            s.occupiedBy = null;
        item.occupiedSlots.Clear();

        // Fill new slots
        for (int r = 0; r < item.height; r++)
        {
            for (int c = 0; c < item.width; c++)
            {
                SlotScript slot = slots[startRow + r, startCol + c];
                slot.occupiedBy = item;
                item.occupiedSlots.Add(slot);
            }
        }

        // Parent item under ItemsContainer
        item.transform.SetParent(itemsContainer);

        // Get slot + item rects
        RectTransform slotRect = slots[startRow, startCol].GetComponent<RectTransform>();
        RectTransform itemRect = item.GetComponent<RectTransform>();

        float cellWidth = slotRect.rect.width;
        float cellHeight = slotRect.rect.height;

        // Resize item to cover correct area
        itemRect.sizeDelta = new Vector2(cellWidth * item.width, cellHeight * item.height);

        // Position to match slot center
        itemRect.position = slotRect.position;
    }
}
