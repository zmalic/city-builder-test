using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameMode
{
    Regular,
    Build
}

/// <summary>
/// Main manager where we can to set up game
/// Game manager require Resourse and Building manager scripts
/// GameManager has also references on UI and Map managers
/// </summary>
[RequireComponent(typeof(ResourcesManager))]
[RequireComponent(typeof(BuildingManager))]
public class GameManager : MonoBehaviour
{
    public ResourceAmount[] resources;
    public GameObject[] buildings;

    // Managers
    public ResourcesManager resourcesManager { get; private set; }
    public MapManager mapManager { get; private set;}
    public UIManager uiManager { get; private set;}
    public BuildingManager buildingManager { get; private set;}


    public GameMode gameMode { get; private set;}


    /// <summary>
    /// Singletone instance
    /// we will use this event in UIManager
    /// </summary>
    public static GameManager instance = null;

    /// <summary>
    /// Change mode event
    /// </summary>
    /// <param name="gameMode"></param>
    public delegate void OnGameModeChange(GameMode gameMode);
    public static event OnGameModeChange onGameModeChange;

    void Awake()
    {
        // Singletone
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        // initialize
        InitGame();
    }

    void InitGame()
    {
        SetInstances();

        // Init managers
        resourcesManager.Init();
        mapManager.Init();
        uiManager.Init();

        RegularMode();
    }

    /// <summary>
    /// Set references for all managers
    /// </summary>
    private void SetInstances()
    {
        resourcesManager = GetComponent<ResourcesManager>();
        buildingManager = GetComponent<BuildingManager>();
        mapManager = transform.Find("/Map").gameObject.GetComponent<MapManager>();
        uiManager = transform.Find("/Canvas").gameObject.GetComponent<UIManager>();
    }


    /// <summary>
    /// Go to build mode
    /// </summary>
    public void BuildMode()
    {
        gameMode = GameMode.Build;
        onGameModeChange?.Invoke(gameMode);
    }

    /// <summary>
    /// Go to regular mode
    /// </summary>
    public void RegularMode()
    {
        gameMode = GameMode.Regular;
        onGameModeChange?.Invoke(gameMode);
    }
}
