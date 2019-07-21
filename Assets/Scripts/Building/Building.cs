using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BuildingUI))]
public class Building : MonoBehaviour
{
    public enum State
    {
        Instantiated,
        Construction,
        Ready
    }

    public string description;
    public ResourceAmount production;
    public bool autoProduction;
    public Vector2Int size = Vector2Int.one;
    public ResourceAmount[] price;

    private BuildingUI _ui;
    private State _state;

    public static bool placeing = false;
    private static Building _selected;

    void Awake()
    {
        _ui = GetComponent<BuildingUI>();
        _state = State.Instantiated;
        _ui.description.text = description;
        StartCoroutine(PlaceBuilding());
    }

    IEnumerator PlaceBuilding()
    {
        placeing = true;
        Plane xzPlane = new Plane(Vector3.up, 0);
        while (_state == State.Instantiated)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter;
            if (xzPlane.Raycast(ray, out enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                transform.position = hitPoint;

            }
            if (Input.GetMouseButtonDown(0))
            {
                if(AddOnMap())
                    Construct();
            }
            yield return null;
        }
    }

    public bool AddOnMap()
    {
        return GameManager.instance.mapManager.TryToSetBuilding(this);
    }

    public void RemoveFromMap()
    {
        GameManager.instance.mapManager.RemoveBuildingFromMap(this);
    }

    public void Remove()
    {
        placeing = false;
        Destroy(gameObject);
    }
    public void Construct()
    {
        GameManager.instance.resourcesManager.Buy(price);
        placeing = false;
        _state = State.Construction;
        StartCoroutine(ConstructBuilding());
    }

    IEnumerator ConstructBuilding()
    {
        float constructBegining = Time.time;
        _ui.constructionProgress.gameObject.SetActive(true);
        float progress = 0;
        while (progress < 1)
        {
            _ui.constructionProgress.value = progress;
            yield return null;
            progress = (Time.time - constructBegining) / 10;
        }
        _ui.constructionProgress.gameObject.SetActive(false);
        _state = State.Ready;
        if (autoProduction && production.resourceType != ResourceType.None)
        {
            StartCoroutine(StartProduction());
        }
    }



    IEnumerator StartProduction()
    {
        _ui.productionProgress.gameObject.SetActive(true);
        do
        {
            float productionBegining = Time.time;
            float progress = 0;
            while (progress < 1)
            {
                _ui.productionProgress.value = progress;
                yield return null;
                progress = (Time.time - productionBegining) / 10;
            }
            yield return null;
            GameManager.instance.resourcesManager.Increase(production.resourceType, production.amount);
        }
        while (autoProduction);
        _ui.productionProgress.gameObject.SetActive(false);
    }

    public void Select()
    {
        if (_selected != this)
        {
            DeselectAll();
            _ui.selectedPanel.SetActive(true);
            _selected = this;
        }
        else
            Deselect();
    }

    public void Deselect()
    {
        _ui.selectedPanel.SetActive(false);
        _selected = null;
    }

    public static void DeselectAll()
    {
        _selected?.Deselect();
    }
}
