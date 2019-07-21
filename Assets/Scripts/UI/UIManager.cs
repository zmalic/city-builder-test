using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager for main UI components (screen space canvas) 
/// </summary>
public class UIManager : MonoBehaviour
{
    public GameObject buildModePanel;
    public GameObject buildingBuyButtonPrefab;

    public Text gold;
    public Text wood;
    public Text steel;

    private void OnEnable()
    {
        // Add event listeners
        GameManager.onGameModeChange += OnGameModeChange;
        ResourcesManager.onResourceChange += OnResourceChange;
    }
    private void OnDisable()
    {
        // Remove event listeners
        GameManager.onGameModeChange -= OnGameModeChange;
        ResourcesManager.onResourceChange -= OnResourceChange;
    }

    public void Init()
    {
        // Populate resource panel with the initial resource amounts
        OnResourceChange(GameManager.instance.resourcesManager.amounts);

        // Populate Build mode panel with buttons for buildings instantiation
        foreach (GameObject gameObject in GameManager.instance.buildings)
        {
            Building building = gameObject.GetComponent<Building>();
            GameObject button = Instantiate(buildingBuyButtonPrefab, buildModePanel.transform);
            BuildingBuyButton buildingBuyButton = button.GetComponent<BuildingBuyButton>();
            buildingBuyButton.Init(building);
        }
    }

    private void OnGameModeChange(GameMode gameMode)
    {
        // Show/hide menu Build mode panel
        buildModePanel.SetActive(gameMode == GameMode.Build);
        // When go to Build mode, deselect selected building
        if (buildModePanel.activeSelf)
            Building.DeselectAll();
    }

    /// <summary>
    /// On resources change populate resource panel 
    /// </summary>
    /// <param name="amounts"></param>
    private void OnResourceChange(Dictionary<ResourceType, int> amounts)
    {
        gold.text = string.Format("[{0}]", amounts[ResourceType.Gold]);
        wood.text = string.Format("[{0}]", amounts[ResourceType.Wood]);
        steel.text = string.Format("[{0}]", amounts[ResourceType.Steel]);
    }
}
