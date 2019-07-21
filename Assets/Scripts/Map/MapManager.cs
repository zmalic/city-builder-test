using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Here you can define map size (grid) in edit mode,
/// also, there are set of functions for adding and removing buildings on the map (grid)
/// </summary>
[ExecuteInEditMode]
public class MapManager : MonoBehaviour
{
    [Range(8, 20)]
    public int gridSize = 12;

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
        // if you are in the play mode, force _oldGridSize
        if (EditorApplication.isPlaying)
        {
            gridSize = _oldGridSize;
            return;
        }

        if(gridSize != _oldGridSize)
        {
            // when in the edit mode we change grid size
            // change pam plane scale, _GridSize parameter in map material
            // and cameras ortographic size (because we want to see the whole map)
            _mapPlaneTransform.localScale = new Vector3(gridSize, 1, gridSize);
            _mapPlaneRenderer.sharedMaterial.SetInt("_GridSize", gridSize);
            Camera.main.orthographicSize = 5.0f * gridSize;
            _oldGridSize = gridSize;
        }
    }
#endif

    public void Init()
    {
        // Create Grid component for care of the grid tiles usages
        _grid = new Grid(gridSize);
        float extents = ((float)gridSize / 2.0f) * 10;
        // 2D world coordinates of grids start position (XZ world)
        _startPosition = new Vector2(transform.position.x - extents, transform.position.z - extents);
    }

    /// <summary>
    /// Transform 3D point from world space to 2D int point in grid space
    /// </summary>
    /// <param name="worldPosition"></param>
    /// <returns></returns>
    private Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        float x = worldPosition.x - _startPosition.x;
        float y = worldPosition.z - _startPosition.y;

        return new Vector2Int(Mathf.FloorToInt(x/10), Mathf.FloorToInt(y/10));
    }

    /// <summary>
    /// Transform 2D int point from grid space to 3D world space point 
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    private Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        Vector3 worldPosition = Vector3.zero;
        worldPosition.x = gridPosition.x * 10 + _startPosition.x + 5;
        worldPosition.z = gridPosition.y * 10 + _startPosition.y + 5;
        return worldPosition;
    }

    /// <summary>
    /// Set building to grid if it is possible and return true,
    /// otherwise return false
    /// Remove building if the building position is outside of the grid
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Calculate 2D int grid position of the building
    /// </summary>
    /// <param name="building"></param>
    /// <returns></returns>
    private Vector2Int GetGridPosition(Building building)
    {
        Vector3 buildingWorldPosition = building.transform.position;
        float offsetX = ((float)(building.size.x - 1) / 2.0f) * 10;
        float offsetY = ((float)(building.size.y - 1) / 2.0f) * 10;
        buildingWorldPosition.x -= offsetX;
        buildingWorldPosition.z -= offsetY;

        return WorldToGridPosition(buildingWorldPosition);
    }

    /// <summary>
    /// Move buildings transform to grid position
    /// </summary>
    /// <param name="building"></param>
    /// <param name="gridPosition"></param>
    private void SetBuildingOnPosition(Building building, Vector2Int gridPosition)
    {
        Vector3 buildingWorldPosition = GridToWorldPosition(gridPosition);
        float offsetX = ((float)(building.size.x - 1) / 2.0f) * 10;
        float offsetY = ((float)(building.size.y - 1) / 2.0f) * 10;
        buildingWorldPosition.x += offsetX;
        buildingWorldPosition.z += offsetY;
        building.transform.position = buildingWorldPosition;
    }
}
