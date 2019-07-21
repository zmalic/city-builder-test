using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BuildingUI))]
public class Building : MonoBehaviour
{
    /// <summary>
    /// Building states
    /// </summary>
    public enum State
    {
        Instantiated,
        Construction,
        Ready,
        Removing
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

        // After the building was created start placeing
        StartCoroutine(PlaceBuilding());
    }

    IEnumerator PlaceBuilding()
    {
        placeing = true;
        Plane xzPlane = new Plane(Vector3.up, 0);
        // move building on the mouse position until it is placed
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
                // on left mouse click, if the building is placecesd on map start construction
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
        _state = State.Removing;
        Destroy(gameObject);
    }

    public bool Removing()
    {
        return _state == State.Removing;
    }

    public void Construct()
    {
        GameManager.instance.resourcesManager.Buy(price);
        placeing = false;
        // change state (after that coroutine PlaceBuilding will stop)
        _state = State.Construction;
        StartCoroutine(ConstructBuilding());
    }

    /// <summary>
    /// Start building construction (10 seconds)
    /// </summary>
    /// <returns></returns>
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

        // if the building can produce resources - set it up
        if (production.resourceType != ResourceType.None)
        {
            if(autoProduction)
                StartCoroutine(StartProduction());
            else
            {
                // if the building has manual resource production, set startProduction button listener
                _ui.startProduction.gameObject.SetActive(true);
                _ui.startProduction.onClick.AddListener(() => StartCoroutine(StartProduction()));
            }
        }
    }


    /// <summary>
    /// Starts building production (10s)
    /// </summary>
    /// <returns></returns>
    IEnumerator StartProduction()
    {
        _ui.productionProgress.gameObject.SetActive(true);
        _ui.startProduction.gameObject.SetActive(false);
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
        while (autoProduction);  // if autoProduction repeat forever
        _ui.productionProgress.gameObject.SetActive(false);
        if(!autoProduction)
        {
            _ui.startProduction.gameObject.SetActive(true);
        }
    }

    public void Select()
    {
        // if this building is selected, deselect it
        if (_selected != this)
        {
            // if any building is selected, diselect it and select this building
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
        // if there is selected building, deselect it
        _selected?.Deselect();
    }
}
