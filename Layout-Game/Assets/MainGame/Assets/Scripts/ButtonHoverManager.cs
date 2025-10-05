using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Button Settings")]
    public int buttonIndex;
    
    [Header("Game Objects to Control")]
    public List<GameObject> objectsToShow = new List<GameObject>();
    
    [Header("Optional - Auto find parent manager")]
    public HoverManager hoverManager;
    
    private void Start()
    {
        // Auto-find the parent manager if not assigned
        if (hoverManager == null)
            hoverManager = GetComponentInParent<HoverManager>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverManager?.ShowObjectsForIndex(buttonIndex);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        hoverManager?.HideAllObjects();
    }
}