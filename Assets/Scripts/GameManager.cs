using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameMode
{
    Regular,
    Build
}


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



    public static GameManager instance = null;

    public delegate void OnGameModeChange(GameMode gameMode);
    public static event OnGameModeChange onGameModeChange;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        InitGame();
    }

    void InitGame()
    {
        SetInstances();
        resourcesManager.Init();
        mapManager.Init();
        uiManager.Init();

        RegularMode();
    }

    private void SetInstances()
    {
        resourcesManager = GetComponent<ResourcesManager>();
        buildingManager = GetComponent<BuildingManager>();
        mapManager = transform.Find("/Map").gameObject.GetComponent<MapManager>();
        uiManager = transform.Find("/Canvas").gameObject.GetComponent<UIManager>();
    }

    public void BuildMode()
    {
        gameMode = GameMode.Build;
        onGameModeChange?.Invoke(gameMode);
    }

    public void RegularMode()
    {
        gameMode = GameMode.Regular;
        onGameModeChange?.Invoke(gameMode);
    }
}
