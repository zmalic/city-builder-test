using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log("There are not enough resources");
        }
    }
}
