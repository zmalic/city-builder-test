using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject buildModePanel;
    public GameObject buildingBuyButtonPrefab;

    public Text gold;
    public Text wood;
    public Text steel;

    private void OnEnable()
    {
        GameManager.onGameModeChange += OnGameModeChange;
        ResourcesManager.onResourceChange += OnResourceChange;
    }
    private void OnDisable()
    {
        GameManager.onGameModeChange -= OnGameModeChange;
        ResourcesManager.onResourceChange -= OnResourceChange;
    }
    public void Init()
    {
        OnResourceChange(GameManager.instance.resourcesManager.amounts);

        foreach(GameObject gameObject in GameManager.instance.buildings)
        {
            Building building = gameObject.GetComponent<Building>();
            GameObject button = Instantiate(buildingBuyButtonPrefab, buildModePanel.transform);
            BuildingBuyButton buildingBuyButton = button.GetComponent<BuildingBuyButton>();
            buildingBuyButton.Init(building);
        }
    }

    private void OnGameModeChange(GameMode gameMode)
    {
        buildModePanel.SetActive(gameMode == GameMode.Build);
        if (buildModePanel.activeSelf)
            Building.DeselectAll();
    }

    private void OnResourceChange(Dictionary<ResourceType, int> amounts)
    {
        gold.text = string.Format("[{0}]", amounts[ResourceType.Gold]);
        wood.text = string.Format("[{0}]", amounts[ResourceType.Wood]);
        steel.text = string.Format("[{0}]", amounts[ResourceType.Steel]);
    }
}
