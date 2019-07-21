using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class MapManager : MonoBehaviour
{
    const int DEFAULT_GRID_SIZE = 12;

    [Range(8, 20)]
    public int gridSize = DEFAULT_GRID_SIZE;

    private int _oldGridSize;
    private Transform _mapPlaneTransform;
    private Renderer _mapPlaneRenderer;
    private Grid _grid;
    private Vector2 _startPosition = Vector2.zero;

    void Awake()
    {
        _mapPlaneTransform = transform.Find("MapPlane");
        _mapPlaneRenderer = _mapPlaneTransform.gameObject.GetComponent<MeshRenderer>();
        _oldGridSize = gridSize;
    }

#if UNITY_EDITOR
    void Update()
    {
        if (EditorApplication.isPlaying)
        {
            gridSize = _oldGridSize;
            return;
        }

        if(gridSize != _oldGridSize)
        {
            _mapPlaneTransform.localScale = new Vector3(gridSize, 1, gridSize);
            _mapPlaneRenderer.sharedMaterial.SetInt("_GridSize", gridSize);
            Camera.main.orthographicSize = 5.0f * gridSize;
            _oldGridSize = gridSize;
        }
    }
#endif

    public void Init()
    {
        _grid = new Grid(gridSize);
        float extents = ((float)gridSize / 2.0f) * 10;
        _startPosition = new Vector2(transform.position.x - extents, transform.position.z - extents);

        //Debug.Log(WorldToGridPosition(transform.Find("/Bench").transform.position));
    }

    private Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        float x = worldPosition.x - _startPosition.x;
        float y = worldPosition.z - _startPosition.y;

        return new Vector2Int(Mathf.FloorToInt(x/10), Mathf.FloorToInt(y/10));
    }

    private Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        Vector3 worldPosition = Vector3.zero;
        worldPosition.x = gridPosition.x * 10 + _startPosition.x + 5;
        worldPosition.z = gridPosition.y * 10 + _startPosition.y + 5;
        return worldPosition;
    }

    public bool TryToSetBuilding(Building building)
    {
        Vector2Int gridPosition = GetGridPosition(building);
        if(_grid.IsOutOfGrid(gridPosition, building.size))
        {
            building.Remove();
        }
        else if(_grid.AddObject(gridPosition, building.size))
        {
            SetBuildingOnPosition(building, gridPosition);
            return true;
        }
        return false;
    }

    public void RemoveBuildingFromMap(Building building)
    {
        Vector2Int gridPosition = GetGridPosition(building);
        _grid.RemoveObject(gridPosition, building.size);
    }

    private Vector2Int GetGridPosition(Building building)
    {
        Vector3 buildingWorldPosition = building.transform.position;
        float offsetX = ((float)(building.size.x - 1) / 2.0f) * 10;
        float offsetY = ((float)(building.size.y - 1) / 2.0f) * 10;
        buildingWorldPosition.x -= offsetX;
        buildingWorldPosition.z -= offsetY;

        return WorldToGridPosition(buildingWorldPosition);
    }

    private void SetBuildingOnPosition(Building building, Vector2Int gridPosition)
    {
        Vector3 buildingWorldPosition = GridToWorldPosition(gridPosition);
        float offsetX = ((float)(building.size.x - 1) / 2.0f) * 10;
        float offsetY = ((float)(building.size.y - 1) / 2.0f) * 10;
        buildingWorldPosition.x += offsetX;
        buildingWorldPosition.z += offsetY;
        building.transform.position = buildingWorldPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(_startPosition.x, 0, _startPosition.y), 1);
    }
}
