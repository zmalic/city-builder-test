using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Building event handler (select, drag'n'drop)
/// </summary>
[RequireComponent(typeof(Building))]
public class BuildingEventHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Building _building;
    private Vector3 _startPosition;
    private Plane _xzPlane;

    void Awake()
    {
        _building = GetComponent<Building>();
        _xzPlane = new Plane(Vector3.up, 0);
    }

    /// <summary>
    /// Select building
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!IsBuildMode() && eventData.button == PointerEventData.InputButton.Left)
        {
            _building.Select();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsBuildMode())
        {
            // keep start position in the case we can't find empty space for the building 
            _startPosition = transform.position;

            // free grid space
            _building.RemoveFromMap();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsBuildMode())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;
            if (_xzPlane.Raycast(ray, out enter))
            {
                // move building
                Vector3 hitPoint = ray.GetPoint(enter);
                transform.position = hitPoint;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsBuildMode())
        {
            if (!_building.AddOnMap() && !_building.Removing())
            {
                // if there is no free space return building to start position
                transform.position = _startPosition;
                _building.AddOnMap();
            }
        }
    }

    private bool IsBuildMode()
    {
        return GameManager.instance.gameMode == GameMode.Build;
    }
}
