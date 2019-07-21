using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Takes care of buildings creation
/// </summary>
public class BuildingManager : MonoBehaviour
{
    public void BuyBuilding(Building building)
    {
        if (Building.placeing)
            return;

        ResourcesManager resourcesManager = GameManager.instance.resourcesManager;
        MapManager mapManager = GameManager.instance.mapManager;
        if (resourcesManager.HasEnough(building.price))
        {
            Instantiate(building.gameObject, mapManager.transform);
        }
        else
        {
            Debug.Log("There is not enough resources");
        }
    }
}
