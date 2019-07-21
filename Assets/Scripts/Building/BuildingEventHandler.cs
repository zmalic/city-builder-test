using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Building))]
public class BuildingEventHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Building _building;

    void Awake()
    {
        _building = GetComponent<Building>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
